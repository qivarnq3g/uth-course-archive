# Bài 5.2: House Price Prediction

Project này xây dựng mô hình hồi quy tuyến tính để dự đoán giá nhà dựa trên dữ liệu Ames Housing Dataset. `LinearRegression` được dùng làm baseline, sau đó project thử thêm `Ridge` với nhiều giá trị `alpha` để đáp ứng phần tinh chỉnh mô hình.

## Mục tiêu bài toán

Mục tiêu là dự đoán giá bán nhà từ các đặc trưng mô tả căn nhà và khu vực xung quanh, ví dụ diện tích, số phòng, số phòng tắm, năm xây dựng, khu dân cư, tiện ích và điều kiện liên quan.

## Nguồn dữ liệu

- Dataset: Ames Housing Dataset, OpenIntro.
- Trang nguồn: https://www.openintro.org/data/index.php?data=ames
- CSV trực tiếp: https://www.openintro.org/data/csv/ames.csv
- Dữ liệu gốc từ Ames Assessor's Office, mô tả nhà dân cư bán tại Ames, Iowa trong giai đoạn 2006-2010.
- Dataset OpenIntro có 2930 dòng và 82 biến.
- Biến mục tiêu: `price`, giá bán nhà theo USD.

Đây là dataset open-source, không phải dữ liệu mô phỏng.

## Các bước thực hiện

1. Thu thập hoặc đọc dữ liệu từ `data/raw/ames.csv`.
2. Kiểm tra dữ liệu và chuẩn hóa một số tên cột nếu dùng nguồn fallback.
3. Chọn biến mục tiêu `price`.
4. Chọn các đặc trưng phù hợp với đề bài: diện tích, số phòng, vị trí, tiện ích và điều kiện.
5. Xử lý dữ liệu thiếu bằng `SimpleImputer`.
6. Chuẩn hóa biến numeric bằng `StandardScaler`.
7. Mã hóa biến categorical bằng `OneHotEncoder`.
8. Chia train/test theo tỷ lệ 80/20 với `random_state=42`.
9. Huấn luyện mô hình baseline `LinearRegression`.
10. Tinh chỉnh mô hình bằng cách thử `Ridge` với các giá trị alpha: 0.1, 1.0, 10.0 và 100.0.
11. So sánh hai bộ đặc trưng: `basic_features` và `full_features`.
12. Đánh giá bằng MAE, RMSE và R2, chọn mô hình tốt nhất theo RMSE thấp nhất.
13. Lưu model, metrics, bảng so sánh, dự đoán mẫu, hệ số mô hình và biểu đồ.

## Tinh chỉnh mô hình

Trong scikit-learn, `LinearRegression` không có tham số `alpha`. Tham số `alpha` thuộc về các biến thể hồi quy tuyến tính có điều chuẩn như `Ridge` hoặc `Lasso`. Vì đề bài yêu cầu thử alpha theo tinh thần tinh chỉnh mô hình, project dùng:

- `LinearRegression`: mô hình baseline.
- `Ridge(alpha=0.1)`, `Ridge(alpha=1.0)`, `Ridge(alpha=10.0)`, `Ridge(alpha=100.0)`: các mô hình hồi quy tuyến tính có L2 regularization.

Project thử các cấu hình trên với hai bộ đặc trưng:

- `basic_features`: `area`, `Bedroom.AbvGr`, `Full.Bath`, `TotRms.AbvGrd`, `Neighborhood`
- `full_features`: toàn bộ các feature chính trong `main.py`

Bảng so sánh được lưu tại:

```text
results/model_comparison.csv
```

## Cách chạy

```bash
pip install -r requirements.txt
python main.py
```

Trên Windows có thể chạy nhanh:

```bat
run.bat
```

## Ý nghĩa các chỉ số đánh giá

- MAE: sai số tuyệt đối trung bình giữa giá thật và giá dự đoán. MAE càng thấp thì dự đoán càng gần giá thật.
- RMSE: căn bậc hai của sai số bình phương trung bình. RMSE phạt nặng các lỗi dự đoán lớn hơn MAE.
- R2: tỷ lệ phương sai của giá nhà được mô hình giải thích. R2 càng gần 1 thì mô hình giải thích dữ liệu càng tốt.

## File kết quả

Sau khi chạy `python main.py`, project sinh ra các file:

- `data/processed/modeling_data.csv`
- `results/metrics.json`
- `results/model_comparison.csv`
- `results/sample_predictions.csv`
- `results/model_coefficients.csv`
- `results/figures/actual_vs_predicted.png`
- `results/figures/residuals.png`
- `models/linear_regression_model.joblib`
- `models/best_model.joblib`

Nếu nguồn OpenIntro bị chặn khi tải tự động, chương trình sẽ thử nguồn OpenML. Nếu tất cả nguồn tự động đều lỗi, chương trình tạo `DOWNLOAD_DATA.md` để hướng dẫn tải thủ công và đặt file `ames.csv` vào `data/raw/`.
