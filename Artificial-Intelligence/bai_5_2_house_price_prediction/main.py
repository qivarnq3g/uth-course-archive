from __future__ import annotations

import json
import sys
from io import BytesIO
from pathlib import Path
from typing import Iterable

import joblib
import matplotlib
import numpy as np
import pandas as pd
import requests
from sklearn.compose import ColumnTransformer
from sklearn.datasets import fetch_openml
from sklearn.impute import SimpleImputer
from sklearn.linear_model import LinearRegression, Ridge
from sklearn.metrics import mean_absolute_error, mean_squared_error, r2_score
from sklearn.model_selection import train_test_split
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import OneHotEncoder, StandardScaler

matplotlib.use("Agg")
import matplotlib.pyplot as plt  # noqa: E402


PROJECT_DIR = Path(__file__).resolve().parent
RAW_DIR = PROJECT_DIR / "data" / "raw"
PROCESSED_DIR = PROJECT_DIR / "data" / "processed"
RESULTS_DIR = PROJECT_DIR / "results"
FIGURES_DIR = RESULTS_DIR / "figures"
MODELS_DIR = PROJECT_DIR / "models"

RAW_DATA_PATH = RAW_DIR / "ames.csv"
PROCESSED_DATA_PATH = PROCESSED_DIR / "modeling_data.csv"
METRICS_PATH = RESULTS_DIR / "metrics.json"
MODEL_COMPARISON_PATH = RESULTS_DIR / "model_comparison.csv"
SAMPLE_PREDICTIONS_PATH = RESULTS_DIR / "sample_predictions.csv"
COEFFICIENTS_PATH = RESULTS_DIR / "model_coefficients.csv"
MODEL_PATH = MODELS_DIR / "linear_regression_model.joblib"
BEST_MODEL_PATH = MODELS_DIR / "best_model.joblib"
ACTUAL_VS_PREDICTED_PATH = FIGURES_DIR / "actual_vs_predicted.png"
RESIDUALS_PATH = FIGURES_DIR / "residuals.png"
DOWNLOAD_GUIDE_PATH = PROJECT_DIR / "DOWNLOAD_DATA.md"

OPENINTRO_DATA_URL = "https://www.openintro.org/data/csv/ames.csv"
OPENINTRO_PAGE_URL = "https://www.openintro.org/data/index.php?data=ames"
OPENINTRO_SOURCE_LABEL = "OpenIntro Ames Housing Dataset, cached at data/raw/ames.csv"

TARGET_COLUMN = "price"
FEATURE_COLUMNS = [
    "area",
    "Bedroom.AbvGr",
    "Full.Bath",
    "Half.Bath",
    "TotRms.AbvGrd",
    "Year.Built",
    "Garage.Area",
    "Neighborhood",
    "Utilities",
    "Condition.1",
    "Condition.2",
    "Overall.Qual",
    "Overall.Cond",
]

BASIC_FEATURES = [
    "area",
    "Bedroom.AbvGr",
    "Full.Bath",
    "TotRms.AbvGrd",
    "Neighborhood",
]

FEATURE_SETS = {
    "basic_features": BASIC_FEATURES,
    "full_features": FEATURE_COLUMNS,
}

RIDGE_ALPHA_VALUES = [0.1, 1.0, 10.0, 100.0]
TUNING_METRIC = "rmse"

NUMERIC_FEATURES = [
    "area",
    "Bedroom.AbvGr",
    "Full.Bath",
    "Half.Bath",
    "TotRms.AbvGrd",
    "Year.Built",
    "Garage.Area",
    "Overall.Qual",
    "Overall.Cond",
]

CATEGORICAL_FEATURES = [
    "Neighborhood",
    "Utilities",
    "Condition.1",
    "Condition.2",
]

