"""Tính và vẽ bốn Decision Tree Regressor ở bài 7.1."""

from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

import matplotlib

matplotlib.use("Agg")

import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from sklearn.metrics import mean_absolute_error, mean_squared_error, r2_score
from sklearn.tree import DecisionTreeRegressor, plot_tree

PROJECT_ROOT = Path(__file__).resolve().parents[1]
DEFAULT_OUTPUT_DIR = PROJECT_ROOT / "outputs" / "regression-tree" / "exercise-7-1"
DEFAULT_ORANGE_DIR = PROJECT_ROOT / "orange" / "data" / "regression-exercise-7-1"
RANDOM_STATE = 42
MAX_DEPTH = 3


def exercise_cases() -> list[tuple[np.ndarray, np.ndarray]]:
    """Trả về đúng bốn bộ dữ liệu được in ở trang 18."""
    return [
        (np.asarray([[2], [1], [3], [5]], dtype=float), np.asarray([1.5, 2.5, 3, 2], dtype=float)),
        (
            np.asarray([[2.5], [1.5], [2], [3], [3.5]], dtype=float),
            np.asarray([1, 2.5, 3, 2.5, 1.5], dtype=float),
        ),
        (
            np.asarray([[2, 5], [1, 5], [2, 0], [0, 3], [3, 5]], dtype=float),
            np.asarray([1, 1.5, 3, 3.5, 1.5], dtype=float),
        ),
        (
            np.asarray([[1, 2, 3], [0, 1, 2], [2, 4, 1]], dtype=float),
            np.asarray([1, 1.5, 2], dtype=float),
        ),
    ]


def run_exercise(
    output_dir: Path = DEFAULT_OUTPUT_DIR,
    orange_dir: Path = DEFAULT_ORANGE_DIR,
) -> dict[str, Any]:
    """Fit từng case trên toàn bộ toy data, vẽ cây và lưu dự đoán train."""
    output_dir = Path(output_dir)
    orange_dir = Path(orange_dir)
    output_dir.mkdir(parents=True, exist_ok=True)
    orange_dir.mkdir(parents=True, exist_ok=True)

    summary: dict[str, Any] = {
        "source_evidence": "AI-Ex-2027.pdf, trang 18, mục 7.1",
        "max_depth": MAX_DEPTH,
        "random_state": RANDOM_STATE,
        "evaluation_note": (
            "Metric chỉ đo độ khớp trên toàn bộ toy data; không phải test/generalization metric."
        ),
        "cases": [],
    }
    prediction_rows: list[dict[str, Any]] = []

    for case_number, (features, target) in enumerate(exercise_cases(), start=1):
        if features.shape[0] != target.shape[0]:
            raise ValueError(f"Case {case_number}: số mẫu X và y không khớp.")

        model = DecisionTreeRegressor(max_depth=MAX_DEPTH, random_state=RANDOM_STATE)
        model.fit(features, target)
        predictions = model.predict(features)
        feature_names = [f"x{index + 1}" for index in range(features.shape[1])]

        case_metrics = {
            "case": case_number,
            "samples": int(features.shape[0]),
            "features": int(features.shape[1]),
            "tree_depth": int(model.get_depth()),
            "leaf_count": int(model.get_n_leaves()),
            "train_mae": float(mean_absolute_error(target, predictions)),
            "train_rmse": float(np.sqrt(mean_squared_error(target, predictions))),
            "train_r2": float(r2_score(target, predictions)),
        }
        summary["cases"].append(case_metrics)

        case_frame = pd.DataFrame(features, columns=feature_names)
        case_frame["target"] = target
        case_frame.to_csv(orange_dir / f"case_{case_number}.csv", index=False)

        for row_number, (actual, predicted) in enumerate(
            zip(target, predictions, strict=True),
            start=1,
        ):
            row: dict[str, Any] = {
                "case": case_number,
                "row": row_number,
                "actual": float(actual),
                "predicted": float(predicted),
            }
            for name, value in zip(feature_names, features[row_number - 1], strict=True):
                row[name] = float(value)
            prediction_rows.append(row)

        figure, axis = plt.subplots(figsize=(12, 7))
        plot_tree(
            model,
            feature_names=feature_names,
            filled=True,
            rounded=True,
            impurity=True,
            ax=axis,
        )
        axis.set_title(f"Bài 7.1 - Case {case_number} - max_depth=3")
        figure.tight_layout()
        figure.savefig(
            output_dir / f"case_{case_number}_tree.png",
            dpi=180,
            bbox_inches="tight",
        )
        plt.close(figure)

    pd.DataFrame(prediction_rows).to_csv(
        output_dir / "training_predictions.csv",
        index=False,
    )
    (output_dir / "summary.json").write_text(
        json.dumps(summary, ensure_ascii=False, indent=2),
        encoding="utf-8",
    )
    return summary


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--output-dir", type=Path, default=DEFAULT_OUTPUT_DIR)
    parser.add_argument("--orange-dir", type=Path, default=DEFAULT_ORANGE_DIR)
    return parser.parse_args()


def main() -> None:
    """Điểm vào CLI."""
    args = parse_args()
    summary = run_exercise(args.output_dir, args.orange_dir)
    print(json.dumps(summary, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()

