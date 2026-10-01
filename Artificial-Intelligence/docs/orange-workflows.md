# Hướng dẫn workflow Orange cho Decision Tree

Tài liệu này mô tả cách dựng lại ba workflow trong Orange bằng dữ liệu cục bộ của project. Orange là phạm vi thực hành do người dùng yêu cầu; `AI-Ex-2027.pdf` không bắt buộc Orange cho hai bài Decision Tree.

## 1. Trạng thái Orange và file `.ows`

Máy hiện có Orange standalone tại:

```text
C:\Users\Lucian\AppData\Local\Programs\Orange
```

Python nội bộ báo Orange `3.40.0` trên Python `3.12.12`. Executable không nằm trên `PATH`; có thể mở `Orange.lnk` hoặc `Scripts\orange-canvas.exe` trong thư mục trên.

Workspace hiện tại là `D:\courses\Artificial-Intelligence`. Ngày 2026-07-10, cache `abspath` trong hai `RecentPath` đã được tái tạo bằng Orange 3.40.0 tại workspace mới; `prefix="basedir"` và `relpath="data/..."` vẫn giữ nguyên. Node ID, link/signal, topology và model setting không đổi.

Hai workflow đã có:

| File | Dataset | Xác minh bằng Orange 3.40.0 |
| --- | --- | --- |
| `orange/classification-tree.ows` | `data/iris.csv` | 9 node, 11 link, target `species` discrete; CLI exit `0` |
| `orange/regression-tree.ows` | `data/ames_regression.csv` | 8 node, 10 link, target `price` continuous; CLI exit `0` |

Script `orange/create_workflows.py` dùng widget registry thật của bản Orange cài trên máy, không tự bịa ID. Hai file hiện tại đã qua kiểm tra round-trip về qualified widget name, channel ID, target/type, setting và topology. Sau đó cả hai tiếp tục vượt qua runner end-to-end `Orange.canvas.run`: Orange nạp workflow, khởi tạo toàn bộ widget, truyền dữ liệu qua Tree, Predictions, Test & Score và các widget đích, rồi tự thoát với exit code `0` mà không có widget Error.

Phép kiểm tra hậu migration ngày 2026-07-10 tiếp tục trả `Classification: 0` và `Regression: 0`; log mới nằm tại `outputs/orange-cli-check/` và không có traceback, `OSError`, timeout hoặc widget Error.

Đường dẫn File dùng cặp `prefix="basedir"` và `relpath="data/..."`; Orange vẫn lưu một `abspath` cache trong `RecentPath`, nhưng workflow không phụ thuộc duy nhất vào đường dẫn tuyệt đối đó và có thể resolve theo thư mục chứa `.ows`.

Nếu cần tái tạo sau khi thay đổi CSV, chạy bằng Python của Orange:

```powershell
& 'C:\Users\Lucian\AppData\Local\Programs\Orange\python.exe' '.\orange\create_workflows.py'
```

Script chỉ ghi vào `orange/`. Không chạy nó bằng `.venv` của project vì `.venv` không chứa Orange3.

### Kiểm tra end-to-end qua CLI

Chạy wrapper có timeout 180 giây và log riêng cho từng workflow:

```powershell
& '.\orange\test_workflows.ps1'
```

Log được lưu tại `outputs/orange-cli-check/`. Quy ước kết quả:

| Exit code | Ý nghĩa |
| ---: | --- |
| `0` | Signal manager kết thúc và không có widget Error |
| `1` | Có ít nhất một widget ở trạng thái Error |
| `124` | Wrapper chủ động dừng vì timeout; không phải exit code do Orange định nghĩa |

Có thể chạy trực tiếp từng file:

```powershell
$orangePython = Join-Path $env:LOCALAPPDATA 'Programs\Orange\python.exe'
& $orangePython -m Orange.canvas.run -l 5 '.\orange\classification-tree.ows'
& $orangePython -m Orange.canvas.run -l 5 '.\orange\regression-tree.ows'
```