COLUMN_ALIASES = {
    "price": ["price", "Sale_Price", "SalePrice"],
    "area": ["area", "Gr.Liv.Area", "Gr_Liv_Area", "GrLivArea"],
    "Bedroom.AbvGr": ["Bedroom.AbvGr", "Bedroom_AbvGr", "BedroomAbvGr"],
    "Full.Bath": ["Full.Bath", "Full_Bath", "FullBath"],
    "Half.Bath": ["Half.Bath", "Half_Bath", "HalfBath"],
    "TotRms.AbvGrd": ["TotRms.AbvGrd", "TotRms_AbvGrd", "TotRmsAbvGrd"],
    "Year.Built": ["Year.Built", "Year_Built", "YearBuilt"],
    "Garage.Area": ["Garage.Area", "Garage_Area", "GarageArea"],
    "Neighborhood": ["Neighborhood"],
    "Utilities": ["Utilities"],
    "Condition.1": ["Condition.1", "Condition_1", "Condition1"],
    "Condition.2": ["Condition.2", "Condition_2", "Condition2"],
    "Overall.Qual": ["Overall.Qual", "Overall_Qual", "OverallQual"],
    "Overall.Cond": ["Overall.Cond", "Overall_Cond", "OverallCond"],
}


def ensure_directories() -> None:
    for directory in [RAW_DIR, PROCESSED_DIR, RESULTS_DIR, FIGURES_DIR, MODELS_DIR]:
        directory.mkdir(parents=True, exist_ok=True)


def write_download_guide(errors: Iterable[str]) -> None:
    details = "\n".join(f"- {error}" for error in errors)
    content = f"""# Huong dan tai du lieu thu cong

Qua trinh tai du lieu tu dong khong thanh cong. Khong su dung du lieu mo phong.

## Cach tai

1. Mo trang: {OPENINTRO_PAGE_URL}
2. Tai file CSV cua Ames Housing Dataset.
3. Doi ten file thanh `ames.csv` neu can.
4. Dat file vao thu muc:

```text
data/raw/ames.csv
```

5. Chay lai:

```bash
python main.py
```

## Loi ghi nhan

{details}
"""
    DOWNLOAD_GUIDE_PATH.write_text(content, encoding="utf-8")


def read_csv_safely(path: Path) -> pd.DataFrame:
    return pd.read_csv(path, low_memory=False)


def validate_dataframe(df: pd.DataFrame, source_name: str) -> None:
    if df.empty:
        raise ValueError(f"{source_name}: dataset rong.")
    if df.shape[0] < 1000 or df.shape[1] < 20:
        raise ValueError(
            f"{source_name}: dataset co kich thuoc bat thuong {df.shape[0]} dong, {df.shape[1]} cot."
        )


def standardize_column_names(df: pd.DataFrame) -> pd.DataFrame:
    renamed = df.copy()
    rename_map: dict[str, str] = {}

    for canonical, aliases in COLUMN_ALIASES.items():
        if canonical in renamed.columns:
            continue
        for alias in aliases:
            if alias in renamed.columns:
                rename_map[alias] = canonical
                break

    if rename_map:
        renamed = renamed.rename(columns=rename_map)

    return renamed


def load_from_openintro() -> pd.DataFrame:
    headers = {
        "User-Agent": (
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) "
            "AppleWebKit/537.36 (KHTML, like Gecko) "
            "Chrome/126.0 Safari/537.36"
        ),
        "Accept": "text/csv,application/csv,text/plain,*/*",
        "Referer": OPENINTRO_PAGE_URL,
    }
    response = requests.get(OPENINTRO_DATA_URL, headers=headers, timeout=45)
    if response.status_code != 200:
        raise RuntimeError(f"OpenIntro HTTP {response.status_code}: {response.text[:200]}")

    content_type = response.headers.get("content-type", "")
    if "html" in content_type.lower() and b"," not in response.content[:300]:
        raise RuntimeError(f"OpenIntro tra ve HTML thay vi CSV: {content_type}")

    df = pd.read_csv(BytesIO(response.content), low_memory=False)
    validate_dataframe(df, "OpenIntro")
    RAW_DATA_PATH.write_bytes(response.content)
    return df


