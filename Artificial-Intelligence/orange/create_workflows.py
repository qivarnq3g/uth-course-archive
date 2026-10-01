"""Tạo và kiểm tra hai workflow Decision Tree cho Orange 3.40.

Chạy file này bằng Python nội bộ của bản Orange standalone. Script tạo
dataset CSV cục bộ, sinh hai file .ows, sau đó nạp lại bằng widget
registry để xác minh node, channel, target và các setting quan trọng.
"""

from __future__ import annotations

import copy
import csv
import os
from pathlib import Path
from typing import Final

# Phải đặt trước khi import Qt để script chạy được không cần màn hình.
os.environ.setdefault("QT_QPA_PLATFORM", "offscreen")

try:
    import Orange
    from AnyQt.QtWidgets import QApplication
    from Orange.canvas.config import Config
    from Orange.data import ContinuousVariable, DiscreteVariable, Table, Variable
    from Orange.widgets.data.owdatasampler import OWDataSampler
    from Orange.widgets.data.owfile import OWFile
    from Orange.widgets.data.owselectcolumns import OWSelectAttributes
    from Orange.widgets.evaluate.owtestandscore import OWTestAndScore
    from Orange.widgets.utils.filedialogs import RecentPath
    from orangecanvas.registry import WidgetRegistry
    from orangecanvas.scheme import Scheme, SchemeLink, SchemeNode
    from orangewidget.workflow.widgetsscheme import WidgetsScheme
except ModuleNotFoundError as exc:  # pragma: no cover - thông báo cho sai interpreter
    raise SystemExit(
        "Không tìm thấy Orange3. Hãy chạy script bằng Python nội bộ của "
        "Orange standalone; xem docs/orange-workflows.md."
    ) from exc


ORANGE_DIR: Final = Path(__file__).resolve().parent
PROJECT_ROOT: Final = ORANGE_DIR.parent
DATA_DIR: Final = ORANGE_DIR / "data"

IRIS_CSV: Final = DATA_DIR / "iris.csv"
AMES_CSV: Final = DATA_DIR / "ames_regression.csv"
AMES_SOURCE: Final = (
    PROJECT_ROOT / "bai_5_2_house_price_prediction" / "data" / "raw" / "ames.csv"
)

CLASSIFICATION_OWS: Final = ORANGE_DIR / "classification-tree.ows"
REGRESSION_OWS: Final = ORANGE_DIR / "regression-tree.ows"

IRIS_FEATURES: Final = (
    "sepal length",
    "sepal width",
    "petal length",
    "petal width",
)
IRIS_TARGET: Final = "species"

# Chỉ dùng các cột số để workflow Orange tập trung vào Regression Tree.
AMES_FEATURES: Final = (
    "area",
    "Bedroom.AbvGr",
    "Full.Bath",
    "Half.Bath",
    "TotRms.AbvGrd",
    "Year.Built",
    "Garage.Area",
    "Overall.Qual",
    "Overall.Cond",
)
AMES_TARGET: Final = "price"

QNAME: Final = {
    "file": "Orange.widgets.data.owfile.OWFile",
    "select": "Orange.widgets.data.owselectcolumns.OWSelectAttributes",
    "sampler": "Orange.widgets.data.owdatasampler.OWDataSampler",
    "tree": "Orange.widgets.model.owtree.OWTreeLearner",
    "predictions": "Orange.widgets.evaluate.owpredictions.OWPredictions",
    "table": "Orange.widgets.data.owtable.OWTable",
    "test_score": "Orange.widgets.evaluate.owtestandscore.OWTestAndScore",
    "confusion": "Orange.widgets.evaluate.owconfusionmatrix.OWConfusionMatrix",
    "viewer": "Orange.widgets.visualize.owtreeviewer.OWTreeGraph",
}
ALLOWED_QNAMES: Final = frozenset(QNAME.values())

