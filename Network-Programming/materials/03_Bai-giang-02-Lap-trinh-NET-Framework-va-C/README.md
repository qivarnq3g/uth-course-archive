# [Bài giảng 02] - Lập trình .NET Framework và C#

- Học phần: **[012012301302] - Lập trình mạng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=21009))
- Mục nguồn: [Moodle, mục 3](https://courses.ut.edu.vn/course/view.php?id=21009&section=3)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Tệp | Slide 02.pdf | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537575) |
| Liên kết | Học C# cơ bản | [Mở trên Moodle](https://courses.ut.edu.vn/mod/url/view.php?id=537576) |
| Liên kết | Xử lý chuỗi với C# | [Mở trên Moodle](https://courses.ut.edu.vn/mod/url/view.php?id=537577) |
| Liên kết | Lập trình giao diện Windows form và xử lý sự kiện | [Mở trên Moodle](https://courses.ut.edu.vn/mod/url/view.php?id=537578) |
| Bài nộp | Bài tập: Xử lý chuỗi với C# - Mã hoá dữ liệu thành dạng chuỗi | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537579) |
| Tệp | Sửa bài tập Mã hoá dữ liệu - EnDeCodeFileList | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537580) |
| Trang nội dung | Hướng dẫn học | [Mở trên Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=537581) |
| Tệp | bài giảng | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537566) |
| Tệp | hướng dẫn | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537636) |
| Liên kết | Hướng dẫn gỡ lỗi (debugging) | [Mở trên Moodle](https://courses.ut.edu.vn/mod/url/view.php?id=537582) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Tệp đã lưu

- [LTM.zip](<../00_Elearning-UTH/LTM.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537566)
- [EnDeCodeFileList.zip](<EnDeCodeFileList.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537580)
- [Lect02.pdf](<Lect02.pdf>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537575)

## Nguồn chưa thể tải

- [hướng dẫn](https://courses.ut.edu.vn/mod/resource/view.php?id=537636) — Hoạt động được đánh dấu khoá trong chỉ mục; liên kết mô tả không cấp tệp tải.

## Nội dung hiển thị trên trang

> - Giới thiệu về .NET FRAMEWORK [Slides]
>
> - Tổng quan về ngôn ngữ lập trình C# [Lect01]
>
> - Các kiểu dữ liệu cơ bản, các cấu trúc điều khiển [Lect02, Lect03, Lect04]
>
> - Lập trình GUI & Thread [Lect05, Lect06]
>
> Due: Saturday, 14 March 2026, 3:57 AM
>
> Tuần này các bạn điểm qua ngôn ngữ lập trình C# và bộ thư viện .NET Framework.
>
> Sinh viên xem các tài liệu dạng [Lect...], tải ở phần tài nguyên (bài giảng) và code thử các ví dụ mẫu trong đó. Ngoài ra, có các video hướng dẫn lập trình C# cơ bản, dành cho các bạn nào chưa được tiếp cận với ngôn ngữ này.
>
> Các bàn tập trong tuần này các bạn làm và nộp theo yêu cầu (nộp file nén dự án và ảnh chụp màn hình thực thi).
>
> Tuần này hỏi đáp theo dạng gửi các câu hỏi trong mục Hỏi đáp và trao đổi của tuần, hoặc các bạn có thể gửi tin nhắn trực tiếp trong hệ thống E-Learning.

## Chi tiết hoạt động

### Bài tập: Xử lý chuỗi với C# - Mã hoá dữ liệu thành dạng chuỗi

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537579)
Thời hạn trên Moodle: **Due: Saturday, 14 March 2026, 3:57 AM**. Cần kiểm tra lại trước khi nộp.

> 1. Viết chương trình C# với các yêu cầu sau: Đọc thông tin số danh sách file và thư mục con (folder) từ một thư mục P cho trước và mã hoá thông tin ở trên theo dạng chuỗi S với cấu trúc sau:
>
> - Ký tự đầu tiên và cuối cùng của chuỗi S lần lượt là $ và #. 
>
> - Nội dung nằm giữa $ và # gọi là các thành phần, cách nhau bởi dấu ; 
>
> - Thành phần đầu tiên là giá trị nguyên chỉ tổng số file và thư mục con có trong P
>
> - Các thành phần tiếp theo là tên của file (hoặc folder), viết theo quy tắc:
>
> <TênFile;1> hoặc <TênFolder;0> (sau tên file là số 1, sau tên folder là số 0)
>
> Ví dụ: 
>
> Folder D:\Data có 2 folder con là Text và Doc; và 1 tập tin là readme.txt, thì sẽ được mã hoá thành chuỗi 
>
> S = "$3;Text;0;Doc;0;readme.txt;1#";
>
> 2. Viết chương trình giải mã chuỗi S, xuất ra màn hình danh sách các file và folder từ một chuỗi mã hoá 
>
> 3. Tổ chức thành một class thư viện tên là EnDeCodeDirInfo với hai method
>
> static public string encode(string path);
>
> static public tring decode(string S);

### Hướng dẫn học

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=537581)

> Tuần này các bạn điểm qua ngôn ngữ lập trình C# và bộ thư viện .NET Framework.
>
> Sinh viên xem các tài liệu dạng [Lect...], tải ở phần tài nguyên (bài giảng) và code thử các ví dụ mẫu trong đó. Ngoài ra, có các video hướng dẫn lập trình C# cơ bản, dành cho các bạn nào chưa được tiếp cận với ngôn ngữ này.
>
> Các bàn tập trong tuần này các bạn làm và nộp theo yêu cầu (nộp file nén dự án và ảnh chụp màn hình thực thi).
>
> Tuần này hỏi đáp theo dạng gửi các câu hỏi trong mục Hỏi đáp và trao đổi của tuần, hoặc các bạn có thể gửi tin nhắn trực tiếp trong hệ thống E-Learning.

## Ghi chú hoạt động

### Học C# cơ bản  URL

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/url/view.php?id=537576)

Nguồn ngoài:
- <https://www.youtube.com/watch?v=lisiwUZJXqQ>

> Click on Học C# cơ bản to open the resource.

### Xử lý chuỗi với C#  URL

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/url/view.php?id=537577)

Nguồn ngoài:
- <https://www.youtube.com/watch?v=-G-yY_EQcL0>

> Click on Xử lý chuỗi với C# to open the resource.

### Lập trình giao diện Windows form và xử lý sự kiện  URL

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/url/view.php?id=537578)

Nguồn ngoài:
- <https://www.youtube.com/watch?v=W6vJ_c9Mt6A>

> Click on Lập trình giao diện Windows form và xử lý sự kiện to open the resource.

### Hướng dẫn gỡ lỗi (debugging)  URL

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/url/view.php?id=537582)

Nguồn ngoài:
- <https://docs.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-debugger?view=vs-2019>

> Click on Hướng dẫn gỡ lỗi (debugging) to open the resource.
