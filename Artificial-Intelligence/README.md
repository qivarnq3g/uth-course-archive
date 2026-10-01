# Decision Tree: Iris, Regression toy và Ames

Project triển khai hai loại cây quyết định bằng Python, lưu kết quả chạy thật và cung cấp workflow Orange để thực hành trực quan. Nội dung được tách rõ giữa bài luyện có trong PDF, phạm vi người dùng giao và phần mở rộng đánh giá.

> [!IMPORTANT]
> `AI-Ex-2027.pdf` xác nhận bài 7.1 Regression Tree trên bốn bộ dữ liệu toy và bài 7.3 Classification Tree trên Iris. PDF không xác nhận Body Fat, Orange, đúng hai bài phải nộp hoặc một “bài số 3” bắt buộc.

## Phạm vi và căn cứ

| Phần | Trạng thái | Căn cứ |
| --- | --- | --- |
| Classification Tree - Iris | Bài luyện được PDF xác nhận | `AI-Ex-2027.pdf`, trang 19, mục 7.3 |
| Regression Tree - bốn bộ toy, `max_depth=3` | Bài luyện được PDF xác nhận | `AI-Ex-2027.pdf`, trang 18, mục 7.1 |
| Thực hành bằng Python và Orange | Phạm vi người dùng yêu cầu | Không phải yêu cầu trực tiếp của hai mục PDF trên |
| Regression Tree - Ames | Phần mở rộng để có tập đủ lớn cho holdout và metric | Ames có sẵn trong workspace; không phải dataset mục 7.1 |
| Linear, Logistic và Polynomial Regression | Tài liệu ôn thêm | Không gọi là “bài số 3” |

Ma trận bằng chứng đầy đủ nằm tại [docs/assignment-evidence.md](docs/assignment-evidence.md).

## Workspace sau migration

Workspace thực thi chính thức là `D:\courses\Artificial-Intelligence`. OLD_ROOT `D:\courses\Trí tuệ nhân tạo` chỉ còn là dữ kiện lịch sử của lỗi đường dẫn Unicode, không được dùng trong lệnh hoặc cấu hình hoạt động.

Ngày 2026-07-10, `.venv` đã được tạo lại tại NEW_ROOT bằng Python 3.11.9; `compileall` thành công, toàn bộ `8` test pass và cả hai workflow Orange 3.40.0 trả exit code `0` qua CLI. GUI vẫn cần được mở thủ công để kiểm tra bố cục và chụp ảnh. Chi tiết checksum và phạm vi file thay đổi nằm tại [docs/path-migration.md](docs/path-migration.md).

## Cấu trúc chính

```text
classification_tree_iris/       Mã Classification Tree trên Iris
regression_tree/                Bài 7.1 toy và phần mở rộng Ames
tests/                          8 test cho dữ liệu, split, metric và artifact
outputs/                        JSON, CSV và hình sinh từ lần chạy thật
orange/                         Hai workflow .ows và dữ liệu CSV cục bộ
docs/                           Bằng chứng, báo cáo, hướng dẫn và tài liệu ôn
requirements.txt                Dependency Python của project
```

PDF, dataset Ames và project House Price gốc chỉ được đọc; project này không sửa hoặc ghi đè chúng.

## Yêu cầu môi trường

- Windows PowerShell.
- Python tương thích với các ràng buộc trong `requirements.txt`.
- Orange Data Mining 3.40.0 nếu muốn mở `.ows`; Orange là phần thực hành bổ sung, không cần để chạy mã Python.

## Cài đặt

Từ thư mục root của project:

```powershell
python -m venv .venv
& '.\.venv\Scripts\python.exe' -m pip install -r requirements.txt
$env:PYTHONIOENCODING='utf-8'
```

Không cần và không nên cài package global.

## Chạy project

```powershell
# Classification Tree - Iris
& '.\.venv\Scripts\python.exe' -m classification_tree_iris.main

# Regression Tree - bốn bộ toy của bài 7.1
& '.\.venv\Scripts\python.exe' -m regression_tree.exercise_7_1

# Regression Tree - phần mở rộng Ames
& '.\.venv\Scripts\python.exe' -m regression_tree.main

# Kiểm thử
& '.\.venv\Scripts\python.exe' -m pytest

# Kiểm tra cú pháp/import bytecode
& '.\.venv\Scripts\python.exe' -m compileall classification_tree_iris regression_tree tests
```

Grid search Ames có thể mất thời gian. Nếu các artifact hiện có còn nguyên và chỉ cần đọc kết quả, không cần chạy lại model.

## Kết quả đã xác minh

Các số dưới đây được đọc từ JSON trong `outputs/`, không hard-code trong mã huấn luyện.

### Classification Tree - Iris

- Split: 105 train / 45 test, phân tầng, `random_state=42`.
- Cấu hình tốt nhất theo 5-fold CV trên train: `criterion=gini`, `max_depth=3`, `min_samples_split=2`, `min_samples_leaf=4`.
- Macro-F1 CV: `0.952332`.

| Test metric | Giá trị |
| --- | ---: |
| Accuracy | 0.888889 |
| Precision macro | 0.889881 |
| Recall macro | 0.888889 |
| F1 macro | 0.888765 |

Confusion Matrix dùng hàng là lớp thật, cột là lớp dự đoán:

| Actual \ Predicted | Setosa | Versicolor | Virginica |
| --- | ---: | ---: | ---: |
| Setosa | 15 | 0 | 0 |
| Versicolor | 0 | 12 | 3 |
| Virginica | 0 | 2 | 13 |

