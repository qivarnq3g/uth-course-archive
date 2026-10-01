"""Huấn luyện và đánh giá DecisionTreeClassifier trên Iris."""

from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any, Mapping, Sequence

import matplotlib

matplotlib.use("Agg")

import matplotlib.pyplot as plt
import pandas as pd
from sklearn.datasets import load_iris
from sklearn.metrics import (
    ConfusionMatrixDisplay,
    accuracy_score,
    classification_report,
    confusion_matrix,
    f1_score,
    precision_score,
    recall_score,
)
from sklearn.model_selection import GridSearchCV, StratifiedKFold, train_test_split
from sklearn.tree import DecisionTreeClassifier, plot_tree

PROJECT_ROOT = Path(__file__).resolve().parents[1]
DEFAULT_OUTPUT_DIR = PROJECT_ROOT / "outputs" / "classification-tree"
DEFAULT_ORANGE_DATA = PROJECT_ROOT / "orange" / "data" / "iris.csv"
RANDOM_STATE = 42
TEST_SIZE = 0.30

DEFAULT_PARAM_GRID: Mapping[str, Sequence[Any]] = {
    "criterion": ("gini", "entropy"),
    "max_depth": (2, 3, 4, None),
    "min_samples_split": (2, 5, 10),
    "min_samples_leaf": (1, 2, 4),
}


def load_iris_data() -> tuple[pd.DataFrame, pd.Series, list[str]]:
    """Đọc Iris từ scikit-learn và kiểm tra target."""
    bunch = load_iris(as_frame=True)
    if bunch.data is None or bunch.target is None:
        raise ValueError("scikit-learn không trả về dữ liệu hoặc target Iris.")

    features = bunch.data.copy()
    target = bunch.target.astype("int64").rename("target")
    target_names = [str(name) for name in bunch.target_names]

    if features.empty or target.empty:
        raise ValueError("Dataset Iris rỗng.")
    if len(features) != len(target):
        raise ValueError("Số dòng feature và target Iris không khớp.")
    if target.nunique() != 3:
        raise ValueError("Iris phải có đúng ba lớp.")
    return features, target, target_names


def split_iris_data(
    features: pd.DataFrame,
    target: pd.Series,
) -> tuple[pd.DataFrame, pd.DataFrame, pd.Series, pd.Series]:
    """Chia Iris 70/30 và giữ tỷ lệ lớp bằng stratify."""
    return train_test_split(
        features,
        target,
        test_size=TEST_SIZE,
        random_state=RANDOM_STATE,
        stratify=target,
    )


def _json_ready_params(params: Mapping[str, Any]) -> dict[str, Any]:
    return {key: value for key, value in params.items()}