def load_from_openml() -> pd.DataFrame:
    last_error: Exception | None = None
    candidates = [
        {"name": "ames-housing", "version": 1},
        {"data_id": 41211},
        {"name": "house_prices"},
    ]

    for candidate in candidates:
        try:
            dataset = fetch_openml(as_frame=True, **candidate)
            if dataset.frame is not None:
                df = dataset.frame.copy()
            else:
                df = pd.DataFrame(dataset.data)
                if dataset.target is not None:
                    target_name = getattr(dataset, "target_names", None)
                    target_column = target_name[0] if target_name else "target"
                    df[target_column] = dataset.target

            if dataset.target is not None:
                target_name = getattr(dataset, "target_names", None)
                if target_name:
                    target_column = target_name[0]
                    if target_column not in df.columns:
                        df[target_column] = dataset.target
                elif "SalePrice" not in df.columns and "Sale_Price" not in df.columns:
                    df["SalePrice"] = dataset.target

            validate_dataframe(df, f"OpenML {candidate}")
            df = standardize_column_names(df)
            RAW_DATA_PATH.write_text(df.to_csv(index=False), encoding="utf-8")
            return df
        except Exception as exc:  # noqa: BLE001
            last_error = exc

    raise RuntimeError(f"Khong tai duoc Ames Housing tu OpenML. Loi cuoi: {last_error}")


def load_or_download_dataset() -> tuple[pd.DataFrame, str]:
    if RAW_DATA_PATH.exists():
        df = read_csv_safely(RAW_DATA_PATH)
        validate_dataframe(df, "data/raw/ames.csv")
        return df, OPENINTRO_SOURCE_LABEL

    errors: list[str] = []
    try:
        return load_from_openintro(), OPENINTRO_SOURCE_LABEL
    except Exception as exc:  # noqa: BLE001
        errors.append(f"OpenIntro: {exc}")

    try:
        return load_from_openml(), "OpenML Ames Housing fallback"
    except Exception as exc:  # noqa: BLE001
        errors.append(f"OpenML: {exc}")

    write_download_guide(errors)
    raise RuntimeError(
        "Khong the tai dataset tu dong. Da tao DOWNLOAD_DATA.md de huong dan tai thu cong."
    )


def make_one_hot_encoder() -> OneHotEncoder:
    try:
        return OneHotEncoder(handle_unknown="ignore", sparse_output=False)
    except TypeError:
        return OneHotEncoder(handle_unknown="ignore", sparse=False)


def prepare_modeling_data(df: pd.DataFrame) -> pd.DataFrame:
    df = standardize_column_names(df)
    missing = [column for column in [TARGET_COLUMN, *FEATURE_COLUMNS] if column not in df.columns]
    if missing:
        available = ", ".join(df.columns[:25])
        raise ValueError(
            "Dataset thieu cot bat buoc: "
            + ", ".join(missing)
            + f". Mot so cot hien co: {available}"
        )

    modeling_df = df[[TARGET_COLUMN, *FEATURE_COLUMNS]].copy()
    modeling_df[TARGET_COLUMN] = pd.to_numeric(modeling_df[TARGET_COLUMN], errors="coerce")
    modeling_df = modeling_df.dropna(subset=[TARGET_COLUMN])
    modeling_df = modeling_df[modeling_df[TARGET_COLUMN] > 0]

    for column in NUMERIC_FEATURES:
        modeling_df[column] = pd.to_numeric(modeling_df[column], errors="coerce")

    for column in CATEGORICAL_FEATURES:
        modeling_df[column] = modeling_df[column].astype("string")

    if modeling_df.empty:
        raise ValueError("Khong con dong hop le sau khi xu ly cot price.")

    modeling_df.to_csv(PROCESSED_DATA_PATH, index=False, encoding="utf-8-sig")
    return modeling_df


def get_numeric_features(feature_columns: list[str]) -> list[str]:
    return [feature for feature in feature_columns if feature in NUMERIC_FEATURES]


def get_categorical_features(feature_columns: list[str]) -> list[str]:
    return [feature for feature in feature_columns if feature in CATEGORICAL_FEATURES]


