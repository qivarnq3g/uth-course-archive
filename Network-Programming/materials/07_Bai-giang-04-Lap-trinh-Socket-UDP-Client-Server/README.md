# [Bài giảng 04] - Lập trình Socket - UDP Client/Server

- Học phần: **[012012301302] - Lập trình mạng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=21009))
- Mục nguồn: [Moodle, mục 7](https://courses.ut.edu.vn/course/view.php?id=21009&section=7)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Tệp | Lecture 4 - Lap trinh socket - UDP Client - Server | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537598) |
| Bài kiểm tra | Trắc nghiệm củng cố kiến thức UDP Client/Server | [Mở trên Moodle](https://courses.ut.edu.vn/mod/quiz/view.php?id=537599) |
| Bài nộp | Bài tập Viết chương trình SimpleChat UDP | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537600) |
| Bài nộp | Bài tập Cải tiến SimpleChat UDP, cho phép server quản lý các nickname và các nickname có thể trao đổi thông điệp với nhau. | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537601) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Tệp đã lưu

- [Lecture 4 - Lap trinh socket - UDP Client - Server.pdf](<Lecture 4 - Lap trinh socket - UDP Client - Server.pdf>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537598)

## Nội dung hiển thị trên trang

> Nội dung học tuần này:
>
> - Lập trình socket với mô hình UDP [Lect12]
>
> - Dùng UdpClient trong xây dựng ứng dụng mạng [Lect11]
>
> Due: Saturday, 4 April 2026, 4:06 AM
> Due: Saturday, 4 April 2026, 4:06 AM

## Chi tiết hoạt động

### Bài tập Viết chương trình SimpleChat UDP

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537600)
Thời hạn trên Moodle: **Due: Saturday, 4 April 2026, 4:06 AM**. Cần kiểm tra lại trước khi nộp.

> Xây dựng chương trình Server/Client kết nối theo mô hình UDP để truyền nhận thông điệp là chuỗi kí tự.
>
> Yêu cầu: - Client và Server lần lượt gửi và nhận
>
>                - Nếu thông điệp là "EXIT" thì Client và Server đều kết thúc.
>
>                - Nộp source code projects và chụp hình màn hình lúc thực thi chương trình để minh chứng.

### Bài tập Cải tiến SimpleChat UDP, cho phép server quản lý các nickname và các nickname có thể trao đổi thông điệp với nhau.

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537601)
Thời hạn trên Moodle: **Due: Saturday, 4 April 2026, 4:06 AM**. Cần kiểm tra lại trước khi nộp.

> Thiết kế chương trình UDPChat theo mô hình Server/Client UDP trong đó:
>
> -   Server quản lý danh sách các nickname do Client đăng ký; và là thành phần trung chuyển các thông điệp giữa các Client.
>
> -Client cho phép đăng ký nickname với server, gửi thông điệp đến Client có nickname khác.
> -Nếu nickname không tồn tại, thì Server sẽ phản hồi cho Client vừa gửi.
