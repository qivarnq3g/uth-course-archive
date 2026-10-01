"""Create and validate the two Wine Quality polynomial-regression workflows.

Run this file with the Python interpreter bundled with Orange 3.40.  The
workflows themselves use Orange's Datasets widget, so they stay tied to the
official Orange dataset catalogue instead of a copied third-party CSV.
"""

from __future__ import annotations

import copy
import os
from math import comb
from pathlib import Path
from typing import Final

import numpy as np

os.environ.setdefault("QT_QPA_PLATFORM", "offscreen")

try:
    import Orange
    from AnyQt.QtWidgets import QApplication
    from Orange.canvas.config import Config
    from Orange.data import ContinuousVariable, Table, Variable
    from Orange.misc.environ import data_dir
    from Orange.widgets.data.owdatasets import OWDataSets, ensure_local
    from Orange.widgets.data.owdatasampler import OWDataSampler
    from Orange.widgets.data.owselectcolumns import OWSelectAttributes
    from Orange.widgets.evaluate.owtestandscore import OWTestAndScore
    from orangecanvas.registry import WidgetRegistry
    from orangecanvas.scheme import Scheme, SchemeLink, SchemeNode
    from orangewidget.workflow.widgetsscheme import WidgetsScheme
except ModuleNotFoundError as exc:  # pragma: no cover - wrong interpreter help
    raise SystemExit(
        "Orange3 is required. Run this script with Orange's own python.exe."
    ) from exc


ORANGE_DIR: Final = Path(__file__).resolve().parent
PROJECT_ROOT: Final = ORANGE_DIR.parent
CATALOG_URL: Final = OWDataSets.INDEX_URL
CATALOG_PREFIX: Final = ("core",)
TARGET: Final = "quality"
DEGREES: Final = (2, 3)

DATASETS: Final = {
    "red": {
        "catalog_id": "winequality-red.tab",
        "title": "Wine Quality - Red Wine",
        "workflow": ORANGE_DIR / "wine_poly_red.ows",
    },
    "white": {
        "catalog_id": "winequality-white.tab",
        "title": "Wine Quality - White Wine",
        "workflow": ORANGE_DIR / "wine_poly_white.ows",
    },
}

QNAME: Final = {
    "datasets": "Orange.widgets.data.owdatasets.OWDataSets",
    "select": "Orange.widgets.data.owselectcolumns.OWSelectAttributes",
    "script": "Orange.widgets.data.owpythonscript.OWPythonScript",
    "sampler": "Orange.widgets.data.owdatasampler.OWDataSampler",
    "linear": "Orange.widgets.model.owlinearregression.OWLinearRegression",
    "predictions": "Orange.widgets.evaluate.owpredictions.OWPredictions",
    "table": "Orange.widgets.data.owtable.OWTable",
    "test_score": "Orange.widgets.evaluate.owtestandscore.OWTestAndScore",
}


def polynomial_script(degree: int) -> str:
    """Return embedded Orange Python Script code for a deterministic expansion."""

    return f'''# Multivariate polynomial expansion, degree {degree}.
import numpy as np
from Orange.data import ContinuousVariable, Domain, Table
from sklearn.preprocessing import PolynomialFeatures

out_data = None
if in_data is not None:
    attrs = tuple(in_data.domain.attributes)
    if not attrs or not all(isinstance(v, ContinuousVariable) for v in attrs):
        raise TypeError("All Features must be continuous before polynomial expansion.")
    if any(v.name == "quality" for v in attrs):
        raise ValueError("Target leakage: quality is present in Features.")

    transformer = PolynomialFeatures(degree={degree}, include_bias=False)
    expanded = transformer.fit_transform(np.asarray(in_data.X, dtype=float))
    if not np.isfinite(expanded).all():
        raise ValueError("Polynomial expansion produced NaN or infinity.")

    names = list(transformer.get_feature_names_out([v.name for v in attrs]))
    if len(names) != len(set(names)):
        raise ValueError("Polynomial expansion produced duplicate feature names.")
    variables = tuple(ContinuousVariable(name) for name in names)
    domain = Domain(variables, in_data.domain.class_vars, in_data.domain.metas)
    out_data = Table.from_numpy(
        domain,
        expanded,
        in_data.Y,
        in_data.metas,
        in_data.W,
        attributes=dict(in_data.attributes),
        ids=in_data.ids,
    )
'''