def build_pipeline(feature_columns: list[str], estimator: LinearRegression | Ridge) -> Pipeline:
    numeric_transformer = Pipeline(
        steps=[
            ("imputer", SimpleImputer(strategy="median")),
            ("scaler", StandardScaler()),
        ]
    )
    categorical_transformer = Pipeline(
        steps=[
            ("imputer", SimpleImputer(strategy="most_frequent")),
            ("onehot", make_one_hot_encoder()),
        ]
    )
    numeric_features = get_numeric_features(feature_columns)
    categorical_features = get_categorical_features(feature_columns)
    preprocessor = ColumnTransformer(
        transformers=[
            ("numeric", numeric_transformer, numeric_features),
            ("categorical", categorical_transformer, categorical_features),
        ],
        remainder="drop",
    )
    return Pipeline(
        steps=[
            ("preprocess", preprocessor),
            ("model", estimator),
        ]
    )


def get_feature_names(model: Pipeline) -> list[str]:
    preprocessor = model.named_steps["preprocess"]
    try:
        return [
            name.replace("numeric__", "").replace("categorical__", "")
            for name in preprocessor.get_feature_names_out()
        ]
    except Exception:  # noqa: BLE001
        return []


def save_model_coefficients(model: Pipeline) -> None:
    feature_names = get_feature_names(model)
    coefficients = model.named_steps["model"].coef_

    if not feature_names or len(feature_names) != len(coefficients):
        feature_names = [f"feature_{index}" for index in range(len(coefficients))]

    coef_df = pd.DataFrame(
        {
            "feature": feature_names,
            "coefficient": coefficients,
            "absolute_coefficient": np.abs(coefficients),
        }
    ).sort_values("absolute_coefficient", ascending=False)
    coef_df.to_csv(COEFFICIENTS_PATH, index=False, encoding="utf-8-sig")


def save_plots(y_true: pd.Series, y_pred: np.ndarray) -> None:
    min_value = min(float(y_true.min()), float(y_pred.min()))
    max_value = max(float(y_true.max()), float(y_pred.max()))

    plt.figure(figsize=(8, 6))
    plt.scatter(y_true, y_pred, alpha=0.65, edgecolors="none")
    plt.plot([min_value, max_value], [min_value, max_value], color="red", linewidth=2)
    plt.xlabel("Gia thuc te (USD)")
    plt.ylabel("Gia du doan (USD)")
    plt.title("Actual vs Predicted House Prices")
    plt.tight_layout()
    plt.savefig(ACTUAL_VS_PREDICTED_PATH, dpi=160)
    plt.close()

    residuals = y_true.to_numpy() - y_pred
    plt.figure(figsize=(8, 6))
    plt.scatter(y_pred, residuals, alpha=0.65, edgecolors="none")
    plt.axhline(0, color="red", linewidth=2)
    plt.xlabel("Gia du doan (USD)")
    plt.ylabel("Sai so du (actual - predicted)")
    plt.title("Residual Plot")
    plt.tight_layout()
    plt.savefig(RESIDUALS_PATH, dpi=160)
    plt.close()


def calculate_metrics(y_true: pd.Series, y_pred: np.ndarray) -> dict[str, float]:
    return {
        "mae": float(mean_absolute_error(y_true, y_pred)),
        "rmse": float(np.sqrt(mean_squared_error(y_true, y_pred))),
        "r2": float(r2_score(y_true, y_pred)),
    }


def make_model_configs() -> list[tuple[str, float | None, LinearRegression | Ridge]]:
    configs: list[tuple[str, float | None, LinearRegression | Ridge]] = [
        ("LinearRegression", None, LinearRegression())
    ]
    for alpha in RIDGE_ALPHA_VALUES:
        configs.append((f"Ridge(alpha={alpha})", alpha, Ridge(alpha=alpha)))
    return configs


