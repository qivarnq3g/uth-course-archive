"""Đánh giá DecisionTreeRegressor trên Ames Housing như phần mở rộng."""

from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any, Mapping, Sequence

import matplotlib

matplotlib.use("Agg")

import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from sklearn.compose import ColumnTransformer
from sklearn.impute import SimpleImputer
from sklearn.metrics import mean_absolute_error, mean_squared_error, r2_score
from sklearn.model_selection import GridSearchCV, KFold, train_test_split
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import OneHotEncoder
from sklearn.tree import DecisionTreeRegressor, plot_tree

PROJECT_ROOT = Path(__file__).resolve().parents[1]
DEFAULT_DATA_PATH = (
    PROJECT_ROOT / "bai_5_2_house_price_prediction" / "data" / "raw" / "ames.csv"
)
DEFAULT_OUTPUT_DIR = PROJECT_ROOT / "outputs" / "regression-tree" / "ames-extension"
DEFAULT_ORANGE_DATA = PROJECT_ROOT / "orange" / "data" / "ames_regression.csv"
TARGET_COLUMN = "price"
RANDOM_STATE = 42
TEST_SIZE = 0.30

NUMERIC_FEATURES = [
    "area",
    "Lot.Area",
    "Overall.Qual",
    "Overall.Cond",
    "Year.Built",
    "Year.Remod.Add",
    "Full.Bath",
    "Half.Bath",
    "Bedroom.AbvGr",
    "TotRms.AbvGrd",
    "Fireplaces",
    "Garage.Cars",
    "Garage.Area",
]
CATEGORICAL_FEATURES = [
    "MS.Zoning",
    "Neighborhood",
    "Bldg.Type",
    "House.Style",
    "Kitchen.Qual",
    "Sale.Condition",
]
FEATURE_COLUMNS = NUMERIC_FEATURES + CATEGORICAL_FEATURES

DEFAULT_PARAM_GRID: Mapping[str, Sequence[Any]] = {
    "model__max_depth": (4, 6, 8, 10, None),
    "model__min_samples_split": (2, 10, 20),
    "model__min_samples_leaf": (1, 5, 10),
}


def load_ames_data(
    data_path: Path = DEFAULT_DATA_PATH,
) -> tuple[pd.DataFrame, pd.Series, pd.DataFrame]:
    """Đọc Ames, kiểm tra feature/target và bỏ duy nhất dòng target không hợp lệ."""
    data_path = Path(data_path)
    if not data_path.is_file():
        raise FileNotFoundError(
            f"Không tìm thấy Ames CSV: {data_path}. Giữ file ở vị trí gốc hoặc dùng --data."
        )

    raw = pd.read_csv(data_path)
    required = set(FEATURE_COLUMNS + [TARGET_COLUMN])
    missing_columns = sorted(required.difference(raw.columns))
    if missing_columns:
        raise ValueError(f"Ames thiếu cột bắt buộc: {missing_columns}")

    target = pd.to_numeric(raw[TARGET_COLUMN], errors="coerce")
    valid_target = target.notna() & np.isfinite(target)
    features = raw.loc[valid_target, FEATURE_COLUMNS].copy()
    target = target.loc[valid_target].astype(float).rename(TARGET_COLUMN)

    if features.empty:
        raise ValueError("Không còn dòng Ames nào có target số hợp lệ.")
    if len(features) != len(target):
        raise ValueError("Số dòng feature và target Ames không khớp.")
    return features, target, raw


def split_ames_data(
    features: pd.DataFrame,
    target: pd.Series,
) -> tuple[pd.DataFrame, pd.DataFrame, pd.Series, pd.Series]:
    """Chia Ames 70/30 với test set độc lập."""
    return train_test_split(
        features,
        target,
        test_size=TEST_SIZE,
        random_state=RANDOM_STATE,
    )


def build_pipeline() -> Pipeline:
    """Tạo pipeline impute/encode/model; không scale vì dùng Decision Tree."""
    numeric_pipeline = Pipeline(
        steps=[("imputer", SimpleImputer(strategy="median"))]
    )
    categorical_pipeline = Pipeline(
        steps=[
            ("imputer", SimpleImputer(strategy="most_frequent")),
            (
                "encoder",
                OneHotEncoder(handle_unknown="ignore", sparse_output=True),
            ),
        ]
    )
    preprocess = ColumnTransformer(
        transformers=[
            ("numeric", numeric_pipeline, NUMERIC_FEATURES),
            ("categorical", categorical_pipeline, CATEGORICAL_FEATURES),
        ]
    )
    model = DecisionTreeRegressor(random_state=RANDOM_STATE)
    return Pipeline(steps=[("preprocess", preprocess), ("model", model)])