Nguồn số: [metrics.json](outputs/classification-tree/metrics.json) và [confusion_matrix.csv](outputs/classification-tree/confusion_matrix.csv).

### Regression Tree - bài 7.1

Cả bốn case dùng `DecisionTreeRegressor(max_depth=3, random_state=42)` và fit toàn bộ 3-5 mẫu để vẽ cây theo đề. Train MAE/RMSE bằng 0 và train R² bằng 1 chỉ cho thấy cây khớp tập toy cực nhỏ; đây không phải bằng chứng tổng quát hóa.

Nguồn số: [summary.json](outputs/regression-tree/exercise-7-1/summary.json).

### Regression Tree - Ames mở rộng

- Split: 2.051 train / 879 test, `random_state=42`.
- Tuning: 5-fold CV chỉ trên train; preprocessing được fit trong pipeline ở từng fold.
- Cấu hình tốt nhất: `max_depth=10`, `min_samples_split=2`, `min_samples_leaf=5`.
- CV RMSE: `34,949.66 USD`.

| Tập | MAE (USD) | RMSE (USD) | R² |
| --- | ---: | ---: | ---: |
| Train - chỉ tham khảo | 14,240.08 | 23,122.74 | 0.912146 |
| Test - kết quả chính | 22,691.65 | 34,913.24 | 0.826596 |

Nguồn số: [metrics.json](outputs/regression-tree/ames-extension/metrics.json).

## Output

- `outputs/classification-tree/`: metric, CV, classification report, dự đoán, feature importance, cây và Confusion Matrix.
- `outputs/regression-tree/exercise-7-1/`: bốn hình cây, dự đoán train và summary.
- `outputs/regression-tree/ames-extension/`: metric, CV, dự đoán, feature importance, cây ba tầng đầu và Actual vs Predicted.
- `outputs/orange-cli-check/`: stdout/stderr của kiểm tra end-to-end bốn workflow Orange.
- `outputs/wine-polynomial/`: dataset summary và metric MAE, RMSE, R² cho degree 2/3.

## Orange Data Mining

Hướng dẫn từng widget, target, feature, signal, ảnh cần chụp và lỗi thường gặp nằm tại [docs/orange-workflows.md](docs/orange-workflows.md).

Các file có thể mở trực tiếp:

- [orange/classification-tree.ows](orange/classification-tree.ows) - Iris.
- [orange/regression-tree.ows](orange/regression-tree.ows) - Ames mở rộng.
- [orange/wine_poly_red.ows](orange/wine_poly_red.ows) - Polynomial Regression rượu đỏ.
- [orange/wine_poly_white.ows](orange/wine_poly_white.ows) - Polynomial Regression rượu trắng.

Các workflow đã được round-trip với registry thật của Orange 3.40.0 và chạy end-to-end bằng `Orange.canvas.run`. Cả bốn trả exit code `0`; có thể chạy lại bằng [orange/test_workflows.ps1](orange/test_workflows.ps1). Bài toy được hướng dẫn tạo thủ công cho từng CSV vì bốn case có số feature khác nhau và dữ liệu quá ít để coi holdout là đánh giá đáng tin cậy.

Phần Wine Polynomial dùng hai dataset chính thức từ catalog Orange, `quality` làm target, mở rộng đa thức đa biến degree 2/3 rồi fit Linear Regression trên split 70/30 seed 42. Xem [tài liệu thực hành](docs/wine-polynomial-orange.md), [script kiểm chứng](scripts/verify_wine_polynomial.py), [metrics CSV](outputs/wine-polynomial/metrics.csv) và [metrics JSON](outputs/wine-polynomial/metrics.json).

## Tài liệu

- [Báo cáo kết quả](docs/bao-cao.md)
- [Ôn Linear, Logistic và Polynomial Regression](docs/review-linear-logistic.md)
- [Bằng chứng đề bài](docs/assignment-evidence.md)
- [Hướng dẫn Orange](docs/orange-workflows.md)
- [Polynomial Regression rượu đỏ và rượu trắng](docs/wine-polynomial-orange.md)
- [Biên bản migration đường dẫn](docs/path-migration.md)

## Giới hạn

- Tập test Iris chỉ có 45 mẫu nên metric nhạy với một vài dự đoán.
- Bốn bộ toy chỉ có 3-5 mẫu; không dùng train R² bằng 1 để kết luận mô hình tổng quát hóa tốt.
- Ames là phần mở rộng trên dữ liệu nhà ở Mỹ, không phải Body Fat, không phải dataset mục 7.1 và không đại diện thị trường Việt Nam.
- Kết quả Orange có thể khác Python vì workflow Orange minh họa dùng cấu hình/feature riêng; metric Python trong báo cáo phải lấy từ JSON.
- `Orange.canvas.run` xác nhận thực thi kỹ thuật nhưng không đánh giá bố cục canvas hoặc độ dễ đọc của widget; vẫn cần mở GUI để kiểm tra hình thức và chụp ảnh nộp bài.

## Mở và chuẩn bị nộp

1. Đọc [docs/bao-cao.md](docs/bao-cao.md) và chọn các bảng/hình phù hợp với yêu cầu lớp.
2. Mở `.ows` bằng Orange, nhấn **Send/Apply** nếu widget chưa tự cập nhật và chụp canvas, Test & Score, cây, Predictions; với Iris chụp thêm Confusion Matrix.
3. Nếu quy định Elearning áp dụng cho lượt nộp này, đưa đề và kết quả vào Word rồi xuất PDF theo `introduce.txt`.
4. Không trình bày Ames/Orange như yêu cầu trực tiếp của PDF và không gọi phần ôn Regression là “bài số 3”.