NODE_LAYOUT: Final = {
    "file": ("File", (80.0, 180.0)),
    "select": ("Select Columns", (250.0, 180.0)),
    "sampler": ("Data Sampler 70/30", (440.0, 180.0)),
    "tree": ("Tree", (630.0, 180.0)),
    "predictions": ("Predictions (test)", (820.0, 110.0)),
    "table": ("Data Table", (1010.0, 110.0)),
    "viewer": ("Tree Viewer", (820.0, 270.0)),
    "test_score": ("Test & Score (held-out)", (640.0, 430.0)),
    "confusion": ("Confusion Matrix", (860.0, 430.0)),
}

BASE_LINKS: Final = (
    ("file", "data", "select", "data"),
    ("select", "data", "sampler", "data"),
    ("sampler", "data_sample", "tree", "data"),
    ("sampler", "remaining_data", "predictions", "data"),
    ("tree", "model", "predictions", "predictors"),
    ("predictions", "annotated", "table", "data"),
    ("tree", "learner", "test_score", "learner"),
    ("sampler", "data_sample", "test_score", "train_data"),
    ("sampler", "remaining_data", "test_score", "test_data"),
    ("tree", "model", "viewer", "tree"),
)
CLASSIFICATION_ONLY_LINKS: Final = (
    ("test_score", "evaluations_results", "confusion", "evaluation_results"),
)


def _format_number(value: float) -> str:
    """Ghi số gọn, ổn định và không phụ thuộc locale."""

    return format(float(value), ".12g")


def write_iris_csv(path: Path) -> int:
    """Xuất bản Iris tích hợp trong Orange thành CSV portable."""

    iris = Table("iris")
    path.parent.mkdir(parents=True, exist_ok=True)
    with path.open("w", encoding="utf-8", newline="") as stream:
        writer = csv.writer(stream)
        writer.writerow([*IRIS_FEATURES, IRIS_TARGET])
        for instance in iris:
            values = [_format_number(instance[feature]) for feature in iris.domain.attributes]
            label = str(instance.get_class())
            if label.startswith("Iris-"):
                label = label.removeprefix("Iris-")
            writer.writerow([*values, label])
    return len(iris)


def write_ames_csv(source: Path, destination: Path) -> int:
    """Tạo CSV hồi quy gọn từ bản Ames gốc; không sửa file nguồn."""

    if not source.is_file():
        if destination.is_file():
            with destination.open("r", encoding="utf-8", newline="") as stream:
                return sum(1 for _ in csv.DictReader(stream))
        raise FileNotFoundError(f"Không tìm thấy Ames source: {source}")

    required = (*AMES_FEATURES, AMES_TARGET)
    rows: list[list[str]] = []
    with source.open("r", encoding="utf-8-sig", newline="") as stream:
        reader = csv.DictReader(stream)
        missing = [name for name in required if name not in (reader.fieldnames or ())]
        if missing:
            raise ValueError("Ames source thiếu cột: " + ", ".join(missing))

        for row in reader:
            target_text = (row.get(AMES_TARGET) or "").strip()
            try:
                target = float(target_text)
            except ValueError:
                continue
            if target <= 0:
                continue
            rows.append([(row.get(name) or "").strip() for name in required])

    if len(rows) < 100:
        raise ValueError(f"Ames regression CSV chỉ có {len(rows)} dòng hợp lệ.")

    destination.parent.mkdir(parents=True, exist_ok=True)
    with destination.open("w", encoding="utf-8", newline="") as stream:
        writer = csv.writer(stream)
        writer.writerow(required)
        writer.writerows(rows)
    return len(rows)


def build_registry() -> WidgetRegistry:
    """Khám phá registry thật của bản Orange đang chạy."""

    config = Config()
    registry = WidgetRegistry()
    discovery = config.widget_discovery(registry)
    discovery.run(config.widgets_entry_points())
    for qname in ALLOWED_QNAMES:
        registry.widget(qname)
    return registry


def _domain_variables(data: Table) -> dict[str, Variable]:
    variables = (*data.domain.attributes, *data.domain.class_vars, *data.domain.metas)
    result = {variable.name: variable for variable in variables}
    if len(result) != len(variables):
        raise ValueError("Dataset có tên cột trùng nhau; không thể cấu hình target an toàn.")
    return result


