# [Bài giảng 03] - Lập trình Socket - TCP Client/Server

- Học phần: **[012012301302] - Lập trình mạng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=21009))
- Mục nguồn: [Moodle, mục 5](https://courses.ut.edu.vn/course/view.php?id=21009&section=5)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Tệp | Lecture 3 - Lap trinh socket - TCP Client - Server | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537588) |
| Tệp | Demo TCP Client/Server- SimpleChatTCP Console | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537589) |
| Bài kiểm tra | Trắc nghiệm củng cố kiến thức TCP Client/Server | [Mở trên Moodle](https://courses.ut.edu.vn/mod/quiz/view.php?id=537590) |
| Bài nộp | Bài tập TCP/Client Server - TimeTCP | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537591) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Tệp đã lưu

- [DemoTCPClientServer.zip](<DemoTCPClientServer.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537589)
- [Lecture 3 - Lap trinh socket - TCP Client - Server.pdf](<Lecture 3 - Lap trinh socket - TCP Client - Server.pdf>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537588)

## Nội dung hiển thị trên trang

> - Làm quen với cách sử dụng địa chỉ IP và định danh host [Lect08]
>
> - Làm quen với lớp Socket  [Lect10]
>
> - Viết chương trình TCP Server và TCP Client với Socket [Lect10]
>
> - Làm quen với thiết kế Server/Client [Case study trên lớp]
>
> - Viết nhanh ứng dụng mạng với các lớp TcpListener, TcpClient [Lect09]
>
> Ứng dụng chat đơn giản giữa Client và Server, giao diện console, theo mô tả sau:
>
> - Server chờ kết nối từ cổng 12345
>
> - Nếu kết nối thành công với client thì sẽ gửi chuỗi thông báo phiên bản và chờ Client gửi thông điệp
>
> - Sau khi nhận được thông điệp từ Client thì Server hiển thị và cho phép gửi thông điệp mới đến Client
>
> - Lặp lại cho đến khi Client hoặc Server đóng kết nối
>
> Due: Saturday, 28 March 2026, 1:39 AM

## Chi tiết hoạt động

### Bài tập TCP/Client Server - TimeTCP

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537591)
Thời hạn trên Moodle: **Due: Saturday, 28 March 2026, 1:39 AM**. Cần kiểm tra lại trước khi nộp.

> Viết chương trình date/time client/server TCP
>
> Chương trình client cung cấp các chức năng sau:
>
>      1- Kết nối đến server: nhập địa chỉ server cung cấp dịch vụ và port  trước khi kết nối
>
>      2- Nếu kết nối được thì nhập yêu cầu cần phục vụ:
>
>                         1- Time
>
>                         2- Date
>
>                         3- Date & Time
>
>            - Nhận trả lời của server và in ra màn hình
>
>       3- Ngắt kết nối đến server
>
> Yêu cầu: 
>
> - Tạo Project TimeTCPServer với Solution name là MSSV_TimeTCP trong Visual Studio .NET
>
> - Thêm Project  TimeTCPClient trong Solution MSSV_TimeTCP
>
> - Viết code cho các dự án tạo ra
