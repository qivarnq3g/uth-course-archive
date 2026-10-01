# Polynomial Regression cho chất lượng rượu bằng Orange

## 1. Mục tiêu và căn cứ học thuật

Bài thực hành xây dựng hai workflow Orange Data Mining độc lập cho rượu đỏ và
rượu trắng, dùng `quality` làm target hồi quy, so sánh Polynomial Regression
bậc 2 và bậc 3 bằng MAE, RMSE và R² trên holdout 30% chưa dùng để huấn luyện.

Đây là phần thực hành do người dùng chọn triển khai, không phải một bài nộp được
PDF bài tập `AI-Ex-2027.pdf` chỉ định. Nội dung phù hợp Chương 6 - Machine
Learning trong `AI2027.pdf`: trang 319 nêu mô hình bậc `k`, trang 320 chuyển bài
toán thành Linear Regression trên feature space, và trang 321 nêu mô hình đa
biến bậc hai có các bình phương cùng interaction term.

Với một biến:

\[
\hat y = \theta_0 + \theta_1x + \theta_2x^2 + \cdots + \theta_kx^k.
\]

Mô hình vẫn tuyến tính theo các hệ số \(\theta\). Tính phi tuyến nằm trong các
feature đã biến đổi như \(x^2\), \(x^3\) và \(x_1x_2\). Với nhiều biến, workflow
mở rộng toàn bộ monomial đến degree đang xét rồi fit Linear Regression.

## 2. Dataset, feature và target

Hai nguồn được kiểm tra trực tiếp trong catalog của widget **Datasets** thuộc
Orange 3.40.0, không phải CSV ngẫu nhiên:

| Dataset | Tên hiển thị | Catalog ID | Dòng | Cột | Feature | Target | Missing |
| --- | --- | --- | ---: | ---: | ---: | --- | ---: |
| Rượu đỏ | Wine quality - red | `winequality-red.tab` | 1.599 | 12 | 11 | `quality`, numeric | 0 |
| Rượu trắng | Wine quality - white | `winequality-white.tab` | 4.898 | 12 | 11 | `quality`, numeric | 0 |

Mười một feature liên tục ở cả hai bộ là: `fixed acidity`, `volatile acidity`,
`citric acid`, `residual sugar`, `chlorides`, `free sulfur dioxide`,
`total sulfur dioxide`, `density`, `pH`, `sulphates`, `alcohol`.

`quality` là điểm số có ý nghĩa ordinal, nhưng bài này chủ động xử lý nó như một
biến số liên tục cho regression. Select Columns giữ đủ 11 feature và đặt
`quality` làm target duy nhất; target không xuất hiện trong ma trận feature.

## 3. Vì sao không dùng trực tiếp widget Polynomial Regression

Registry thật của Orange 3.40.0 có add-on widget
`orangecontrib.educational.widgets.owpolynomialregression.OWPolynomialRegression`.
Tuy nhiên description và mã nguồn của widget xác nhận nó là **univariate
regression**: chỉ chọn một predictor. Dùng widget này sẽ buộc loại 10 feature và
không tạo interaction đa biến, trái yêu cầu dữ liệu.

Do đó workflow dùng mô hình tương đương đa biến:

```text
Python Script: PolynomialFeatures(include_bias=False)
    -> Linear Regression (ordinary least squares)
```

Python Script chỉ mở rộng các `ContinuousVariable` trong Features, bảo toàn
target và meta, kiểm tra target leakage, tên trùng, NaN và infinity. Không có
imputation, encoding hay scaling. Polynomial expansion là phép biến đổi xác
định, không học thống kê từ dữ liệu, nên có thể thực hiện trước split. Linear
Regression chỉ fit trên nhánh train.

Với 11 feature đầu vào:

- Degree 2 tạo \(\binom{11+2}{2}-1=77\) feature.
- Degree 3 tạo \(\binom{11+3}{3}-1=363\) feature.

## 4. Kiến trúc hai workflow

Hai file có cùng topology; widget Datasets khác catalog ID:

- `orange/wine_poly_red.ows`: `winequality-red.tab`.
- `orange/wine_poly_white.ows`: `winequality-white.tab`.

Luồng tổng quát:

```text
Datasets -> Raw Data
    |
    -> Select Columns (Target = quality) -> Selected Data
         |                                
         +-> Polynomial Features d=2 -> Data Sampler 70/30 seed 42
         |      +-> train -> Linear Regression d=2 -> Model -> Predictions d=2
         |      |                  | Learner                 ^
         |      |                  +-> Test & Score d=2      | test
         |      +-> test ---------------------------> Test & Score d=2
         |      Predictions -> Residual d=2 -> Prediction Table d=2
         |
         +-> Polynomial Features d=3 -> Data Sampler 70/30 seed 42
                +-> train -> Linear Regression d=3 -> Model -> Predictions d=3
                |                  | Learner                 ^
                |                  +-> Test & Score d=3      | test
                +-> test ---------------------------> Test & Score d=3
                Predictions -> Residual d=3 -> Prediction Table d=3
```

Mỗi workflow có 18 node và 23 link. Data Table chỉ là điểm xem dữ liệu, không
nằm trên đường bắt buộc đến learner. Predictions nhận đồng thời Model và test
data. Test & Score nhận Learner, train data và test data; không nhận Model.
Script Residual thêm `residual = quality thật - quality dự đoán`, nên Prediction
Table hiển thị target thật, prediction và residual.

## 5. Chia train/test và chống leakage

Hai Data Sampler đều dùng Fixed Proportion 70%, sampling không replacement,
không stratify vì target liên tục, và bật reproducible. Mã nguồn Orange 3.40.0
xác nhận `OWDataSampler.RandomSeed = 42`. Orange dùng `ceil(0.70 * n)` cho Data
Sample, vì vậy kích thước thực là:

| Dataset | Train | Test |
| --- | ---: | ---: |
| Rượu đỏ | 1.120 | 479 |
| Rượu trắng | 3.429 | 1.469 |

Hai nhánh degree dùng cùng thuật toán và seed, nên lấy cùng chỉ số dòng. Script
kiểm chứng độc lập tái tạo đúng quy tắc `ShuffleSplit` này. Test data chỉ dùng để
dự đoán và báo metric; không dùng để fit hay chỉnh degree. Sau khi thấy degree 3
suy giảm, workflow vẫn giữ cấu hình ordinary least squares ban đầu thay vì
chuyển sang Ridge dựa trên test.

## 6. Kết quả chạy thật

Nguồn số là `outputs/wine-polynomial/metrics.json`, sinh bởi
`scripts/verify_wine_polynomial.py` với cùng dataset catalog, split và seed.

| Dataset | Degree | Train/Test | Feature | MAE | RMSE | R² | Fit (giây) |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Red | 2 | 1.120 / 479 | 11 -> 77 | 0,500840 | 0,663791 | 0,297321 | 0,0083 |
| Red | 3 | 1.120 / 479 | 11 -> 363 | 0,696647 | 1,957216 | -5,109028 | 0,0461 |
| White | 2 | 3.429 / 1.469 | 11 -> 77 | 0,560103 | 0,730545 | 0,322226 | 0,0157 |
| White | 3 | 3.429 / 1.469 | 11 -> 363 | 0,599312 | 0,871150 | 0,036222 | 0,0826 |

Thời gian chỉ là phép đo tham khảo của lần chạy hiện tại, phụ thuộc máy.

## 7. So sánh và diễn giải

Degree 2 tốt hơn degree 3 trên cả hai dataset theo cả ba metric holdout. Với rượu
đỏ, degree 3 có RMSE tăng mạnh và R² âm sâu, nghĩa là trên holdout nó còn tệ hơn
dự đoán hằng bằng trung bình target. Với rượu trắng, degree 3 cũng giảm R² từ
0,322226 xuống 0,036222. Kết quả phù hợp nguy cơ variance/đa cộng tuyến khi số
feature tăng từ 77 lên 363; degree lớn hơn không mặc nhiên tốt hơn.

So giữa dataset ở degree 2, rượu đỏ có MAE/RMSE thấp hơn, còn rượu trắng có R²
nhỉnh hơn. Không nên chỉ dùng một metric để nói dataset nào "dễ" hơn vì phân bố
điểm quality và kích thước mẫu khác nhau.