> [!IMPORTANT]
> OLD_ROOT (lịch sử): `D:\courses\Trí tuệ nhân tạo`. NEW_ROOT: `D:\courses\Artificial-Intelligence`. Ở OLD_ROOT, Orange 3.40.0 `Orange.canvas.run` đọc lại argv qua Qt 5; đường dẫn tuyệt đối chứa tiếng Việt có thể bị đổi thành ký tự `?` và gây `OSError` trước khi workflow được nạp. Wrapper đặt working directory về project root và chỉ truyền đường dẫn tương đối ASCII; OLD_ROOT không còn được dùng để thực thi.

Runner là tính năng experimental của Orange. Exit `0` xác nhận thực thi kỹ thuật end-to-end, nhưng không đánh giá bố cục canvas, độ dễ đọc của Tree Viewer hay chất lượng ảnh chụp; các phần hình thức đó vẫn cần xem bằng GUI.

Log DEBUG hiện có một số `UserWarning` nội bộ của Orange, nhưng runner vẫn trả `0`; theo mã nguồn `Orange.canvas.run`, warning được in ra `stderr` còn chỉ node Error mới làm runner thoát với mã `1`.

## 2. Nguyên tắc nối chung

Luồng dự đoán holdout dùng đúng hai output của Data Sampler và hai vai trò khác nhau của Tree:

```text
File/Datasets → Select Columns → Data Sampler

Data Sample   → Tree (Data)                 # huấn luyện model
Remaining Data → Predictions (Data)         # dữ liệu test
Tree Model    → Predictions (Predictors)
Predictions   → Data Table

Tree Learner  → Test & Score (Learner)
Data Sample   → Test & Score (Data)
Remaining Data → Test & Score (Test Data)

Tree Model    → Tree Viewer
```

Trong **Test & Score**, chọn **Test on test data**. Không nối `Tree Model` vào Test & Score: widget này cần **Learner** để tự fit trên Data và đánh giá trên Test Data. Ngược lại, Predictions và Tree Viewer cần **Model** đã fit.

Data Sampler nên đặt:

- Fixed proportion of data: `70%`.
- Sampling without replacement.
- Replicable/deterministic sampling bật.
- Stratify bật cho Iris; tắt cho regression.

Orange 3.40 chỉ cung cấp lựa chọn reproducible trong UI, không có ô nhập đúng số `random_state=42` như scikit-learn. Vì vậy split Orange không được coi là chính cùng split Python.

## 3. Classification Tree - Iris

### Dataset, target và feature

- File: `orange/data/iris.csv`.
- Target: `species` - discrete, gồm `setosa`, `versicolor`, `virginica`.
- Features: `sepal length`, `sepal width`, `petal length`, `petal width`.

Trong **Select Columns**, kéo bốn feature vào **Features**, `species` vào **Target**, không để target trong Meta hoặc Features.

### Widget và kết nối

Thêm lần lượt: **File**, **Select Columns**, **Data Sampler**, **Tree**, **Predictions**, **Data Table**, **Test & Score**, **Confusion Matrix**, **Tree Viewer**.

Nối theo luồng chung ở mục 2, rồi nối thêm:

```text
Test & Score (Evaluation Results) → Confusion Matrix (Evaluation Results)
```

File `.ows` có sẵn đã dùng chính topology này.

### Thiết lập

1. **File:** mở `orange/data/iris.csv`; kiểm tra Orange đọc 150 dòng.
2. **Select Columns:** target `species`; bốn feature như trên.
3. **Data Sampler:** 70%, không replacement, bật replicable và stratify.
4. **Tree:** loại classification sẽ được suy ra từ target discrete. File `.ows` giới hạn depth 4, minimum internal subset 4 và minimum leaf 2 để cây dễ đọc. Nếu muốn mô phỏng cấu hình Python tốt nhất, đặt depth 3, minimum leaf 4 và minimum internal subset 2; kết quả vẫn có thể khác do split Orange khác.
5. **Predictions:** nhận Remaining Data và Tree Model; bật cột predicted class/probability cần xem.
6. **Test & Score:** chọn Test on test data; đọc Accuracy/CA, Precision, Recall và F1. Nêu rõ đây là metric trên Remaining Data của Orange.
7. **Confusion Matrix:** chọn Tree; kiểm tra hàng là actual và cột là predicted theo nhãn hiển thị của widget trước khi diễn giải.
8. **Tree Viewer:** nhận Tree Model; chọn cách tô màu theo lớp và phóng to để đọc node.