def residual_script(learner_name: str) -> str:
    """Return code that adds actual-minus-predicted residual to Predictions data."""

    return f'''# Add residual = actual quality - predicted quality.
import numpy as np
from Orange.data import ContinuousVariable, Domain, Table

out_data = None
if in_data is not None:
    if in_data.domain.class_var is None or in_data.domain.class_var.name != "quality":
        raise ValueError("Predictions data must retain numeric target quality.")
    predicted_vars = [
        var for var in in_data.domain.metas
        if isinstance(var, ContinuousVariable) and var.name == {learner_name!r}
    ]
    if len(predicted_vars) != 1:
        raise ValueError("Could not identify the prediction column uniquely.")
    predicted = np.asarray(in_data.get_column(predicted_vars[0]), dtype=float)
    actual = np.asarray(in_data.Y, dtype=float).reshape(-1)
    residual = actual - predicted
    if not np.isfinite(residual).all():
        raise ValueError("Residual contains NaN or infinity.")

    residual_var = ContinuousVariable("residual")
    meta_vars = (*in_data.domain.metas, residual_var)
    metas = np.column_stack((in_data.metas, residual))
    domain = Domain(in_data.domain.attributes, in_data.domain.class_vars, meta_vars)
    out_data = Table.from_numpy(
        domain,
        in_data.X,
        in_data.Y,
        metas,
        in_data.W,
        attributes=dict(in_data.attributes),
        ids=in_data.ids,
    )
'''


def build_registry() -> WidgetRegistry:
    config = Config()
    registry = WidgetRegistry()
    config.widget_discovery(registry).run(config.widgets_entry_points())
    for qualified_name in QNAME.values():
        registry.widget(qualified_name)
    return registry


def load_catalogue_table(catalog_id: str) -> Table:
    """Download through Orange's catalogue cache and return the exact table."""

    cache = Path(data_dir()) / OWDataSets.DATASET_DIR
    path = ensure_local(
        CATALOG_URL,
        (*CATALOG_PREFIX, catalog_id),
        str(cache),
    )
    return Table(path)


def dataset_properties(catalog_id: str) -> dict[str, object]:
    return {
        "selected_id": catalog_id,
        "language": OWDataSets.DEFAULT_LANG,
        "domain": None,
        "filter_hint": None,
        "__version__": OWDataSets.settings_version,
    }


def sampler_properties() -> dict[str, object]:
    return {
        "sampling_type": OWDataSampler.FixedProportion,
        "sampleSizePercentage": 70,
        "replacement": False,
        "use_seed": True,
        "stratify": False,
        "compatibility_mode": False,
        "__version__": OWDataSampler.settings_version,
    }


def linear_properties(degree: int) -> dict[str, object]:
    return {
        "ridge": False,
        "reg_type": 0,
        "alpha_index": 0,
        "l2_ratio": 0.5,
        "fit_intercept": True,
        "autosend": True,
        "learner_name": f"Polynomial Linear Regression d={degree}",
        "__version__": 1,
    }


def test_score_properties() -> dict[str, object]:
    return {
        "resampling": OWTestAndScore.TestOnTest,
        "cv_stratified": False,
        "shuffle_stratified": False,
        "__version__": OWTestAndScore.settings_version,
    }


def script_properties(title: str, code: str) -> dict[str, object]:
    return {
        "scriptLibrary": [{"name": title, "script": code, "filename": None}],
        "currentScriptIndex": 0,
        "scriptText": code,
        "splitterState": None,
        "vimModeEnabled": False,
        "__version__": 2,
    }


