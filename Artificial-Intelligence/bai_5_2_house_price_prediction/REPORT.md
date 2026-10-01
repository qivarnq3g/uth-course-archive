# Báo cáo bài 5.2: House Price Prediction

## 1. Giới thiệu bài toán

Bài toán House Price Prediction yêu cầu xây dựng mô hình dự đoán giá bán nhà dựa trên các đặc trưng mô tả căn nhà và môi trường xung quanh. Trong project này, mô hình được sử dụng là Linear Regression, một phương pháp học máy có giám sát phù hợp cho bài toán dự đoán giá trị liên tục.

## 2. Mô tả dataset

Project sử dụng Ames Housing Dataset từ OpenIntro. Dataset chứa dữ liệu nhà dân cư bán tại Ames, Iowa trong giai đoạn 2006-2010, được thu thập từ Ames Assessor's Office. Theo mô tả của OpenIntro, dataset có 2930 dòng và 82 biến.

Biến mục tiêu của bài toán là:

- `price`: giá bán thực tế của căn nhà, đơn vị USD.

Một số nhóm biến được sử dụng:

- Diện tích: `area`
- Số phòng và số phòng tắm: `Bedroom.AbvGr`, `Full.Bath`, `Half.Bath`, `TotRms.AbvGrd`
- Vị trí: `Neighborhood`
- Tiện ích và điều kiện xung quanh: `Utilities`, `Condition.1`, `Condition.2`
- Chất lượng và tình trạng tổng thể: `Overall.Qual`, `Overall.Cond`

## 3. Ánh xạ yêu cầu đề bài với cột dữ liệu

| Yêu cầu đề bài | Cột trong dataset |
| --- | --- |
| Diện tích | `area` |
| Số phòng | `Bedroom.AbvGr`, `TotRms.AbvGrd` |
| Vị trí | `Neighborhood` |
| Tiện ích/điều kiện xung quanh | `Utilities`, `Condition.1`, `Condition.2` |
| Giá bán thực tế | `price` |

## 4. Phương pháp Linear Regression

Linear Regression giả định rằng giá nhà có thể được xấp xỉ bằng một tổ hợp tuyến tính của các đặc trưng đầu vào. Mô hình học các hệ số sao cho sai số giữa giá dự đoán và giá thực tế trên tập huấn luyện là nhỏ nhất.

Công thức tổng quát:

```text
y_hat = beta_0 + beta_1*x_1 + ... + beta_n*x_n
```

Trong đó:

- `y_hat` là giá nhà dự đoán.
- `beta_0` là hệ số chặn.
- `beta_1 ... beta_n` là các hệ số của mô hình.
- `x_1 ... x_n` là các đặc trưng đầu vào.

Trong project này, `LinearRegression` được dùng làm mô hình baseline. Vì `LinearRegression` của scikit-learn không có tham số `alpha`, project dùng thêm `Ridge` để thử các giá trị `alpha` theo tinh thần yêu cầu tinh chỉnh mô hình. Ridge Regression vẫn là một mô hình hồi quy tuyến tính, nhưng có thêm L2 regularization để hạn chế hệ số quá lớn.

## 5. Tiền xử lý dữ liệu

Các bước tiền xử lý được thực hiện trong `main.py`:

1. Đọc hoặc tải Ames Housing Dataset.
2. Chuẩn hóa tên cột nếu dữ liệu đến từ nguồn fallback.
3. Chọn các cột cần thiết cho bài toán.
4. Loại bỏ dòng không có giá bán hợp lệ.
5. Xử lý giá trị thiếu của biến numeric bằng median.
6. Xử lý giá trị thiếu của biến categorical bằng giá trị xuất hiện nhiều nhất.
7. Chuẩn hóa biến numeric bằng `StandardScaler`.
8. Mã hóa biến categorical bằng `OneHotEncoder`.
9. Chia dữ liệu train/test theo tỷ lệ 80/20.

## 6. Tinh chỉnh mô hình

Project thử nghiệm 10 cấu hình, gồm:

- Mô hình baseline: `LinearRegression`
- Mô hình có điều chuẩn: `Ridge(alpha=0.1)`, `Ridge(alpha=1.0)`, `Ridge(alpha=10.0)`, `Ridge(alpha=100.0)`
- Hai bộ đặc trưng:
  - `basic_features`: `area`, `Bedroom.AbvGr`, `Full.Bath`, `TotRms.AbvGrd`, `Neighborhood`
  - `full_features`: toàn bộ 13 feature chính trong project

