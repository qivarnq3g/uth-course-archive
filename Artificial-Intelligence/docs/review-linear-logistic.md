# Ôn tập Linear, Logistic và Polynomial Regression

> Phần này là tài liệu ôn thêm, không phải “bài số 3”. Bằng chứng về bài nộp được tách riêng ở `assignment-evidence.md`.

Nguồn chính: `AI2027.pdf` trang 291-332; `AI-Ex-2027.pdf` trang 16-17; project House Price có sẵn trong workspace. Các bước Orange được thêm theo phạm vi thực hành hiện tại, vì PDF không bắt buộc Orange cho ba chủ đề này.

## 1. Linear Regression

### Định nghĩa ngắn

Linear Regression dự đoán một target liên tục bằng tổ hợp tuyến tính của các feature. Với một feature:

\[
\hat{y} = \theta_0 + \theta_1x
\]

Với nhiều feature:

\[
\hat{y} = \theta_0 + \sum_{j=1}^{d}\theta_jx_j
\]

Nguồn: `AI2027.pdf`, trang 291-301.

### Điểm cần nhớ

- **Feature** là biến đầu vào, ví dụ diện tích, số phòng, vị trí và tiện ích của căn nhà.
- **Target** là giá trị cần dự đoán; trong bài House Price là giá bán thực tế.
- Mô hình học các hệ số để giảm sai số bình phương trên dữ liệu train.
- Dữ liệu phải được chia train/test; preprocessing phải học trên train để tránh leakage.
- `AI-Ex-2027.pdf`, trang 16-17 yêu cầu đọc/khám phá dữ liệu, xử lý thiếu, chọn feature, chia tập, đánh giá, tinh chỉnh và dự đoán.

**Từ khóa:** feature, target liên tục, hệ số, intercept, least squares, train/test.

### Metric hồi quy

| Metric | Ý nghĩa | Cách đọc |
| --- | --- | --- |
| MAE | Trung bình trị tuyệt đối của sai số | Càng thấp càng tốt; cùng đơn vị với target |
| RMSE | Căn bậc hai của trung bình sai số bình phương | Càng thấp càng tốt; phạt sai số lớn mạnh hơn MAE |
| R² | Tỷ lệ biến thiên của target được mô hình giải thích trên tập đánh giá | Gần 1 thường tốt hơn; phải nói rõ đang đo trên test |

Các định nghĩa này cũng được dùng trong `bai_5_2_house_price_prediction/REPORT.md`. Không kết luận chất lượng chỉ từ R² train.

### Workflow Orange cơ bản

```text
File
→ Select Columns (đặt price vào Target)
→ Data Sampler
→ Linear Regression
→ Predictions
→ Data Table

Linear Regression (Learner)
+ Data / Test Data
→ Test & Score
```

Orange là phần thực hành mở rộng; `AI-Ex-2027.pdf` không đưa workflow Linear Regression cụ thể.

**Câu có thể viết trong bài:** “Linear Regression dự đoán target liên tục bằng một hàm tuyến tính của các feature. Mô hình phải được đánh giá trên dữ liệu test bằng MAE, RMSE và R² để tránh nhầm độ khớp train với khả năng tổng quát hóa.”

**Câu hỏi ôn:** Vì sao không được chọn mô hình tốt nhất bằng cách xem RMSE trên test nhiều lần?

## 2. Logistic Regression với Iris

### Vì sao có chữ “Regression” nhưng dùng cho classification?

Logistic Regression tính một điểm tuyến tính rồi đưa qua hàm logistic:

\[
\sigma(z) = \frac{1}{1 + e^{-z}}
\]

Đầu ra là xác suất trong khoảng 0 đến 1; ngưỡng xác suất được dùng để chọn lớp. Vì vậy tên mô hình có “Regression” nhưng nhiệm vụ là classification. Nguồn: `AI2027.pdf`, trang 322-327.

> `AI2027.pdf`, trang 326 dùng cụm “Constant output” khi so với Linear Regression. Ngữ cảnh các trang 322-325 và tiêu đề “for Classification” cho thấy ý đúng là đầu ra lớp/xác suất, không phải target liên tục.

### Iris và quy trình