def pack_select_columns_settings(
    registry: WidgetRegistry,
    data_path: Path,
    feature_names: tuple[str, ...],
    target_name: str,
    *,
    classification: bool,
) -> dict[str, object]:
    """Tạo context Select Columns và xác minh target khi nạp lại."""

    data = Table(str(data_path))
    variables = _domain_variables(data)
    missing = [name for name in (*feature_names, target_name) if name not in variables]
    if missing:
        raise ValueError(f"{data_path.name} thiếu cột: {', '.join(missing)}")

    target = variables[target_name]
    expected_type = DiscreteVariable if classification else ContinuousVariable
    if not isinstance(target, expected_type):
        raise TypeError(
            f"Target {target_name!r} phải là {expected_type.__name__}, "
            f"nhưng Orange đọc thành {type(target).__name__}."
        )

    probe = WidgetsScheme(title="Select Columns settings probe")
    probe.set_runtime_env("basedir", str(ORANGE_DIR))
    node = SchemeNode(registry.widget(QNAME["select"]))
    probe.add_node(node)
    widget = probe.widget_for_node(node)
    if not isinstance(widget, OWSelectAttributes):
        raise TypeError("Registry trả về sai widget cho Select Columns.")

    widget.set_data(data)
    widget.handleNewSignals()
    selected_features = [variables[name] for name in feature_names]
    selected = {*selected_features, target}
    all_variables = [
        *data.domain.attributes,
        *data.domain.class_vars,
        *data.domain.metas,
    ]
    widget.used_attrs[:] = selected_features
    widget.class_attrs[:] = [target]
    widget.meta_attrs[:] = []
    widget.available_attrs[:] = [variable for variable in all_variables if variable not in selected]
    widget.update_domain_role_hints()
    widget.commit.now()
    properties = copy.deepcopy(widget.settingsHandler.pack_data(widget))
    probe.clear()
    QApplication.processEvents()

    assert_select_columns_settings(
        registry,
        properties,
        data_path,
        feature_names,
        target_name,
        classification=classification,
    )
    return properties


def assert_select_columns_settings(
    registry: WidgetRegistry,
    properties: dict[str, object],
    data_path: Path,
    feature_names: tuple[str, ...],
    target_name: str,
    *,
    classification: bool,
) -> None:
    """Khởi tạo widget mới từ settings đã lưu và kiểm tra role."""

    data = Table(str(data_path))
    probe = WidgetsScheme(title="Select Columns restore probe")
    probe.set_runtime_env("basedir", str(ORANGE_DIR))
    node = SchemeNode(registry.widget(QNAME["select"]), properties=properties)
    probe.add_node(node)
    widget = probe.widget_for_node(node)
    if not isinstance(widget, OWSelectAttributes):
        raise TypeError("Không thể khởi tạo Select Columns để xác minh.")
    widget.set_data(data)
    widget.handleNewSignals()

    restored_features = tuple(variable.name for variable in widget.used_attrs)
    restored_targets = tuple(variable.name for variable in widget.class_attrs)
    if restored_features != feature_names:
        raise AssertionError(
            f"Select Columns khôi phục feature sai: {restored_features!r}"
        )
    if restored_targets != (target_name,):
        raise AssertionError(
            f"Select Columns khôi phục target sai: {restored_targets!r}"
        )

    target = widget.class_attrs[0]
    if classification and not isinstance(target, DiscreteVariable):
        raise AssertionError("Classification target không phải discrete.")
    if not classification and not isinstance(target, ContinuousVariable):
        raise AssertionError("Regression target không phải continuous.")

    probe.clear()
    QApplication.processEvents()


def file_properties(data_path: Path) -> dict[str, object]:
    """Cấu hình File bằng basedir để workflow không phụ thuộc đường dẫn máy."""

    relative = data_path.resolve().relative_to(ORANGE_DIR.resolve()).as_posix()
    recent = RecentPath(
        str(data_path.resolve()),
        "basedir",
        relative,
        file_format="Orange.data.io.CSVReader",
    )
    return {
        "recent_paths": [recent],
        "recent_urls": [],
        "source": OWFile.LOCAL_FILE,
        "url": "",
        "sheet_names": {},
        "__version__": OWFile.settings_version,
    }