def train_and_compare_models(
    modeling_df: pd.DataFrame,
) -> tuple[pd.DataFrame, dict[str, object], Pipeline]:
    comparison_rows: list[dict[str, object]] = []
    best_result: dict[str, object] | None = None
    baseline_full_model: Pipeline | None = None

    y = modeling_df[TARGET_COLUMN]

    for feature_set_name, feature_columns in FEATURE_SETS.items():
        X = modeling_df[feature_columns]
        X_train, X_test, y_train, y_test = train_test_split(
            X,
            y,
            test_size=0.2,
            random_state=42,
        )

        for model_name, alpha, estimator in make_model_configs():
            model = build_pipeline(feature_columns, estimator)
            model.fit(X_train, y_train)
            y_pred = model.predict(X_test)
            score = calculate_metrics(y_test, y_pred)

            row: dict[str, object] = {
                "feature_set": feature_set_name,
                "model_name": model_name,
                "alpha": alpha if alpha is not None else "",
                "n_features_before_encoding": len(feature_columns),
                "train_samples": int(X_train.shape[0]),
                "test_samples": int(X_test.shape[0]),
                **score,
            }
            comparison_rows.append(row)

            result: dict[str, object] = {
                **row,
                "alpha": alpha,
                "model": model,
                "feature_columns": feature_columns,
                "X_train": X_train,
                "X_test": X_test,
                "y_train": y_train,
                "y_test": y_test,
                "y_pred": y_pred,
            }

            if feature_set_name == "full_features" and model_name == "LinearRegression":
                baseline_full_model = model

            if best_result is None or score[TUNING_METRIC] < float(best_result[TUNING_METRIC]):
                best_result = result

    if best_result is None or baseline_full_model is None:
        raise RuntimeError("Khong the huan luyen va so sanh mo hinh.")

    comparison_df = pd.DataFrame(comparison_rows).sort_values(
        by=["rmse", "mae"],
        ascending=[True, True],
    )
    comparison_df.to_csv(MODEL_COMPARISON_PATH, index=False, encoding="utf-8-sig")

    return comparison_df, best_result, baseline_full_model


def save_metrics(metrics: dict[str, object]) -> None:
    METRICS_PATH.write_text(
        json.dumps(metrics, indent=2, ensure_ascii=False),
        encoding="utf-8",
    )


def build_example_home(modeling_df: pd.DataFrame, feature_columns: list[str]) -> pd.DataFrame:
    example = {
        "area": 1800,
        "Bedroom.AbvGr": 3,
        "Full.Bath": 2,
        "Half.Bath": 1,
        "TotRms.AbvGrd": 7,
        "Year.Built": 2005,
        "Garage.Area": 450,
        "Overall.Qual": 7,
        "Overall.Cond": 5,
    }

    for column in get_categorical_features(feature_columns):
        mode = modeling_df[column].mode(dropna=True)
        example[column] = str(mode.iloc[0]) if not mode.empty else "Unknown"

    return pd.DataFrame([example], columns=feature_columns)


def format_currency(value: float) -> str:
    return f"{value:,.0f} USD"


def print_outputs(metrics: dict[str, object]) -> None:
    print("\nCac file ket qua da luu:")
    for path in [
        RAW_DATA_PATH,
        PROCESSED_DATA_PATH,
        METRICS_PATH,
        MODEL_COMPARISON_PATH,
        SAMPLE_PREDICTIONS_PATH,
        COEFFICIENTS_PATH,
        MODEL_PATH,
        BEST_MODEL_PATH,
        ACTUAL_VS_PREDICTED_PATH,
        RESIDUALS_PATH,
    ]:
        print(f"- {path.relative_to(PROJECT_DIR)}")

    print("\nMetrics:")
    print(f"- MAE : {metrics['mae']:,.2f}")
    print(f"- RMSE: {metrics['rmse']:,.2f}")
    print(f"- R2  : {metrics['r2']:.4f}")
    print(f"- Best model: {metrics['best_model_name']}")
    print(f"- Best alpha: {metrics['best_alpha']}")
    print(f"- Best feature set: {metrics['best_feature_set']}")


