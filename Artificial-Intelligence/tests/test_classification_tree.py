"""Tests tối thiểu cho Classification Tree Iris."""

from __future__ import annotations

import json
import math
from pathlib import Path

from classification_tree_iris.main import (
    load_iris_data,
    split_iris_data,
    train_and_evaluate,
)


def test_load_iris_has_target_and_expected_shape() -> None:
    features, target, target_names = load_iris_data()
    assert features.shape == (150, 4)
    assert len(target) == 150
    assert target.nunique() == 3
    assert target_names == ["setosa", "versicolor", "virginica"]


def test_iris_split_is_70_30_and_stratified() -> None:
    features, target, _ = load_iris_data()
    x_train, x_test, y_train, y_test = split_iris_data(features, target)
    assert len(x_train) == len(y_train) == 105
    assert len(x_test) == len(y_test) == 45
    assert y_train.value_counts().sort_index().tolist() == [35, 35, 35]
    assert y_test.value_counts().sort_index().tolist() == [15, 15, 15]


def test_classification_metrics_are_finite_and_figures_exist(tmp_path: Path) -> None:
    output_dir = tmp_path / "classification"
    metrics = train_and_evaluate(
        output_dir=output_dir,
        orange_data_path=tmp_path / "iris.csv",
        param_grid={
            "criterion": ["gini"],
            "max_depth": [3],
            "min_samples_split": [2],
            "min_samples_leaf": [1],
        },
        cv_folds=3,
    )
    for value in metrics["test_metrics"].values():
        assert math.isfinite(value)
    assert (output_dir / "tree.png").is_file()
    assert (output_dir / "confusion_matrix.png").is_file()
    loaded = json.loads((output_dir / "metrics.json").read_text(encoding="utf-8"))
    assert loaded["train_samples"] == 105
    assert loaded["test_samples"] == 45

