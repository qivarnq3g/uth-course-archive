# Skill đã khảo sát và lựa chọn

## Phạm vi khảo sát

- `anthropics/skills` tại revision `9d2f1ae187231d8199c64b5b762e1bdf2244733d` (2026-07-01).
- `github/awesome-copilot` tại revision `30472ecf0fe34cc561df958c08501ecc5ca80ea4` (2026-07-10).
- Chỉ thư mục có `SKILL.md` đúng cấu trúc Agent Skill mới được xem là ứng viên.
- Mỗi `SKILL.md` được đọc toàn bộ trước khi cài; scripts, references, assets, dependency, API key, MCP, network, thao tác file và license đều được kiểm tra.

## Skill được chọn

### 1. `exam-ready`

- Repository: `https://github.com/github/awesome-copilot`.
- Đường dẫn nguồn: `skills/exam-ready`.
- Revision: `30472ecf0fe34cc561df958c08501ecc5ca80ea4`.
- License: MIT, Copyright GitHub, Inc.; bản `LICENSE` được giữ trong thư mục cài.
- Vị trí cài: `.agents/skills/exam-ready/`.
- Lý do chọn: buộc phần ôn tập bám sát tài liệu, tách chủ đề không tìm thấy và tạo câu hỏi ôn ngắn.
- Tác vụ sử dụng: `docs/review-linear-logistic.md`.
- Thành phần: chỉ `SKILL.md`; không script, reference, asset hay package.
- Dependency/API/MCP/network: không có. Skill không tự gọi shell, không ghi/xóa file ngoài nội dung được yêu cầu.
- Điều chỉnh khi dùng: prompt đã cung cấp syllabus và yêu cầu tự chủ, nên dùng dạng long-answer mặc định mà không dừng để hỏi loại đề thi.

### 2. `create-readme`

- Repository: `https://github.com/github/awesome-copilot`.
- Đường dẫn nguồn: `skills/create-readme`.
- Revision: `30472ecf0fe34cc561df958c08501ecc5ca80ea4`.
- License: MIT, Copyright GitHub, Inc.; bản `LICENSE` được giữ trong thư mục cài.
- Vị trí cài: `.agents/skills/create-readme/`.
- Lý do chọn: tạo README gọn, có cấu trúc, dễ chạy lại sau khi code/metric đã ổn định.
- Tác vụ sử dụng: `README.md` ở cuối quy trình.
- Thành phần: chỉ `SKILL.md`; không script, reference, asset hay package.
- Dependency/API/MCP: không có. Có các URL README mẫu và hướng dẫn GFM dạng GET công khai; không cần tài khoản, API key, upload hay dịch vụ trả phí.
- Rủi ro đã kiểm tra: tác vụ ghi chỉ là README đúng phạm vi; không có lệnh xóa hoặc thực thi mã.

## Ứng viên bị loại gần nhất

| Ứng viên | Lý do loại |
| --- | --- |
| `anthropics/skills/skills/pdf` | Phù hợp kỹ thuật nhưng `LICENSE.txt` là proprietary và không phù hợp để copy/retain project-local; có nhiều script và dependency hệ thống. Dùng skill PDF tích hợp của Codex thay thế. |
| `anthropics/skills/skills/doc-coauthoring` | Không có license rõ trong folder/root snapshot đã kiểm tra; chứa hướng dẫn Claude-specific và workflow hỏi/đợi nhiều vòng, xung đột yêu cầu tự chủ. |
| `awesome-copilot/skills/pytest-coverage` | Thêm `pytest-cov`, tạo artifact coverage và ép 100% line coverage; vượt yêu cầu test tối thiểu. |
| `awesome-copilot/skills/documentation-writer` | Bắt buộc hỏi làm rõ, đề xuất outline rồi chờ duyệt; không phù hợp quy trình tự thực hiện. |
| `awesome-copilot/skills/create-agentsmd` | Quá tổng quát và dài trong khi workspace cần `AGENTS.md` ngắn, tập trung vào bằng chứng và lệnh chạy. |
| `awesome-copilot/skills/ruff-recursive-fix` | Cần Ruff và cho phép unsafe autofix; tăng dependency và rủi ro sửa rộng. |
| `awesome-copilot/skills/datanalysis-credit-risk` | Domain credit risk quá chuyên biệt, kèm scripts/references lớn, không phù hợp Iris/Ames. |

## Cài đặt và xác minh

Lệnh cài đã dùng helper chính thức của `skill-installer` với `--dest` trỏ vào project và `--ref` ghim commit. Helper đã chép đủ hai thư mục nhưng lỗi `UnicodeEncodeError` khi in thông báo cuối do đường dẫn tiếng Việt. Đây là lỗi console sau khi copy, không phải lỗi nội dung cài.

Xác minh local:

| Skill | `SKILL.md` | `LICENSE` | SHA-256 của `SKILL.md` |
| --- | --- | --- | --- |
| `exam-ready` | Có | Có | `C3551239018434CB3CF080CE505B279EFEACA8211A5C2D513D7C91B1A157E2AD` |
| `create-readme` | Có | Có | `A93F9161BEF324E3A1550FC79D4F01B660A3FB7EADC4455677D9ACE4DA2965AC` |

Frontmatter của cả hai có `name` và `description` hợp lệ. Codex sẽ tự khám phá skill project-local ở lượt làm việc kế tiếp; trong lượt hiện tại, nội dung hai `SKILL.md` đã được đọc đầy đủ và áp dụng trực tiếp.