def train_and_evaluate(
    output_dir: Path = DEFAULT_OUTPUT_DIR,
    orange_data_path: Path = DEFAULT_ORANGE_DATA,
    *,
    param_grid: Mapping[str, Sequence[Any]] | None = None,
    cv_folds: int = 5,
) -> dict[str, Any]:
    """Tune trên train bằng CV, đánh giá một lần trên test và lưu artifact."""
    if cv_folds < 2:
        raise ValueError("cv_folds phải >= 2.")

    output_dir = Path(output_dir)
    orange_data_path = Path(orange_data_path)
    output_dir.mkdir(parents=True, exist_ok=True)
    orange_data_path.parent.mkdir(parents=True, exist_ok=True)

    features, target, target_names = load_iris_data()
    x_train, x_test, y_train, y_test = split_iris_data(features, target)

    estimator = DecisionTreeClassifier(random_state=RANDOM_STATE)
    cv = StratifiedKFold(
        n_splits=cv_folds,
        shuffle=True,
        random_state=RANDOM_STATE,
    )
    search = GridSearchCV(
        estimator=estimator,
        param_grid=dict(param_grid or DEFAULT_PARAM_GRID),
        scoring="f1_macro",
        cv=cv,
        n_jobs=-1,
        refit=True,
        return_train_score=True,
        error_score="raise",
    )
    search.fit(x_train, y_train)

    model = search.best_estimator_
    predictions = model.predict(x_test)
    labels = list(range(len(target_names)))
    matrix = confusion_matrix(y_test, predictions, labels=labels)

    metrics: dict[str, Any] = {
        "dataset": "Iris (scikit-learn load_iris)",
        "source_evidence": "AI-Ex-2027.pdf, trang 19, mục 7.3",
        "random_state": RANDOM_STATE,
        "test_size": TEST_SIZE,
        "train_samples": int(len(x_train)),
        "test_samples": int(len(x_test)),
        "feature_count": int(features.shape[1]),
        "class_names": target_names,
        "class_counts": {
            target_names[int(key)]: int(value)
            for key, value in target.value_counts().sort_index().items()
        },
        "missing_values": int(features.isna().sum().sum()),
        "duplicate_rows_with_target": int(
            pd.concat([features, target], axis=1).duplicated().sum()
        ),
        "cv_folds": cv_folds,
        "cv_scoring": "f1_macro",
        "best_cv_f1_macro": float(search.best_score_),
        "best_params": _json_ready_params(search.best_params_),
        "test_metrics": {
            "accuracy": float(accuracy_score(y_test, predictions)),
            "precision_macro": float(
                precision_score(y_test, predictions, average="macro", zero_division=0)
            ),
            "recall_macro": float(
                recall_score(y_test, predictions, average="macro", zero_division=0)
            ),
            "f1_macro": float(
                f1_score(y_test, predictions, average="macro", zero_division=0)
            ),
        },
        "confusion_matrix": matrix.tolist(),
    }

    report = classification_report(
        y_test,
        predictions,
        labels=labels,
        target_names=target_names,
        output_dict=True,
        zero_division=0,
    )
    pd.DataFrame(report).transpose().to_csv(
        output_dir / "classification_report.csv",
        index_label="class",
    )
    pd.DataFrame(matrix, index=target_names, columns=target_names).to_csv(
        output_dir / "confusion_matrix.csv",
        index_label="actual/predicted",
    )

    prediction_table = x_test.copy()
    prediction_table["actual"] = [target_names[int(value)] for value in y_test]
    prediction_table["predicted"] = [
        target_names[int(value)] for value in predictions
    ]
    prediction_table["correct"] = prediction_table["actual"] == prediction_table["predicted"]
    prediction_table.to_csv(output_dir / "predictions.csv", index_label="source_row")

    cv_frame = pd.DataFrame(search.cv_results_)
    cv_columns = [
        column
        for column in (
            "rank_test_score",
            "mean_test_score",
            "std_test_score",
            "mean_train_score",
            "param_criterion",
            "param_max_depth",
            "param_min_samples_split",
            "param_min_samples_leaf",
        )
        if column in cv_frame.columns
    ]
    cv_frame.loc[:, cv_columns].sort_values("rank_test_score").to_csv(
        output_dir / "cv_results.csv",
        index=False,
    )

    importance = pd.DataFrame(
        {
            "feature": features.columns,
            "importance": model.feature_importances_,
        }
    ).sort_values("importance", ascending=False)
    importance.to_csv(output_dir / "feature_importance.csv", index=False)

    figure, axis = plt.subplots(figsize=(18, 10))
    plot_tree(
        model,
        feature_names=list(features.columns),
        class_names=target_names,
        filled=True,
        rounded=True,
        impurity=True,
        proportion=False,
        ax=axis,
    )
    axis.set_title("Decision Tree - Iris (mô hình tốt nhất theo CV trên train)")
    figure.tight_layout()
    figure.savefig(output_dir / "tree.png", dpi=180, bbox_inches="tight")
    plt.close(figure)

    figure, axis = plt.subplots(figsize=(7, 6))
    ConfusionMatrixDisplay(
        confusion_matrix=matrix,
        display_labels=target_names,
    ).plot(ax=axis, cmap="Blues", colorbar=False)
    axis.set_title("Confusion Matrix - tập test")
    figure.tight_layout()
    figure.savefig(output_dir / "confusion_matrix.png", dpi=180, bbox_inches="tight")
    plt.close(figure)

    orange_frame = features.copy()
    orange_frame["species"] = [target_names[int(value)] for value in target]
    orange_frame.to_csv(orange_data_path, index=False)

    (output_dir / "metrics.json").write_text(
        json.dumps(metrics, ensure_ascii=False, indent=2),
        encoding="utf-8",
    )
    return metrics


def parse_args() -> argparse.Namespace:
    """Đọc tham số dòng lệnh."""
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument(
        "--output-dir",
        type=Path,
        default=DEFAULT_OUTPUT_DIR,
        help="Thư mục lưu metric, bảng và hình.",
    )
    parser.add_argument(
        "--orange-data",
        type=Path,
        default=DEFAULT_ORANGE_DATA,
        help="CSV dùng cho workflow Orange.",
    )
    parser.add_argument("--cv-folds", type=int, default=5)
    return parser.parse_args()


def main() -> None:
    """Điểm vào CLI."""
    args = parse_args()
    metrics = train_and_evaluate(
        output_dir=args.output_dir,
        orange_data_path=args.orange_data,
        cv_folds=args.cv_folds,
    )
    print(json.dumps(metrics, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()