def sampler_properties(*, classification: bool) -> dict[str, object]:
    return {
        "sampling_type": OWDataSampler.FixedProportion,
        "sampleSizePercentage": 70,
        "replacement": False,
        "use_seed": True,
        "stratify": classification,
        "compatibility_mode": False,
        "__version__": OWDataSampler.settings_version,
    }


def tree_properties(*, classification: bool) -> dict[str, object]:
    return {
        "auto_apply": True,
        "learner_name": "Classification Tree" if classification else "Regression Tree",
        "binary_trees": True,
        "limit_depth": True,
        "max_depth": 4,
        "limit_min_internal": True,
        "min_internal": 4,
        "limit_min_leaf": True,
        "min_leaf": 2,
        "limit_majority": classification,
        "sufficient_majority": 95,
        "__version__": 1,
    }


def test_score_properties(*, classification: bool) -> dict[str, object]:
    return {
        "resampling": OWTestAndScore.TestOnTest,
        "cv_stratified": classification,
        "shuffle_stratified": classification,
        "__version__": OWTestAndScore.settings_version,
    }


def build_workflow(
    registry: WidgetRegistry,
    destination: Path,
    data_path: Path,
    feature_names: tuple[str, ...],
    target_name: str,
    *,
    classification: bool,
) -> None:
    """Sinh một workflow với topology và setting đã allowlist."""

    select_properties = pack_select_columns_settings(
        registry,
        data_path,
        feature_names,
        target_name,
        classification=classification,
    )
    title = "Classification Tree - Iris" if classification else "Regression Tree - Ames"
    description = (
        "70/30 held-out workflow. Data Sample trains the Tree; Remaining Data is "
        "used by Predictions and Test & Score."
    )
    scheme = Scheme(title=title, description=description)
    nodes: dict[str, SchemeNode] = {}

    keys = [
        "file",
        "select",
        "sampler",
        "tree",
        "predictions",
        "table",
        "viewer",
        "test_score",
    ]
    if classification:
        keys.append("confusion")

    for key in keys:
        node_title, position = NODE_LAYOUT[key]
        properties: dict[str, object] = {}
        if key == "file":
            properties = file_properties(data_path)
        elif key == "select":
            properties = select_properties
        elif key == "sampler":
            properties = sampler_properties(classification=classification)
        elif key == "tree":
            properties = tree_properties(classification=classification)
        elif key == "test_score":
            properties = test_score_properties(classification=classification)

        node = SchemeNode(
            registry.widget(QNAME[key]),
            title=node_title,
            position=position,
            properties=properties,
        )
        scheme.add_node(node)
        nodes[key] = node

    links = BASE_LINKS + (CLASSIFICATION_ONLY_LINKS if classification else ())
    for source, source_channel, sink, sink_channel in links:
        scheme.add_link(
            SchemeLink(
                nodes[source],
                source_channel,
                nodes[sink],
                sink_channel,
            )
        )

    destination.parent.mkdir(parents=True, exist_ok=True)
    scheme.save_to(str(destination), pretty=True, pickle_fallback=True)
    verify_workflow(
        registry,
        destination,
        data_path,
        feature_names,
        target_name,
        classification=classification,
    )


def _link_signature(link: SchemeLink) -> tuple[str, str, str, str]:
    return (
        link.source_node.description.qualified_name,
        link.source_channel.id,
        link.sink_node.description.qualified_name,
        link.sink_channel.id,
    )


def _expected_link_signatures(*, classification: bool) -> set[tuple[str, str, str, str]]:
    links = BASE_LINKS + (CLASSIFICATION_ONLY_LINKS if classification else ())
    return {
        (QNAME[source], source_channel, QNAME[sink], sink_channel)
        for source, source_channel, sink, sink_channel in links
    }