### Output nên chụp

- Toàn bộ canvas và các đường nối.
- Select Columns cho thấy target/features.
- Data Sampler 70/30.
- Test & Score với Tree.
- Confusion Matrix có nhãn lớp.
- Tree Viewer ở mức zoom đọc được.
- Predictions hoặc Data Table với actual/predicted.

### Lỗi thường gặp

- `species` bị đọc là text/meta: đặt lại trong Select Columns; nếu vẫn sai, mở File và chỉnh column type thành categorical/discrete.
- Predictions trống: thiếu một trong hai nối Remaining Data → Data hoặc Tree Model → Predictors.
- Test & Score báo thiếu train/test: chọn Test on test data và nối đúng Data Sample/Remaining Data.
- Confusion Matrix không nhận dữ liệu: phải nối Evaluation Results từ Test & Score, không nối trực tiếp Model.
- Metric không giống JSON Python: split/configuration Orange không đồng nhất hoàn toàn với GridSearch Python; không chép số Orange vào báo cáo Python.

## 4. Regression Tree - bốn toy case của bài 7.1

### Dataset, target và feature

Mở từng file riêng:

| Case | File | Features | Target | Số mẫu |
| --- | --- | --- | --- | ---: |
| 1 | `orange/data/regression-exercise-7-1/case_1.csv` | `x1` | `target` | 4 |
| 2 | `orange/data/regression-exercise-7-1/case_2.csv` | `x1` | `target` | 5 |
| 3 | `orange/data/regression-exercise-7-1/case_3.csv` | `x1`, `x2` | `target` | 5 |
| 4 | `orange/data/regression-exercise-7-1/case_4.csv` | `x1`, `x2`, `x3` | `target` | 3 |

`target` phải là continuous/numeric. Mỗi case có số feature khác nhau nên không dùng chung một Select Columns context đã lưu mà không kiểm tra lại role.

### Workflow chính để bám đúng PDF

PDF yêu cầu tính và vẽ cây với `max_depth=3`, không yêu cầu holdout. Với mỗi case, dùng:

```text
File → Select Columns → Tree → Tree Viewer
                         └────→ Predictions ← Select Columns
                                      ↓
                                  Data Table
```

Trong **Tree**, bật giới hạn depth và đặt `3`. Nối toàn bộ dữ liệu vào Tree để cây mô tả đúng tập toy; nối Model và cùng dữ liệu vào Predictions nếu muốn kiểm tra khả năng khớp train.

### Data Sampler và Test & Score chỉ để minh họa

Nếu bài thực hành bắt buộc phải có luồng 70/30, thêm Data Sampler và nối theo mục 2:

- 70%, không replacement, replicable; không stratify vì target liên tục.
- Tree vẫn đặt depth 3.
- Predictions nhận Remaining Data và Tree Model.
- Test & Score chọn Test on test data; xem MAE/RMSE nếu widget tính được.
- Không dùng Confusion Matrix cho regression.

Với 3-5 mẫu, Remaining Data chỉ có khoảng 1-2 dòng. R² có thể không xác định hoặc dao động cực mạnh; MAE/RMSE cũng không đại diện khả năng tổng quát hóa. Không đưa metric holdout toy vào kết luận chính.

### Output nên chụp

- Select Columns của từng case.
- Tree setting `max_depth=3`.
- Tree Viewer của cả bốn case.
- Predictions/Data Table nếu cần chứng minh cây khớp dữ liệu train.
- Nếu chụp Test & Score, ghi chú rõ “minh họa, không đủ mẫu để đánh giá đáng tin cậy”.

### Lỗi thường gặp

