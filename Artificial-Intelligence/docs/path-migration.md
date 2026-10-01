# Biên bản migration đường dẫn workspace

## 1. Phạm vi và trạng thái

- Thời điểm hậu kiểm: `2026-07-10T17:12:23+07:00` (Asia/Bangkok).
- Hình thức migration: script đã đổi tên trực tiếp thành công trước phiên hậu kiểm; không chạy lại rename và không dùng fallback.
- OLD_ROOT (lịch sử): `D:\courses\Trí tuệ nhân tạo`.
- NEW_ROOT (đang thực thi): `D:\courses\Artificial-Intelligence`.
- OLD_ROOT không còn tồn tại; NEW_ROOT tồn tại và là working directory đã dùng cho mọi lệnh bên dưới.
- Manifest: `D:\courses\ai-workspace-before-migration.json`, schema `1`, tạo lúc `2026-07-10T16:40:51.5767950+07:00`.

## 2. Manifest, số file và checksum

Manifest ghi nhận `83` file, tổng `116,374,268` byte, không tính `.venv`, cache và thư mục tạm. Trước khi sửa hậu migration, cả `83/83` file đều tồn tại, đúng size và khớp SHA-256; không có file thiếu hoặc sai checksum.

Sau hậu migration có `84` file trong cùng phạm vi đếm: `73` file của manifest giữ nguyên checksum, `10` file manifest thay đổi có chủ đích và `docs/path-migration.md` là file mới. `.venv` và `.venv-old` không nằm trong số đếm.

Các nguồn quan trọng vẫn khớp manifest:

| File | SHA-256 |
| --- | --- |
| `AI-Ex-2027.pdf` | `9943B268D238F30B302964B80515FE477A37C7A52F9D569F5F5CF95FBEBBBDA5` |
| `AI2027.pdf` | `5B36DC80E16EB0B2B3EAA9DE17FBAA387A31F991F3D4559C03D9C369DF32F701` |
| `121033 Tri tue nhan tao.pdf` | `2A3DBE94FC129568001152574F8B6A28B7E0B831C786F9FBF44BB0C40D5D3AAC` |
| `bai_5_2_house_price_prediction/data/raw/ames.csv` | `CCE822E6D71812F6EDCF9D34BF42A2D65B927702C99AC4E6BCF20E5759D7BE11` |

Hai sách PDF lớn, toàn bộ code Python, test, `requirements.txt`, `orange/test_workflows.ps1`, các CSV Orange, output JSON/CSV/hình không thuộc danh sách sửa cũng tiếp tục khớp manifest. Không PDF hoặc dataset gốc nào bị sửa.

## 3. File thay đổi

| File | Lý do | SHA-256 sau migration |
| --- | --- | --- |
| `AGENTS.md` | Ghi NEW_ROOT và chặn dùng OLD_ROOT/`.venv-old` để thực thi | `CDBA503D500FBC7DED944F85837238E0F6FBBC0FD792AF0CB88B9B78E0F1EADC` |
| `README.md` | Ghi trạng thái workspace, test và Orange hậu migration | `CECF94D4E759E6631DA01FF4D567FFEE369E9822B393947A5D8595D25C356C66` |
| `docs/project-context.md` | Đổi workspace hoạt động sang NEW_ROOT; giữ OLD_ROOT như lịch sử | `755AB06D11DD5C811655798F4AF041C8E9843A8683509264AE0EE338A3B8CE50` |
| `docs/orange-workflows.md` | Ghi cache `.ows`, CLI và caveat Unicode hậu migration | `4AEC99C6B463D923D7602A7B4E983D7D8F296C6290110697080209560DC28F3D` |
| `docs/bao-cao.md` | Bổ sung trạng thái Orange/path, không đổi kết luận học thuật | `2BDCE201C1525618E245FC8ACE6DF705BE2ABA159E737413CD349EFFBBFE49EB` |
| `orange/classification-tree.ows` | Tái tạo cache `RecentPath.abspath` ở NEW_ROOT | `6CB04A3280D9EF85C01E3296D1381F9F8B6610D2B8C8DA4376FF1113BBD43187` |
| `orange/regression-tree.ows` | Tái tạo cache `RecentPath.abspath` ở NEW_ROOT | `7F2CCCCAB6AB6992F3750A60A104EDF034B9835585AD8EC30C01249A2AFFDDA8` |
| `outputs/regression-tree/ames-extension/metrics.json` | Chỉ đổi trường audit `data_path` sang NEW_ROOT | `10C29B929D0C6EEA427BE52C6CD3D8DA5EB13495C9C6195731A1E244C779D7BE` |
| `outputs/orange-cli-check/classification-tree.stderr.log` | Log CLI mới tại NEW_ROOT | `06B654806A0EC922FBA59147664D6F1A377C50624048B40B049CBDD08CA8363E` |
| `outputs/orange-cli-check/regression-tree.stderr.log` | Log CLI mới tại NEW_ROOT | `CFE33D40F33FC5A10581FC6513EDAA9552300AED1E2E1915355CD7B2D827C4B5` |
| `docs/path-migration.md` | Biên bản mới | File mới, không có checksum trước migration |