Mỗi cấu hình đều dùng cách chia train/test 80/20 với `random_state=42`. Mô hình tốt nhất được chọn theo RMSE thấp nhất.

Bảng so sánh được lưu trong:

```text
results/model_comparison.csv
```

Kết quả so sánh chính:

| Feature set | Model | Alpha | MAE | RMSE | R2 |
| --- | --- | ---: | ---: | ---: | ---: |
| full_features | Ridge | 10.0 | 22282.89 | 37114.75 | 0.8282 |
| full_features | Ridge | 1.0 | 22029.41 | 37159.60 | 0.8278 |
| full_features | Ridge | 0.1 | 22052.40 | 37329.92 | 0.8262 |
| full_features | LinearRegression |  | 22057.74 | 37363.01 | 0.8259 |
| full_features | Ridge | 100.0 | 24098.64 | 38778.50 | 0.8124 |
| basic_features | Ridge | 1.0 | 27741.19 | 42826.09 | 0.7712 |
| basic_features | Ridge | 0.1 | 27754.91 | 42840.81 | 0.7711 |
| basic_features | LinearRegression |  | 27756.95 | 42843.92 | 0.7711 |
| basic_features | Ridge | 10.0 | 28068.04 | 43201.86 | 0.7672 |
| basic_features | Ridge | 100.0 | 31195.98 | 48279.24 | 0.7093 |

Kết quả tốt nhất là `Ridge(alpha=10.0)` với `full_features`.

## 7. Đánh giá mô hình

Mô hình được đánh giá trên tập test bằng ba chỉ số:

- MAE: sai số tuyệt đối trung bình.
- RMSE: căn bậc hai của sai số bình phương trung bình.
- R2: mức độ mô hình giải thích biến động của giá nhà.

Kết quả chạy kiểm tra trên Ames Housing Dataset:

| Chỉ số | Giá trị |
| --- | ---: |
| Số dòng dataset | 2930 |
| Số cột dataset | 82 |
| Số mẫu train | 2344 |
| Số mẫu test | 586 |
| Best model | Ridge(alpha=10.0) |
| Best feature set | full_features |
| MAE | 22282.89 |
| RMSE | 37114.75 |
| R2 | 0.8282 |

Kết quả chi tiết sau khi chạy chương trình được lưu trong:

```text
results/metrics.json
```

Biểu đồ đánh giá được lưu tại:

```text
results/figures/actual_vs_predicted.png
results/figures/residuals.png
```

Mô hình baseline `LinearRegression` với `full_features` đạt RMSE = 37363.01 và R2 = 0.8259. Sau tuning, `Ridge(alpha=10.0)` với `full_features` đạt RMSE = 37114.75 và R2 = 0.8282, tức là cải thiện nhẹ so với baseline theo tiêu chí RMSE.

## 8. Nhận xét kết quả

Linear Regression và Ridge đều cho kết quả dễ giải thích vì mô hình có hệ số cho từng đặc trưng sau khi tiền xử lý. Với R2 = 0.8282 ở cấu hình tốt nhất, mô hình giải thích được phần lớn biến động của giá nhà trên tập test. MAE khoảng 22282.89 USD cho thấy sai số trung bình vẫn còn đáng kể, phù hợp với thực tế là giá nhà có thể phụ thuộc vào quan hệ phi tuyến, tương tác giữa biến, hoặc yếu tố thị trường không có trong dataset.

File `results/model_coefficients.csv` giúp xem các đặc trưng sau mã hóa có ảnh hưởng như thế nào đến dự đoán của mô hình.

## 9. Hạn chế

- Dataset ở Mỹ, không phản ánh trực tiếp thị trường Việt Nam.
- Linear Regression giả định quan hệ tuyến tính giữa đặc trưng và giá nhà.
- Giá nhà thực tế còn bị ảnh hưởng bởi nhiều yếu tố khác như lãi suất, thời điểm giao dịch, hạ tầng khu vực và xu hướng thị trường.
- Một số biến categorical sau khi One-Hot Encoding làm số chiều dữ liệu tăng lên.

## 10. Kết luận

Project đã xây dựng pipeline hoàn chỉnh bằng Python để dự đoán giá nhà bằng hồi quy tuyến tính. Dữ liệu sử dụng là dữ liệu thật, open-source từ Ames Housing Dataset, không phải dữ liệu mô phỏng. Chương trình có thể chạy bằng command line, tự lưu model baseline, best model, metrics, bảng so sánh tuning, dự đoán mẫu và biểu đồ để phục vụ việc nộp bài.