def main() -> None:
    ensure_directories()

    df, data_source = load_or_download_dataset()
    df = standardize_column_names(df)
    modeling_df = prepare_modeling_data(df)

    comparison_df, best_result, baseline_full_model = train_and_compare_models(modeling_df)
    best_model = best_result["model"]
    feature_columns = best_result["feature_columns"]
    X_test = best_result["X_test"]
    y_test = best_result["y_test"]
    y_pred = best_result["y_pred"]

    baseline_row = comparison_df[
        (comparison_df["feature_set"] == "full_features")
        & (comparison_df["model_name"] == "LinearRegression")
    ].iloc[0]

    sample_predictions = X_test.copy().reset_index(drop=True).head(20)
    sample_predictions["actual_price"] = y_test.reset_index(drop=True).head(20)
    sample_predictions["predicted_price"] = y_pred[:20]
    sample_predictions["absolute_error"] = (
        sample_predictions["actual_price"] - sample_predictions["predicted_price"]
    ).abs()
    sample_predictions.to_csv(SAMPLE_PREDICTIONS_PATH, index=False, encoding="utf-8-sig")

    save_model_coefficients(best_model)
    save_plots(y_test, y_pred)
    joblib.dump(baseline_full_model, MODEL_PATH)
    joblib.dump(best_model, BEST_MODEL_PATH)

    example_home = build_example_home(modeling_df, feature_columns)
    example_prediction = float(best_model.predict(example_home)[0])

    metrics: dict[str, object] = {
        "data_source": data_source,
        "dataset_rows": int(df.shape[0]),
        "dataset_columns": int(df.shape[1]),
        "modeling_rows": int(modeling_df.shape[0]),
        "train_samples": int(best_result["train_samples"]),
        "test_samples": int(best_result["test_samples"]),
        "target": TARGET_COLUMN,
        "features": feature_columns,
        "mae": float(best_result["mae"]),
        "rmse": float(best_result["rmse"]),
        "r2": float(best_result["r2"]),
        "best_model_name": best_result["model_name"],
        "best_alpha": best_result["alpha"],
        "best_feature_set": best_result["feature_set"],
        "tuning_metric": TUNING_METRIC,
        "tuning_candidates": int(comparison_df.shape[0]),
        "baseline_model_name": "LinearRegression",
        "baseline_feature_set": "full_features",
        "baseline_mae": float(baseline_row["mae"]),
        "baseline_rmse": float(baseline_row["rmse"]),
        "baseline_r2": float(baseline_row["r2"]),
        "example_prediction_usd": example_prediction,
    }
    save_metrics(metrics)

    print("Ames Housing - Linear Regression with Tuning")
    print(f"Nguon du lieu: {data_source}")
    print(f"So dong/so cot dataset: {df.shape[0]} dong, {df.shape[1]} cot")
    print("Features cua mo hinh tot nhat:")
    for feature in feature_columns:
        print(f"- {feature}")
    print(f"So mau train/test: {best_result['train_samples']} train, {best_result['test_samples']} test")

    print("\nBang so sanh mo hinh:")
    print(
        comparison_df[
            ["feature_set", "model_name", "alpha", "mae", "rmse", "r2"]
        ].to_string(index=False)
    )

    print("\nMo hinh tot nhat theo RMSE:")
    print(f"- Model: {best_result['model_name']}")
    print(f"- Alpha: {best_result['alpha']}")
    print(f"- Feature set: {best_result['feature_set']}")
    print(f"MAE : {float(best_result['mae']):,.2f}")
    print(f"RMSE: {float(best_result['rmse']):,.2f}")
    print(f"R2  : {float(best_result['r2']):.4f}")

    print("\nVi du du doan gia cho mot can nha moi:")
    print(example_home.to_string(index=False))
    print(f"Gia du doan: {format_currency(example_prediction)}")

    print_outputs(metrics)


if __name__ == "__main__":
    try:
        main()
    except Exception as exc:  # noqa: BLE001
        print(f"Loi: {exc}", file=sys.stderr)
        sys.exit(1)