Đây là so sánh hai mô hình bắt buộc, không phải quy trình chọn và khóa một mô
hình cuối cùng. Nếu muốn thử Ridge, cần đặt trước một validation protocol hoặc
cross-validation chỉ trên train, giữ cùng chính sách regularization cho d=2 và
d=3, rồi đánh giá một test set chưa bị dùng để quyết định.

## 8. Cách mở và chạy workflow

1. Mở Orange 3.40.0.
2. Mở `orange/wine_poly_red.ows` hoặc `orange/wine_poly_white.ows`.
3. Chờ widget Datasets tải/phát dữ liệu từ catalog; lần đầu cần mạng.
4. Kiểm tra Select Columns có 11 Features và target `quality`.
5. Mở Test & Score d=2/d=3 để xem MAE, RMSE, R².
6. Mở Prediction Table để xem `quality`, prediction và `residual`.

Canvas được bố trí từ trái sang phải, hai nhánh degree tách theo hàng, không có
widget thừa. `Orange.canvas.run` xác nhận signal chạy end-to-end; vẫn nên mở GUI
để kiểm tra hình thức trên màn hình thật trước khi chụp bài nộp.

## 9. Chạy script kiểm chứng và test

Khi `.venv` chuẩn của workspace đã có:

```powershell
.venv\Scripts\python.exe scripts\verify_wine_polynomial.py
.venv\Scripts\python.exe -m pytest
.venv\Scripts\python.exe -m compileall classification_tree_iris regression_tree scripts tests
```

Script tải/cache đúng hai file `.tab` từ `https://datasets.biolab.si/` vào
`orange/data/`, không cần import Orange, rồi ghi:

- `outputs/wine-polynomial/metrics.csv`
- `outputs/wine-polynomial/metrics.json`
- `outputs/wine-polynomial/dataset-summary.csv`

Test mới kiểm tra dataset, target leakage, missing, split tái lập, số feature,
metric hữu hạn, XML parse được, node/link hợp lệ, Predictions có đủ Data/Model và
Test & Score nhận đúng Learner/train/test.

## 10. Kiểm thử Orange CLI

Lệnh:

```powershell
.\orange\test_workflows.ps1
```

Ngày 2026-07-11, cả bốn workflow cũ và mới đều exit `0`; riêng hai workflow wine
đều truyền dữ liệu qua expansion, model, Predictions, residual table và Test &
Score mà không có widget Error, traceback hoặc OSError. Timeout mặc định là 180
giây mỗi workflow; wrapper trả `124` nếu có timeout và lưu stdout/stderr riêng
tại `outputs/orange-cli-check/`.

Warning còn lại là warning nội bộ Orange:

- `OWLinearRegression.apply` chưa được decorate theo API deferred mới.
- Với workflow white, hai warning `combo 'class_selection' is empty` xuất hiện
  trong lúc khởi tạo widget nhưng không làm mất output hay đổi exit code.

## 11. Hạn chế

- `quality` là điểm ordinal nhưng đang được coi là continuous numeric.
- Polynomial expansion làm số feature tăng nhanh và degree 3 cho thấy dấu hiệu
  bất ổn/overfitting; chưa có regularization được chọn bằng train-only validation.
- Không có scaling; ordinary least squares không cần scale để biểu diễn mô hình,
  nhưng lũy thừa và interaction có thể làm điều kiện số xấu.
- Metric là kết quả của một holdout seed 42, chưa mô tả đầy đủ độ biến thiên qua
  nhiều split.
- Tương quan giữa thuộc tính hóa lý và quality không chứng minh quan hệ nhân quả.
- CLI không đánh giá mỹ thuật canvas; cần kiểm tra GUI thủ công trước khi nộp.
- Trong phiên triển khai này, `.venv` biến mất sau khi thao tác cài Orange bị
  người dùng dừng; theo yêu cầu không cài vào `.venv`, script đã được chạy xác
  minh bằng Python của Orange 3.40.0. Vì vậy lệnh pytest chuẩn bằng `.venv` chưa
  thể chạy lại trong phiên này; sáu hàm kiểm tra mới đã được gọi trực tiếp và đều
  pass, nhưng đây không thay thế một lần `pytest` chính thức.