def iqr_outlier_counts(features: pd.DataFrame) -> dict[str, int]:
    """Đếm outlier IQR để mô tả; không dùng kết quả này để lọc dữ liệu."""
    counts: dict[str, int] = {}
    for column in NUMERIC_FEATURES:
        values = pd.to_numeric(features[column], errors="coerce").dropna()
        first_quartile = values.quantile(0.25)
        third_quartile = values.quantile(0.75)
        iqr = third_quartile - first_quartile
        lower = first_quartile - 1.5 * iqr
        upper = third_quartile + 1.5 * iqr
        counts[column] = int(((values < lower) | (values > upper)).sum())
    return counts


def regression_metrics(actual: pd.Series | np.ndarray, predicted: np.ndarray) -> dict[str, float]:
    """Tính MAE, RMSE và R²."""
    return {
        "mae": float(mean_absolute_error(actual, predicted)),
        "rmse": float(np.sqrt(mean_squared_error(actual, predicted))),
        "r2": float(r2_score(actual, predicted)),
    }


def train_and_evaluate(
    data_path: Path = DEFAULT_DATA_PATH,
    output_dir: Path = DEFAULT_OUTPUT_DIR,
    orange_data_path: Path = DEFAULT_ORANGE_DATA,
    *,
    param_grid: Mapping[str, Sequence[Any]] | None = None,
    cv_folds: int = 5,
) -> dict[str, Any]:
    """Tune chỉ trên train bằng CV, đánh giá test và lưu artifact."""
    if cv_folds < 2:
        raise ValueError("cv_folds phải >= 2.")

    output_dir = Path(output_dir)
    orange_data_path = Path(orange_data_path)
    output_dir.mkdir(parents=True, exist_ok=True)
    orange_data_path.parent.mkdir(parents=True, exist_ok=True)

    features, target, raw = load_ames_data(data_path)
    x_train, x_test, y_train, y_test = split_ames_data(features, target)

    search = GridSearchCV(
        estimator=build_pipeline(),
        param_grid=dict(param_grid or DEFAULT_PARAM_GRID),
        scoring="neg_root_mean_squared_error",
        cv=KFold(n_splits=cv_folds, shuffle=True, random_state=RANDOM_STATE),
        n_jobs=-1,
        refit=True,
        return_train_score=True,
        error_score="raise",
    )
    search.fit(x_train, y_train)

    pipeline = search.best_estimator_
    train_predictions = pipeline.predict(x_train)
    test_predictions = pipeline.predict(x_test)
    test_metrics = regression_metrics(y_test, test_predictions)
    train_metrics = regression_metrics(y_train, train_predictions)

    best_params = {
        key.removeprefix("model__"): value
        for key, value in search.best_params_.items()
    }
    metrics: dict[str, Any] = {
        "dataset": "Ames Housing (OpenIntro file already present in workspace)",
        "dataset_role": (
            "Phần mở rộng để đánh giá Regression Tree; không phải dataset mục 7.1."
        ),
        "source_evidence": (
            "House Price được mô tả cho Linear Regression ở AI-Ex-2027.pdf, trang 16-17."
        ),
        "data_path": str(Path(data_path).resolve()),
        "raw_rows": int(raw.shape[0]),
        "raw_columns": int(raw.shape[1]),
        "usable_rows": int(len(features)),
        "selected_feature_count": int(len(FEATURE_COLUMNS)),
        "numeric_features": NUMERIC_FEATURES,
        "categorical_features": CATEGORICAL_FEATURES,
        "target": TARGET_COLUMN,
        "target_missing_or_invalid_rows": int(len(raw) - len(features)),
        "selected_feature_missing_values": {
            key: int(value)
            for key, value in features.isna().sum().items()
            if int(value) > 0
        },
        "duplicate_rows_raw": int(raw.duplicated().sum()),
        "iqr_outlier_counts_descriptive_only": iqr_outlier_counts(features),
        "outlier_policy": (
            "Chỉ audit; không lọc theo toàn dataset nên không làm rò rỉ test."
        ),
        "random_state": RANDOM_STATE,
        "test_size": TEST_SIZE,
        "train_samples": int(len(x_train)),
        "test_samples": int(len(x_test)),
        "preprocessing": (
            "Median imputation cho số; most-frequent + one-hot cho category; fit trong pipeline trên train."
        ),
        "scaling": "Không dùng; Decision Tree không cần StandardScaler.",
        "criterion": "squared_error",
        "cv_folds": cv_folds,
        "cv_scoring": "neg_root_mean_squared_error",
        "best_cv_rmse": float(-search.best_score_),
        "best_params": best_params,
        "train_metrics_reference_only": train_metrics,
        "test_metrics": test_metrics,
    }

    predictions = pd.DataFrame(
        {
            "actual": y_test,
            "predicted": test_predictions,
            "residual": y_test.to_numpy() - test_predictions,
        },
        index=y_test.index,
    ).sort_index()
    predictions.to_csv(output_dir / "predictions.csv", index_label="source_row")

    cv_frame = pd.DataFrame(search.cv_results_)
    cv_columns = [
        column
        for column in (
            "rank_test_score",
            "mean_test_score",
            "std_test_score",
            "mean_train_score",
            "param_model__max_depth",
            "param_model__min_samples_split",
            "param_model__min_samples_leaf",
        )
        if column in cv_frame.columns
    ]
    cv_frame.loc[:, cv_columns].sort_values("rank_test_score").to_csv(
        output_dir / "cv_results.csv",
        index=False,
    )

    preprocess = pipeline.named_steps["preprocess"]
    model = pipeline.named_steps["model"]
    transformed_feature_names = preprocess.get_feature_names_out()
    importance = pd.DataFrame(
        {
            "feature": transformed_feature_names,
            "importance": model.feature_importances_,
        }
    ).sort_values("importance", ascending=False)
    importance.to_csv(output_dir / "feature_importance.csv", index=False)

    display_feature_names = [
        name.replace("numeric__", "").replace("categorical__", "")
        for name in transformed_feature_names
    ]
    figure, axis = plt.subplots(figsize=(18, 10))
    plot_tree(
        model,
        feature_names=display_feature_names,
        filled=True,
        rounded=True,
        impurity=True,
        max_depth=2,
        fontsize=9,
        ax=axis,
    )
    axis.set_title("Ames Regression Tree - 3 tầng đầu của mô hình tốt nhất")
    figure.tight_layout()
    figure.savefig(output_dir / "tree_top_levels.png", dpi=180, bbox_inches="tight")
    plt.close(figure)

    figure, axis = plt.subplots(figsize=(8, 7))
    axis.scatter(y_test, test_predictions, alpha=0.65, edgecolors="none")
    minimum = float(min(y_test.min(), test_predictions.min()))
    maximum = float(max(y_test.max(), test_predictions.max()))
    axis.plot([minimum, maximum], [minimum, maximum], "--", color="black", linewidth=1.5)
    axis.set_xlabel("Giá thực tế (USD)")
    axis.set_ylabel("Giá dự đoán (USD)")
    axis.set_title("Actual vs Predicted - tập test")
    axis.grid(alpha=0.2)
    figure.tight_layout()
    figure.savefig(output_dir / "actual_vs_predicted.png", dpi=180, bbox_inches="tight")
    plt.close(figure)

    orange_frame = features.copy()
    orange_frame[TARGET_COLUMN] = target
    orange_frame.to_csv(orange_data_path, index=False)

    (output_dir / "metrics.json").write_text(
        json.dumps(metrics, ensure_ascii=False, indent=2),
        encoding="utf-8",
    )
    return metrics


def parse_args() -> argparse.Namespace:
    """Đọc tham số dòng lệnh."""
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--data", type=Path, default=DEFAULT_DATA_PATH)
    parser.add_argument("--output-dir", type=Path, default=DEFAULT_OUTPUT_DIR)
    parser.add_argument("--orange-data", type=Path, default=DEFAULT_ORANGE_DATA)
    parser.add_argument("--cv-folds", type=int, default=5)
    return parser.parse_args()


def main() -> None:
    """Điểm vào CLI."""
    args = parse_args()
    metrics = train_and_evaluate(
        data_path=args.data,
        output_dir=args.output_dir,
        orange_data_path=args.orange_data,
        cv_folds=args.cv_folds,
    )
    print(json.dumps(metrics, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()
