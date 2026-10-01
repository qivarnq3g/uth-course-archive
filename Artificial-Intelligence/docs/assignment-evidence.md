# Bằng chứng về đề bài và số lượng bài

## Kết luận ngắn

Không thể xác nhận từ file trong workspace rằng có đúng hai hay ba bài bắt buộc phải nộp. Hai nhánh cây được chọn làm project hiện tại dựa trên ghi chú/bối cảnh người dùng cung cấp. Tài liệu bài tập thực tế chứa nhiều tiểu mục hơn: Regression Tree toy, Esports Classification Tree, Iris Classification Tree và Titanic Classification Tree.

## Ma trận bằng chứng

| Nhận định | Trạng thái | Bằng chứng / giải thích |
| --- | --- | --- |
| Classification Tree dùng Iris | Xác nhận mạnh | `AI-Ex-2027.pdf`, trang 19, mục 7.3 |
| Regression Tree có dataset được chỉ định | Xác nhận mạnh nhưng phạm vi nhỏ | `AI-Ex-2027.pdf`, trang 18, mục 7.1: bốn bộ toy, `max_depth=3` |
| Regression Tree dùng Body Fat | Không được xác nhận | Không có “Body Fat” trong hai PDF và không có dataset tương ứng trong workspace ban đầu |
| Hai loại cây đều thuộc nội dung đã học | Xác nhận mạnh | `AI2027.pdf`, trang 369-380; `121033 Tri tue nhan tao.pdf`, trang 11 |
| Chỉ có hai bài phải nộp | Chưa xác nhận | Không có câu chỉ định số lượng bài nộp trong tài liệu |
| Có bài thứ ba bắt buộc là Linear hoặc Logistic Regression | Chưa xác nhận | Hai chủ đề có bài luyện riêng nhưng không có bằng chứng gắn chúng với đợt nộp hiện tại |
| Python là lựa chọn khả dụng cho Iris Tree | Xác nhận | `AI-Ex-2027.pdf`, trang 19 ghi có thể dùng scikit-learn/Python |
| Orange bắt buộc cho Decision Tree | Không được xác nhận | Trang 3 chỉ là link tài nguyên; Orange/KNIME ở trang 21 thuộc bài SVM email |
| Confusion Matrix được yêu cầu trong mục Iris Tree | Không được xác nhận trong PDF | Mục 7.3 trang 19 chỉ nêu Accuracy, Recall, Precision, F1; project vẫn bổ sung Confusion Matrix theo phạm vi người dùng giao |
| Đầu ra Elearning phải gom vào Word rồi chuyển PDF | Xác nhận ở mức quy định chung | `introduce.txt`; chưa chứng minh đây là yêu cầu riêng của hai bài cây |

## Những bài liên quan xuất hiện trong `AI-Ex-2027.pdf`

- Trang 16-17: House Price Prediction bằng Linear Regression.
- Trang 17: Iris Flower Classification bằng Logistic Regression.
- Trang 18: 7.1 Regression Tree với bốn bộ toy; 7.2 Esports Classification Tree.
- Trang 19: 7.3 Iris Classification Tree; 7.4 Titanic Classification Tree.

Việc có nhiều mục luyện tập trong tài liệu không đồng nghĩa tất cả đều là bài phải nộp. Không có đánh dấu deadline, “submit/nộp”, mã bài Elearning hoặc phạm vi giao bài cụ thể ở các trang này.

## Mức độ tin cậy

- **Cao:** nội dung, dataset và yêu cầu được ghi trực tiếp ở trang đã dẫn.
- **Trung bình:** hai bài cây là phạm vi thực tế hiện tại, vì thông tin này đến từ ghi chú/trao đổi ngoài PDF nhưng phù hợp với nội dung đã học.
- **Thấp/không dùng để kết luận:** Body Fat và giả thuyết “bài thứ ba”, vì chưa có bằng chứng file.

## Quyết định triển khai

- Làm Iris Classification Tree đúng bài 7.3.
- Làm đủ bốn Regression Tree toy đúng bài 7.1.
- Thêm Ames Regression Tree để đáp ứng yêu cầu đánh giá 70/30 và metric; gắn nhãn rõ là phần mở rộng.
- Đưa Linear/Logistic/Polynomial Regression vào tài liệu ôn riêng, không đánh số thành bài nộp.

