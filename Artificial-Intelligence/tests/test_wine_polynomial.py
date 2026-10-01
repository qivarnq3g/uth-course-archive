from __future__ import annotations

import json
import math
import xml.etree.ElementTree as ET
from pathlib import Path

import numpy as np
import pytest

from scripts.verify_wine_polynomial import (
    DATASETS,
    DEGREES,
    OUTPUT_DIR,
    PROJECT_ROOT,
    TARGET,
    evaluate_dataset,
    load_orange_tab,
    orange_compatible_split,
    validate_frame,
)


@pytest.fixture(scope="module")
def wine_frames():
    frames = {}
    for name, spec in DATASETS.items():
        path = PROJECT_ROOT / "orange" / "data" / str(spec["catalog_id"])
        assert path.is_file(), f"Run scripts/verify_wine_polynomial.py first: {path}"
        frame = load_orange_tab(path)
        validate_frame(name, frame, int(spec["expected_rows"]))
        frames[name] = frame
    return frames


def test_catalogue_datasets_have_numeric_quality_target_without_leakage(wine_frames):
    for name, frame in wine_frames.items():
        features = [column for column in frame.columns if column != TARGET]
        assert len(frame) == int(DATASETS[name]["expected_rows"])
        assert len(frame.columns) == 12
        assert len(features) == 11
        assert TARGET not in features
        assert frame[TARGET].dtype.kind in "fiu"
        assert int(frame.isna().sum().sum()) == 0


def test_orange_compatible_split_is_reproducible_and_disjoint(wine_frames):
    for frame in wine_frames.values():
        train_a, test_a = orange_compatible_split(len(frame))
        train_b, test_b = orange_compatible_split(len(frame))
        assert np.array_equal(train_a, train_b)
        assert np.array_equal(test_a, test_b)
        assert len(train_a) == math.ceil(0.70 * len(frame))
        assert len(test_a) == len(frame) - len(train_a)
        assert not set(train_a).intersection(test_a)
        assert set(train_a).union(test_a) == set(range(len(frame)))


def test_degree_2_and_3_feature_counts_and_metrics_are_finite(wine_frames):
    expected = {2: 77, 3: 363}
    for name, frame in wine_frames.items():
        metrics, summary = evaluate_dataset(name, frame)
        assert summary.features == 11
        assert {row.degree for row in metrics} == set(DEGREES)
        for row in metrics:
            assert row.features_after == expected[row.degree]
            assert np.isfinite([row.mae, row.rmse, row.r2]).all()


@pytest.mark.parametrize(
    ("filename", "catalog_id"),
    [
        ("wine_poly_red.ows", "winequality-red.tab"),
        ("wine_poly_white.ows", "winequality-white.tab"),
    ],
)
def test_workflow_xml_nodes_and_links_are_valid(filename: str, catalog_id: str):
    path = PROJECT_ROOT / "orange" / filename
    root = ET.parse(path).getroot()
    nodes = root.findall("./nodes/node")
    links = root.findall("./links/link")
    ids = {node.attrib["id"] for node in nodes}
    by_id = {node.attrib["id"]: node for node in nodes}

    assert len(nodes) == 18
    assert len(ids) == 18
    assert len(links) == 23
    assert catalog_id in path.read_text(encoding="utf-8")
    for link in links:
        assert link.attrib["source_node_id"] in ids
        assert link.attrib["sink_node_id"] in ids
        assert link.attrib["source_channel_id"]
        assert link.attrib["sink_channel_id"]

    predictions = [node for node in nodes if node.attrib["name"] == "Predictions"]
    test_scores = [node for node in nodes if node.attrib["name"] == "Test and Score"]
    assert len(predictions) == 2
    assert len(test_scores) == 2
    for prediction in predictions:
        incoming = {
            link.attrib["sink_channel_id"]
            for link in links
            if link.attrib["sink_node_id"] == prediction.attrib["id"]
        }
        assert incoming == {"data", "predictors"}
    for test_score in test_scores:
        incoming = {
            link.attrib["sink_channel_id"]
            for link in links
            if link.attrib["sink_node_id"] == test_score.attrib["id"]
        }
        assert incoming == {"learner", "train_data", "test_data"}
        learner_sources = [
            by_id[link.attrib["source_node_id"]].attrib["name"]
            for link in links
            if link.attrib["sink_node_id"] == test_score.attrib["id"]
            and link.attrib["sink_channel_id"] == "learner"
        ]
        assert learner_sources == ["Linear Regression"]


def test_machine_readable_outputs_cover_both_datasets_and_degrees():
    metrics_csv = OUTPUT_DIR / "metrics.csv"
    metrics_json = OUTPUT_DIR / "metrics.json"
    summary_csv = OUTPUT_DIR / "dataset-summary.csv"
    for path in (metrics_csv, metrics_json, summary_csv):
        assert path.is_file() and path.stat().st_size > 0

    payload = json.loads(metrics_json.read_text(encoding="utf-8"))
    records = payload["metrics"]
    assert {(row["dataset"], row["degree"]) for row in records} == {
        ("red", 2),
        ("red", 3),
        ("white", 2),
        ("white", 3),
    }
    assert all(row["random_state"] == 42 for row in records)
    assert all(np.isfinite([row["mae"], row["rmse"], row["r2"]]).all() for row in records)
