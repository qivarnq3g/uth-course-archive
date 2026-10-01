# Khảo sát workspace ban đầu

Snapshot này được ghi trước khi tạo project mới. Root không phải Git repository.

## Tài liệu

- `AI2027.pdf` - 474 trang, slide bài giảng.
- `AI-Ex-2027.pdf` - 24 trang, tài liệu bài tập.
- `121033 Tri tue nhan tao.pdf` - 21 trang, đề cương/kế hoạch học phần.
- `Aurélien Géron - Hands-On Machine Learning with Scikit-Learn, Keras, and TensorFlow - Concepts, Tools, and Techniques to Build Intelligent Systems, 2nd Edition (2019).pdf`.
- `Stuart J. Russell, Peter Norvig - Artificial Intelligence_ A Modern Approach, Global Edition-Pearson (2021).pdf`.
- `introduce.txt` - quy định nộp bài Elearning.

## Project có sẵn

### `bai_5_2_house_price_prediction/`

- `main.py`, `README.md`, `REPORT.md`, `requirements.txt`, `run.bat`.
- Dataset gốc Ames: `data/raw/ames.csv` (2.930 dòng, 82 cột, target `price`).
- Dataset xử lý, model `.joblib`, JSON/CSV metric và hai hình kết quả.
- Đây là project Linear Regression/Ridge 80/20, không phải Regression Tree. File gốc và output cũ không bị sửa.

### `wumpus-prolog-game-main/`

- README, ảnh, file Prolog biên dịch và PDF hướng dẫn.
- Có bản ZIP tương ứng ở root.
- Không liên quan hai bài cây.

## Loại file không có trong snapshot ban đầu

- Không có `.ows`, `.xlsx`, `.xls`, `.ipynb`.
- Không có dataset/file mang tên Iris hoặc Body Fat.
- Không có `AGENTS.md`, `docs/`, `.agents/skills/`, `.venv` hoặc test ở root.

## Môi trường phát hiện

- Python 3.11.9 tại lệnh `python`; `py` mặc định trỏ Python 3.14 nên lệnh project dùng đường dẫn `.venv` rõ ràng.
- Orange standalone 3.40.0 có tại `C:\Users\Lucian\AppData\Local\Programs\Orange`, nhưng executable không nằm trên `PATH`.
- Poppler không có trên `PATH`; PDF đã được trích text bằng pypdf và render kiểm tra bằng PyMuPDF.