- `target` nằm trong Features: kéo sang Target trước khi nối Tree.
- Tree bị nhận thành classification: chỉnh type của `target` thành numeric/continuous trong File.
- R² trống/NaN: số mẫu test quá ít; đây là giới hạn dữ liệu, không sửa bằng cách bịa thêm mẫu.
- Cây không khớp hình Python: kiểm tra depth 3 và việc dùng toàn bộ toy data thay vì chỉ Data Sample.
- Thêm Confusion Matrix: sai loại bài toán; regression không có ma trận nhầm lẫn lớp.

## 5. Regression Tree - Ames mở rộng

### Dataset, target và feature

- File gọn cho workflow có sẵn: `orange/data/ames_regression.csv`.
- Target: `price` - continuous/numeric.
- `.ows` hiện chọn chín feature số: `area`, `Bedroom.AbvGr`, `Full.Bath`, `Half.Bath`, `TotRms.AbvGrd`, `Year.Built`, `Garage.Area`, `Overall.Qual`, `Overall.Cond`.

Nếu CSV được sinh lại bởi `regression_tree.main`, nó có thể chứa thêm các feature được Python dùng; Select Columns của `.ows` vẫn chọn chín cột trên. Orange workflow là minh họa riêng, không phải bản tái tạo tuyệt đối GridSearch/Pipeline Python 19 feature.

### Widget và kết nối

Thêm: **File**, **Select Columns**, **Data Sampler**, **Tree**, **Predictions**, **Data Table**, **Test & Score**, **Tree Viewer**. Nối đúng luồng chung ở mục 2. Không thêm Confusion Matrix.

### Thiết lập

1. **File:** mở `orange/data/ames_regression.csv`; kiểm tra `price` là numeric.
2. **Select Columns:** đặt `price` vào Target; chọn các feature ở trên.
3. **Data Sampler:** 70%, không replacement, replicable; tắt stratify.
4. **Tree:** file `.ows` dùng depth 4, minimum internal subset 4, minimum leaf 2 để workflow chạy gọn và cây dễ quan sát. Cấu hình Python tốt nhất trong JSON là depth 10, split 2, leaf 5; chỉ đặt tương ứng nếu muốn thử, không coi metric Orange là metric Python.
5. **Predictions:** Remaining Data → Data; Tree Model → Predictors; xem predicted `price` cạnh giá thật.
6. **Test & Score:** Test on test data. Orange 3.40 có scorer regression MSE, RMSE, MAE và R²; ưu tiên báo MAE, RMSE, R², ghi rõ split/configuration Orange.
7. **Tree Viewer:** nhận Tree Model; thu gọn/zoom phần đầu cây để ảnh đọc được.

### Output nên chụp

- Canvas và đường nối.
- Select Columns có target `price`.
- Data Sampler 70/30.
- Test & Score hiển thị MAE, RMSE, R².
- Predictions/Data Table có actual và predicted price.
- Tree Viewer ở số tầng đầu có thể đọc.

### Lỗi thường gặp

- File báo không tìm thấy dữ liệu sau khi chuyển project: giữ `.ows` cạnh thư mục `data/`; nếu cần, mở lại file bằng File widget.
- `price` thành discrete/text: đặt lại type numeric trong File rồi gửi sang Select Columns.
- Unknown category hoặc missing value: workflow `.ows` mặc định dùng chín feature số để giảm vấn đề này; với bộ 19 feature, cần kiểm tra cách Orange xử lý missing/category riêng.
- Test & Score chỉ hiện metric classification: target đang sai type.
- Kết quả khác JSON: Orange dùng feature, split, preprocessing và tham số khác; không thay JSON bằng số Orange.

## 6. Checklist trước khi chụp và nộp

- Không có dấu chấm than đỏ hoặc node `Unknown/Missing` trên canvas.
- File widget đọc đúng số dòng và target có đúng type.
- Data Sample huấn luyện Tree; Remaining Data đi vào Predictions và Test Data.
- Learner đi vào Test & Score; Model đi vào Predictions/Tree Viewer.
- Chỉ Iris nối Confusion Matrix.
- Toy Tree dùng depth 3 và không diễn giải train fit như generalization.
- Ảnh có đủ nhãn, không cắt metric hoặc tên lớp/feature.
- Báo cáo ghi rõ Orange là phần thực hành bổ sung và Ames là phần mở rộng.