- Dataset có 150 mẫu, ba lớp Setosa, Versicolor, Virginica; mỗi lớp 50 mẫu.
- Bốn feature: chiều dài/rộng lá đài và chiều dài/rộng cánh hoa.
- Chia train/test; nên dùng `stratify=y` để giữ tỷ lệ lớp.
- Fit Logistic Regression trên train, dự đoán test.
- `AI-Ex-2027.pdf`, trang 17 yêu cầu Accuracy, Recall, Precision và F1-score.
- Confusion Matrix không được ghi trong mục Logistic trang 17, nhưng là bảng phù hợp để xem cặp lớp bị nhầm và được project yêu cầu bổ sung.

**Từ khóa:** sigmoid/logistic function, probability, decision threshold, classification, multiclass, confusion matrix.

### Metric classification

- **Accuracy:** tỷ lệ dự đoán đúng trên toàn bộ test.
- **Precision:** trong các mẫu được dự đoán là một lớp, có bao nhiêu mẫu đúng.
- **Recall:** trong các mẫu thật của một lớp, mô hình tìm đúng bao nhiêu.
- **F1-score:** trung bình điều hòa của Precision và Recall.
- Với Iris ba lớp, nên ghi rõ dùng macro average nếu muốn mỗi loài có trọng số bằng nhau.

### So sánh Logistic Regression và Classification Tree

| Khía cạnh | Logistic Regression | Classification Tree |
| --- | --- | --- |
| Cách ra quyết định | Xác suất từ hàm logistic | Chuỗi điều kiện `feature <= threshold` |
| Ranh giới | Tuyến tính trong không gian feature gốc nếu không biến đổi feature | Từng vùng theo các phép chia trục |
| Tiền xử lý | Thường nhạy hơn với thang đo/regularization | Không cần StandardScaler |
| Giải thích | Hệ số và xác suất | Có thể đọc trực tiếp nút, nhánh và lá |
| Overfitting | Kiểm soát bằng regularization | Kiểm soát bằng depth, split và leaf size |

Nguồn bài luyện: Logistic Iris ở `AI-Ex-2027.pdf`, trang 17; Tree Iris ở trang 19.

**Câu có thể viết trong bài:** “Logistic Regression là mô hình phân loại vì nó biến điểm tuyến tính thành xác suất lớp bằng hàm logistic; Decision Tree phân loại bằng các điều kiện chia dữ liệu thành các vùng ngày càng thuần khiết.”

**Câu hỏi ôn:** Nếu Accuracy cao nhưng Recall của một lớp thấp, Confusion Matrix sẽ giúp phát hiện điều gì?

## 3. Polynomial Regression

### Degree 2 và degree 3

Polynomial Regression bậc `k` có dạng:

\[
\hat{y} = \theta_0 + \theta_1x + \theta_2x^2 + \cdots + \theta_kx^k
\]

- Degree 2 thêm thành phần \(x^2\), cho phép một độ cong cơ bản.
- Degree 3 thêm \(x^3\), cho phép hình dạng linh hoạt hơn.
- Bài toán có thể được chuyển thành Linear Regression trên các feature đa thức đã tạo.

Nguồn: `AI2027.pdf`, trang 317-321.

### Overfitting và đánh giá

- Degree tăng làm mô hình linh hoạt hơn nhưng có thể bám nhiễu của train.
- So sánh degree 2 và 3 phải dùng cùng cách chia dữ liệu và metric trên test.
- Nếu train error giảm nhưng test error tăng, đó là dấu hiệu overfitting.
- Việc tạo feature đa thức và mọi preprocessing phải fit trong pipeline trên train.

Phần nhấn mạnh overfitting/test là yêu cầu ôn tập của project; PDF chỉ cung cấp công thức và cách ánh xạ sang feature space, không giao riêng bài degree 2/3.

**Từ khóa:** polynomial degree, feature mapping, non-linear response, train error, test error, overfitting.

**Câu có thể viết trong bài:** “Polynomial Regression vẫn tuyến tính theo hệ số nhưng sử dụng các feature như \(x^2\), \(x^3\). Degree cao hơn không mặc nhiên tốt hơn; cần chọn bằng validation và báo cáo kết quả trên test.”

**Câu hỏi ôn:** Dấu hiệu nào cho thấy degree 3 đang overfit so với degree 2?