Hai file stdout Orange vẫn rỗng và giữ checksum cũ. `orange/data/iris.csv` giữ SHA-256 `B150FE324A4536F25CA034B8FA187037CBABFBB3021B3EF7FFB89E62BD18F666`; `orange/data/ames_regression.csv` giữ `BAE3AE2065D03872AD47AC6D856A3475BEA83C17287F4D65226181920A2FD789`.

## 4. Đường dẫn và workflow Orange

- Quét các file text được chỉ định không còn OLD_ROOT trong code, script, JSON hoạt động hoặc cấu hình.
- Những lần xuất hiện OLD_ROOT còn lại đều được gắn nhãn lịch sử trong `AGENTS.md`, `README.md` và `docs/*.md`.
- Không phát hiện path mojibake hoặc dạng dấu hỏi trong file cấu hình hoạt động.
- Giải mã an toàn phần pickle của `RecentPath` xác nhận hai `.ows` dùng `D:/courses/artificial-intelligence/orange/data/...`, `prefix="basedir"` và relative path `data/...`.
- Tái tạo bằng Orange `3.40.0` giữ nguyên node ID, qualified widget, vị trí, link/signal, topology, Data Sampler, Tree và Test & Score setting.
- `orange/test_workflows.ps1` giữ nguyên checksum manifest; working directory lấy động từ vị trí script, đối số `.ows` là relative ASCII, timeout mặc định `180` giây, timeout trả `124`, log ghi vào `outputs/orange-cli-check/`.

## 5. Môi trường Python và kiểm thử

`.venv` được tạo mới tại NEW_ROOT bằng Python `3.11.9`; pip được nâng lên `26.1.2`, sau đó cài `requirements.txt`. Phiên bản chính: matplotlib `3.11.0`, numpy `2.4.6`, pandas `3.0.3`, scikit-learn `1.9.0`, pytest `9.1.1`. `pip check` trả `No broken requirements found`.

`.venv-old` không được dùng để chạy project và hiện vẫn được giữ làm bản lưu an toàn; có thể xóa thủ công sau khi người dùng hoàn tất kiểm tra GUI/chụp ảnh.

Kết quả lệnh chuẩn:

| Kiểm tra | Kết quả |
| --- | --- |
| `python -m compileall classification_tree_iris regression_tree tests` | Exit `0` |
| `python -m pytest` | `8 passed in 30.69s` |
| Classification Orange CLI | Exit `0` |
| Regression Orange CLI | Exit `0` |

Hai log Orange mới không có `Traceback`, `OSError`, `Invalid argument`, timeout hoặc widget Error. Log xác nhận Classification truyền Tree Model, Test & Score Results, Predictions, Data Table, Tree Viewer và Confusion Matrix; Regression truyền Tree Model, Test & Score Results, Predictions, Data Table và Tree Viewer.

## 6. Đối chiếu metric và output

Không chạy lại GridSearch Ames. Việc đối chiếu trực tiếp JSON xác nhận metric không đổi:

- Iris: `105/45`, CV macro-F1 `0.952332`, Accuracy `0.888889`, Precision macro `0.889881`, Recall macro `0.888889`, F1 macro `0.888765`, Confusion Matrix `[[15,0,0],[0,12,3],[0,2,13]]`.
- Ames mở rộng: `2051/879`, depth `10`, split `2`, leaf `5`, CV RMSE `34,949.66 USD`, test MAE `22,691.65 USD`, test RMSE `34,913.24 USD`, test R² `0.826596`.
- Bốn toy Regression Tree vẫn là dữ liệu luyện cực nhỏ với `max_depth=3`; kết quả khớp train không chứng minh tổng quát hóa.

## 7. Vấn đề phát sinh và việc còn lại

- Lượt cài dependency đầu tiên chạm giới hạn thời gian terminal; chạy tiếp từ cache đã hoàn tất và `pip check` sạch.
- Orange khi tái tạo `.ows` có warning nội bộ về font/offscreen và dùng pickle fallback cho `RecentPath/Context`; process vẫn exit `0`, registry round-trip pass và cấu trúc trước/sau giống nhau.
- Không tự động xác minh GUI trong bước hậu migration này. Cần mở thủ công hai file dưới đây để kiểm tra bố cục, chụp ảnh và nhấn `Ctrl+S`:
  - `D:\courses\Artificial-Intelligence\orange\classification-tree.ows`
  - `D:\courses\Artificial-Intelligence\orange\regression-tree.ows`
- Sau đó chèn nội dung/hình vào Word, xuất PDF và mở lại kiểm tra trước khi nộp.

Mở lại Codex tại NEW_ROOT:

```powershell
Set-Location -LiteralPath 'D:\courses\Artificial-Intelligence'
codex
```
