"""Independent verification for the Orange Wine Quality polynomial workflows.

The data files come from the exact URLs used by Orange's official Datasets
catalogue.  The model code intentionally uses scikit-learn only, so it can be
run with the project's normal Python environment without importing Orange.
"""

from __future__ import annotations

import csv
import json
import math
import time
from dataclasses import asdict, dataclass
from pathlib import Path
from typing import Final
from urllib.request import urlopen

import numpy as np
import pandas as pd
from sklearn.linear_model import LinearRegression
from sklearn.metrics import mean_absolute_error, mean_squared_error, r2_score
from sklearn.model_selection import ShuffleSplit
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import PolynomialFeatures


PROJECT_ROOT: Final = Path(__file__).resolve().parents[1]
DATA_DIR: Final = PROJECT_ROOT / "orange" / "data"
OUTPUT_DIR: Final = PROJECT_ROOT / "outputs" / "wine-polynomial"
TARGET: Final = "quality"
RANDOM_STATE: Final = 42
TRAIN_PROPORTION: Final = 0.70
DEGREES: Final = (2, 3)

DATASETS: Final = {
    "red": {
        "title": "Wine quality - red",
        "catalog_id": "winequality-red.tab",
        "url": "https://datasets.biolab.si/winequality-red.tab",
        "expected_rows": 1599,
    },
    "white": {
        "title": "Wine quality - white",
        "catalog_id": "winequality-white.tab",
        "url": "https://datasets.biolab.si/winequality-white.tab",
        "expected_rows": 4898,
    },
}


@dataclass(frozen=True)
class MetricRow:
    dataset: str
    degree: int
    model: str
    random_state: int
    train_rows: int
    test_rows: int
    features_before: int
    features_after: int
    mae: float
    rmse: float
    r2: float
    fit_seconds: float


@dataclass(frozen=True)
class DatasetSummary:
    dataset: str
    title: str
    catalog_id: str
    source_url: str
    rows: int
    columns_total: int
    features: int
    target: str
    target_kind: str
    missing_values: int
    train_rows: int
    test_rows: int


def ensure_catalogue_file(catalog_id: str, url: str) -> Path:
    """Cache an unchanged copy of an official Orange catalogue data file."""

    DATA_DIR.mkdir(parents=True, exist_ok=True)
    destination = DATA_DIR / catalog_id
    if destination.is_file() and destination.stat().st_size > 0:
        return destination

    temporary = destination.with_suffix(destination.suffix + ".download")
    try:
        with urlopen(url, timeout=60) as response, temporary.open("wb") as stream:
            stream.write(response.read())
        if temporary.stat().st_size == 0:
            raise ValueError(f"Downloaded empty dataset: {url}")
        temporary.replace(destination)
    finally:
        temporary.unlink(missing_ok=True)
    return destination


def load_orange_tab(path: Path) -> pd.DataFrame:
    """Read the three-header-row Orange .tab format without Orange imports."""

    with path.open("r", encoding="utf-8", newline="") as stream:
        reader = csv.reader(stream, delimiter="\t")
        names = next(reader)
        types = next(reader)
        roles = next(reader)

    if len(names) != len(types) or len(names) != len(roles):
        raise ValueError(f"Invalid Orange .tab header in {path}")
    if TARGET not in names:
        raise ValueError(f"{path.name} does not contain target {TARGET!r}")
    target_index = names.index(TARGET)
    if roles[target_index].strip().lower() != "class":
        raise ValueError(f"{path.name}: quality is not marked as the class/target")
    if types[target_index].strip().lower() not in {"c", "continuous"}:
        raise TypeError(f"{path.name}: quality is not continuous")

    frame = pd.read_csv(path, sep="\t", header=0, skiprows=[1, 2])
    for column in frame.columns:
        frame[column] = pd.to_numeric(frame[column], errors="raise")
    return frame


def orange_compatible_split(n_rows: int) -> tuple[np.ndarray, np.ndarray]:
    """Reproduce Orange Data Sampler's 70%/seed-42 fixed-proportion split."""

    train_size = int(math.ceil(n_rows * TRAIN_PROPORTION))
    splitter = ShuffleSplit(
        n_splits=1,
        test_size=train_size,
        random_state=RANDOM_STATE,
    )
    remaining_indices, sample_indices = next(splitter.split(np.empty((n_rows, 1))))
    return sample_indices, remaining_indices


def validate_frame(name: str, frame: pd.DataFrame, expected_rows: int) -> None:
    if len(frame) != expected_rows:
        raise ValueError(f"{name}: expected {expected_rows} rows, found {len(frame)}")
    if frame.columns.duplicated().any():
        raise ValueError(f"{name}: duplicate column names")
    if TARGET not in frame.columns:
        raise ValueError(f"{name}: target quality is missing")
    if int(frame.isna().sum().sum()) != 0:
        raise ValueError(f"{name}: missing values are not expected")
    if not all(pd.api.types.is_numeric_dtype(dtype) for dtype in frame.dtypes):
        raise TypeError(f"{name}: every feature and target must be numeric")