def select_properties(registry: WidgetRegistry, data: Table) -> dict[str, object]:
    """Pack a real Select Columns context with quality as the only target."""

    all_variables: tuple[Variable, ...] = (
        *data.domain.attributes,
        *data.domain.class_vars,
        *data.domain.metas,
    )
    by_name = {variable.name: variable for variable in all_variables}
    if len(by_name) != len(all_variables):
        raise ValueError("Dataset contains duplicate variable names.")
    if TARGET not in by_name:
        raise ValueError(f"Dataset does not contain target {TARGET!r}.")

    target = by_name[TARGET]
    if not isinstance(target, ContinuousVariable):
        raise TypeError("quality must be continuous for regression.")
    features = [variable for variable in all_variables if variable is not target]
    if not features or not all(isinstance(variable, ContinuousVariable) for variable in features):
        raise TypeError("Every Wine Quality feature must be continuous.")

    probe = WidgetsScheme(title="Wine Select Columns settings probe")
    node = SchemeNode(registry.widget(QNAME["select"]))
    probe.add_node(node)
    widget = probe.widget_for_node(node)
    if not isinstance(widget, OWSelectAttributes):
        raise TypeError("Registry returned the wrong Select Columns widget.")
    widget.set_data(data)
    widget.handleNewSignals()
    widget.used_attrs[:] = features
    widget.class_attrs[:] = [target]
    widget.meta_attrs[:] = []
    widget.available_attrs[:] = []
    widget.update_domain_role_hints()
    widget.commit.now()
    properties = copy.deepcopy(widget.settingsHandler.pack_data(widget))
    probe.clear()
    QApplication.processEvents()
    return properties


def add_node(
    scheme: Scheme,
    registry: WidgetRegistry,
    key: str,
    title: str,
    position: tuple[float, float],
    properties: dict[str, object] | None = None,
) -> SchemeNode:
    node = SchemeNode(
        registry.widget(QNAME[key]),
        title=title,
        position=position,
        properties=properties or {},
    )
    scheme.add_node(node)
    return node


def connect(
    scheme: Scheme,
    source: SchemeNode,
    source_channel: str,
    sink: SchemeNode,
    sink_channel: str,
) -> None:
    scheme.add_link(SchemeLink(source, source_channel, sink, sink_channel))


def build_workflow(
    registry: WidgetRegistry,
    destination: Path,
    catalog_id: str,
    dataset_title: str,
    data: Table,
) -> None:
    """Create one clear left-to-right workflow with independent d=2/d=3 branches."""

    scheme = Scheme(
        title=f"Polynomial Regression - {dataset_title}",
        description=(
            "Official Orange catalogue data; quality is numeric target. "
            "Multivariate polynomial features d=2 and d=3, deterministic 70/30 "
            "holdout (Orange RandomSeed=42), ordinary Linear Regression."
        ),
    )

    datasets = add_node(
        scheme, registry, "datasets", dataset_title, (60.0, 330.0),
        dataset_properties(catalog_id),
    )
    raw_table = add_node(scheme, registry, "table", "Raw Data", (250.0, 100.0))
    select = add_node(
        scheme, registry, "select", "Target = quality", (250.0, 330.0),
        select_properties(registry, data),
    )
    selected_table = add_node(
        scheme, registry, "table", "Selected Data", (450.0, 100.0)
    )

    connect(scheme, datasets, "data", raw_table, "data")
    connect(scheme, datasets, "data", select, "data")
    connect(scheme, select, "data", selected_table, "data")

    for row, degree in enumerate(DEGREES):
        y = 260.0 + row * 410.0
        learner_name = f"Polynomial Linear Regression d={degree}"
        poly = add_node(
            scheme,
            registry,
            "script",
            f"Polynomial Features d={degree}",
            (470.0, y),
            script_properties(
                f"Polynomial Features d={degree}", polynomial_script(degree)
            ),
        )
        sampler = add_node(
            scheme,
            registry,
            "sampler",
            f"70/30 seed 42 d={degree}",
            (680.0, y),
            sampler_properties(),
        )
        linear = add_node(
            scheme,
            registry,
            "linear",
            f"Linear Regression d={degree}",
            (880.0, y - 80.0),
            linear_properties(degree),
        )
        predictions = add_node(
            scheme,
            registry,
            "predictions",
            f"Test Predictions d={degree}",
            (1080.0, y - 130.0),
        )
        residual = add_node(
            scheme,
            registry,
            "script",
            f"Residual d={degree}",
            (1280.0, y - 130.0),
            script_properties(
                f"Residual d={degree}", residual_script(learner_name)
            ),
        )
        prediction_table = add_node(
            scheme,
            registry,
            "table",
            f"Prediction Table d={degree}",
            (1480.0, y - 130.0),
        )
        test_score = add_node(
            scheme,
            registry,
            "test_score",
            f"Test & Score d={degree}",
            (1080.0, y + 100.0),
            test_score_properties(),
        )

        connect(scheme, select, "data", poly, "data")
        connect(scheme, poly, "data", sampler, "data")
        connect(scheme, sampler, "data_sample", linear, "data")
        connect(scheme, sampler, "remaining_data", predictions, "data")
        connect(scheme, linear, "model", predictions, "predictors")
        connect(scheme, predictions, "annotated", residual, "data")
        connect(scheme, residual, "data", prediction_table, "data")
        connect(scheme, linear, "learner", test_score, "learner")
        connect(scheme, sampler, "data_sample", test_score, "train_data")
        connect(scheme, sampler, "remaining_data", test_score, "test_data")

    destination.parent.mkdir(parents=True, exist_ok=True)
    scheme.save_to(str(destination), pretty=True, pickle_fallback=True)
    verify_workflow(registry, destination, catalog_id, len(data))


