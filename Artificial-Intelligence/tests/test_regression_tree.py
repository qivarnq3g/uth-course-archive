"""Tests tối thiểu cho Regression Tree."""

from __future__ import annotations

import json
import math
from pathlib import Path

from regression_tree.exercise_7_1 import exercise_cases, run_exercise
from regression_tree.main import (
    DEFAULT_DATA_PATH,
    TARGET_COLUMN,
    load_ames_data,
    split_ames_data,
    train_and_evaluate,
)


def test_exercise_7_1_has_four_numeric_target_cases() -> None:
    cases = exercise_cases()
    assert len(cases) == 4
    for features, target in cases:
        assert len(features) == len(target)
        assert target.dtype.kind == "f"


def test_load_ames_has_numeric_target_and_expected_rows() -> None:
    features, target, raw = load_ames_data(DEFAULT_DATA_PATH)
    assert TARGET_COLUMN == target.name
    assert len(features) == len(target) == 2930
    assert raw.shape == (2930, 82)
    assert target.dtype.kind == "f"


def test_ames_split_is_70_30() -> None:
    features, target, _ = load_ames_data(DEFAULT_DATA_PATH)
    x_train, x_test, y_train, y_test = split_ames_data(features, target)
    assert len(x_train) == len(y_train) == 2051
    assert len(x_test) == len(y_test) == 879


def test_regression_metrics_are_finite_and_figures_exist(tmp_path: Path) -> None:
    output_dir = tmp_path / "regression"
    metrics = train_and_evaluate(
        data_path=DEFAULT_DATA_PATH,
        output_dir=output_dir,
        orange_data_path=tmp_path / "ames.csv",
        param_grid={
            "model__max_depth": [6],
            "model__min_samples_split": [10],
            "model__min_samples_leaf": [5],
        },
        cv_folds=3,
    )
    for value in metrics["test_metrics"].values():
        assert math.isfinite(value)
    assert (output_dir / "tree_top_levels.png").is_file()
    assert (output_dir / "actual_vs_predicted.png").is_file()
    loaded = json.loads((output_dir / "metrics.json").read_text(encoding="utf-8"))
    assert loaded["train_samples"] == 2051
    assert loaded["test_samples"] == 879


def test_exercise_7_1_creates_all_tree_figures(tmp_path: Path) -> None:
    output_dir = tmp_path / "exercise"
    summary = run_exercise(output_dir, tmp_path / "orange-toy")
    assert len(summary["cases"]) == 4
    for case_number in range(1, 5):
        assert (output_dir / f"case_{case_number}_tree.png").is_file()