def evaluate_dataset(name: str, frame: pd.DataFrame) -> tuple[list[MetricRow], DatasetSummary]:
    feature_names = [column for column in frame.columns if column != TARGET]
    if TARGET in feature_names:
        raise AssertionError("Target leakage: quality is present in feature_names")

    X = frame.loc[:, feature_names].to_numpy(dtype=float)
    y = frame.loc[:, TARGET].to_numpy(dtype=float)
    train_indices, test_indices = orange_compatible_split(len(frame))
    X_train, X_test = X[train_indices], X[test_indices]
    y_train, y_test = y[train_indices], y[test_indices]

    metrics: list[MetricRow] = []
    for degree in DEGREES:
        pipeline = Pipeline(
            steps=[
                (
                    "polynomial",
                    PolynomialFeatures(degree=degree, include_bias=False),
                ),
                ("linear", LinearRegression()),
            ]
        )
        started = time.perf_counter()
        pipeline.fit(X_train, y_train)
        fit_seconds = time.perf_counter() - started
        predicted = pipeline.predict(X_test)
        transformed = pipeline.named_steps["polynomial"].transform(X_train[:1])

        values = {
            "mae": float(mean_absolute_error(y_test, predicted)),
            "rmse": float(mean_squared_error(y_test, predicted) ** 0.5),
            "r2": float(r2_score(y_test, predicted)),
        }
        if not all(np.isfinite(value) for value in values.values()):
            raise ValueError(f"{name} degree {degree}: non-finite metric")
        if not np.isfinite(predicted).all():
            raise ValueError(f"{name} degree {degree}: non-finite prediction")

        metrics.append(
            MetricRow(
                dataset=name,
                degree=degree,
                model="ordinary_polynomial_linear_regression",
                random_state=RANDOM_STATE,
                train_rows=len(train_indices),
                test_rows=len(test_indices),
                features_before=X.shape[1],
                features_after=transformed.shape[1],
                mae=values["mae"],
                rmse=values["rmse"],
                r2=values["r2"],
                fit_seconds=float(fit_seconds),
            )
        )

    spec = DATASETS[name]
    summary = DatasetSummary(
        dataset=name,
        title=str(spec["title"]),
        catalog_id=str(spec["catalog_id"]),
        source_url=str(spec["url"]),
        rows=len(frame),
        columns_total=len(frame.columns),
        features=len(feature_names),
        target=TARGET,
        target_kind="continuous numeric (ordinal score treated as regression)",
        missing_values=int(frame.isna().sum().sum()),
        train_rows=len(train_indices),
        test_rows=len(test_indices),
    )
    return metrics, summary


def write_outputs(metrics: list[MetricRow], summaries: list[DatasetSummary]) -> None:
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
    metric_records = [asdict(row) for row in metrics]
    summary_records = [asdict(row) for row in summaries]

    pd.DataFrame(metric_records).to_csv(OUTPUT_DIR / "metrics.csv", index=False)
    pd.DataFrame(summary_records).to_csv(
        OUTPUT_DIR / "dataset-summary.csv", index=False
    )
    payload = {
        "protocol": {
            "train_proportion": TRAIN_PROPORTION,
            "test_proportion": 0.30,
            "random_state": RANDOM_STATE,
            "degrees": list(DEGREES),
            "selection_policy": (
                "Both required degrees are reported; test metrics are not used "
                "to retune or select a final model."
            ),
        },
        "datasets": summary_records,
        "metrics": metric_records,
    }
    (OUTPUT_DIR / "metrics.json").write_text(
        json.dumps(payload, ensure_ascii=False, indent=2) + "\n",
        encoding="utf-8",
    )


def main() -> None:
    all_metrics: list[MetricRow] = []
    summaries: list[DatasetSummary] = []
    for name, spec in DATASETS.items():
        path = ensure_catalogue_file(str(spec["catalog_id"]), str(spec["url"]))
        frame = load_orange_tab(path)
        validate_frame(name, frame, int(spec["expected_rows"]))
        metrics, summary = evaluate_dataset(name, frame)
        all_metrics.extend(metrics)
        summaries.append(summary)

    write_outputs(all_metrics, summaries)
    print(json.dumps({"datasets": len(summaries), "models": len(all_metrics)}))
    for row in all_metrics:
        print(
            f"{row.dataset:5s} d={row.degree}: "
            f"MAE={row.mae:.6f}, RMSE={row.rmse:.6f}, R2={row.r2:.6f}, "
            f"train/test={row.train_rows}/{row.test_rows}, "
            f"features={row.features_before}->{row.features_after}"
        )


if __name__ == "__main__":
    main()