def verify_workflow(
    registry: WidgetRegistry,
    path: Path,
    data_path: Path,
    feature_names: tuple[str, ...],
    target_name: str,
    *,
    classification: bool,
) -> None:
    """Round-trip .ows và chặn node/link/setting không mong đợi."""

    loaded = Scheme()
    loaded.load_from(str(path), registry=registry)
    qnames = {node.description.qualified_name for node in loaded.nodes}
    expected_qnames = ALLOWED_QNAMES if classification else ALLOWED_QNAMES - {QNAME["confusion"]}
    if qnames != expected_qnames:
        raise AssertionError(
            f"{path.name}: widget allowlist sai; thực tế={sorted(qnames)!r}"
        )

    links = {_link_signature(link) for link in loaded.links}
    expected_links = _expected_link_signatures(classification=classification)
    if links != expected_links:
        raise AssertionError(f"{path.name}: topology không khớp allowlist.")

    by_qname = {node.description.qualified_name: node for node in loaded.nodes}
    file_node = by_qname[QNAME["file"]]
    recent_paths = file_node.properties.get("recent_paths", [])
    if len(recent_paths) != 1:
        raise AssertionError(f"{path.name}: File phải có đúng một recent path.")
    recent = recent_paths[0]
    expected_relative = data_path.resolve().relative_to(ORANGE_DIR.resolve()).as_posix()
    if recent.prefix != "basedir" or recent.relpath.replace("\\", "/") != expected_relative:
        raise AssertionError(f"{path.name}: dataset path không portable theo basedir.")

    sampler = by_qname[QNAME["sampler"]].properties
    if sampler.get("sampling_type") != OWDataSampler.FixedProportion:
        raise AssertionError(f"{path.name}: Data Sampler không dùng Fixed Proportion.")
    if sampler.get("sampleSizePercentage") != 70 or not sampler.get("use_seed"):
        raise AssertionError(f"{path.name}: Data Sampler không phải 70% reproducible.")
    if bool(sampler.get("stratify")) is not classification:
        raise AssertionError(f"{path.name}: setting stratify sai loại bài toán.")

    test_score = by_qname[QNAME["test_score"]].properties
    if test_score.get("resampling") != OWTestAndScore.TestOnTest:
        raise AssertionError(f"{path.name}: Test & Score không dùng Test on test data.")

    tree = by_qname[QNAME["tree"]].properties
    expected_tree = {"max_depth": 4, "min_internal": 4, "min_leaf": 2}
    for setting, value in expected_tree.items():
        if tree.get(setting) != value:
            raise AssertionError(f"{path.name}: Tree setting {setting} không bằng {value}.")

    select_properties = by_qname[QNAME["select"]].properties
    assert_select_columns_settings(
        registry,
        select_properties,
        data_path,
        feature_names,
        target_name,
        classification=classification,
    )

    confusion_present = QNAME["confusion"] in qnames
    if confusion_present is not classification:
        raise AssertionError(f"{path.name}: Confusion Matrix xuất hiện sai workflow.")


def main() -> None:
    """Tạo data, sinh workflow và in kết quả xác minh."""

    app = QApplication.instance()
    if app is None:
        app = QApplication([])
    iris_rows = write_iris_csv(IRIS_CSV)
    ames_rows = write_ames_csv(AMES_SOURCE, AMES_CSV)
    registry = build_registry()

    build_workflow(
        registry,
        CLASSIFICATION_OWS,
        IRIS_CSV,
        IRIS_FEATURES,
        IRIS_TARGET,
        classification=True,
    )
    build_workflow(
        registry,
        REGRESSION_OWS,
        AMES_CSV,
        AMES_FEATURES,
        AMES_TARGET,
        classification=False,
    )

    print(f"Orange {Orange.__version__}")
    print(f"[OK] {IRIS_CSV.relative_to(PROJECT_ROOT)}: {iris_rows} dòng")
    print(f"[OK] {AMES_CSV.relative_to(PROJECT_ROOT)}: {ames_rows} dòng")
    print(f"[OK] {CLASSIFICATION_OWS.relative_to(PROJECT_ROOT)}: 9 node, 11 link")
    print(f"[OK] {REGRESSION_OWS.relative_to(PROJECT_ROOT)}: 8 node, 10 link")
    print("[OK] Registry round-trip, target contexts và qualified-name allowlist")
    app.processEvents()


if __name__ == "__main__":
    main()