def verify_workflow(
    registry: WidgetRegistry,
    path: Path,
    catalog_id: str,
    expected_rows: int,
) -> None:
    loaded = Scheme()
    loaded.load_from(str(path), registry=registry)
    if len(loaded.nodes) != 18 or len(loaded.links) != 23:
        raise AssertionError(
            f"{path.name}: expected 18 nodes/23 links, got "
            f"{len(loaded.nodes)} nodes/{len(loaded.links)} links."
        )

    dataset_nodes = [
        node for node in loaded.nodes
        if node.description.qualified_name == QNAME["datasets"]
    ]
    if len(dataset_nodes) != 1:
        raise AssertionError(f"{path.name}: expected exactly one Datasets widget.")
    if dataset_nodes[0].properties.get("selected_id") != catalog_id:
        raise AssertionError(f"{path.name}: wrong Orange catalogue dataset id.")

    samplers = [
        node for node in loaded.nodes
        if node.description.qualified_name == QNAME["sampler"]
    ]
    if len(samplers) != 2:
        raise AssertionError(f"{path.name}: expected two Data Samplers.")
    for sampler in samplers:
        props = sampler.properties
        if props.get("sampleSizePercentage") != 70 or not props.get("use_seed"):
            raise AssertionError(f"{path.name}: sampler is not reproducible 70/30.")
        if props.get("stratify"):
            raise AssertionError(f"{path.name}: regression sampler must not stratify.")

    scripts = [
        node for node in loaded.nodes
        if node.description.qualified_name == QNAME["script"]
    ]
    if len(scripts) != 4:
        raise AssertionError(f"{path.name}: expected two expansion and two residual scripts.")
    for degree in DEGREES:
        title = f"Polynomial Features d={degree}"
        node = next((node for node in scripts if node.title == title), None)
        if node is None or f"degree={degree}" not in node.properties.get("scriptText", ""):
            raise AssertionError(f"{path.name}: embedded degree {degree} code is missing.")

    if expected_rows <= 0:
        raise AssertionError(f"{path.name}: catalogue dataset is empty.")


def summarize_table(name: str, data: Table) -> str:
    features = data.domain.attributes
    target = data.domain.class_var
    missing = int(np.isnan(data.X).sum() + np.isnan(data.Y).sum())
    return (
        f"{name}: rows={len(data)}, features={len(features)}, "
        f"target={target.name if target else None}, missing={missing}, "
        f"d2={comb(len(features) + 2, 2) - 1}, "
        f"d3={comb(len(features) + 3, 3) - 1}"
    )


def main() -> None:
    app = QApplication.instance() or QApplication([])
    registry = build_registry()
    print(f"Orange {Orange.__version__}")
    print(f"Data Sampler fixed seed: {OWDataSampler.RandomSeed}")

    for name, spec in DATASETS.items():
        data = load_catalogue_table(str(spec["catalog_id"]))
        build_workflow(
            registry,
            Path(spec["workflow"]),
            str(spec["catalog_id"]),
            str(spec["title"]),
            data,
        )
        print(summarize_table(name, data))
        print(f"[OK] {Path(spec['workflow']).relative_to(PROJECT_ROOT)}")
    app.processEvents()


if __name__ == "__main__":
    main()
