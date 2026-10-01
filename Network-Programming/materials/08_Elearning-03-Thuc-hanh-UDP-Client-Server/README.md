# [Elearning 03] - Thực hành UDP Client/Server

- Học phần: **[012012301302] - Lập trình mạng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=21009))
- Mục nguồn: [Moodle, mục 8](https://courses.ut.edu.vn/course/view.php?id=21009&section=8)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Tệp | Hướng dẫn | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537636) |
| Tệp | bài giảng | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537566) |
| Tệp | Vi du mau: UDPTimeClientServer | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537602) |
| Bài nộp | BT3.1 - Chạy ví dụ mẫu TimeClientServerUDP và nộp minh chứng ảnh chụp màn hình | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537603) |
| Bài nộp | BT3.2 - Xây dựng hệ thống client/server truy xuất thông tin files và folder của một đường dẫn | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537604) |
| Bài nộp | BT3.3 - Xây dựng dịch vụ mã hoá đơn giản với giao thức UDP | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537605) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Tệp đã lưu

- [LTM.zip](<../00_Elearning-UTH/LTM.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537566)
- [UDPTimeClientServer.zip](<UDPTimeClientServer.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=537602)

## Nguồn chưa thể tải

- [Hướng dẫn](https://courses.ut.edu.vn/mod/resource/view.php?id=537636) — Hoạt động được đánh dấu khoá trong chỉ mục; liên kết mô tả không cấp tệp tải.

## Nội dung hiển thị trên trang

> Hướng dẫn thực hành:
>
> - Sinh viên xem lại bài giảng tuần 6 (Slide và bài giảng Lect11 & Lect12 ở mục tài nguyên)
>
> - Xem ví dụ mẫu và làm bài tập
>
> - Các vấn đề cần trao đổi gửi ở mục Thảo luận của tuần
>
> Due: Friday, 10 April 2026, 11:14 PM
> Due: Friday, 10 April 2026, 11:14 PM
> Due: Friday, 10 April 2026, 11:14 PM

## Chi tiết hoạt động

### BT3.1 - Chạy ví dụ mẫu TimeClientServerUDP và nộp minh chứng ảnh chụp màn hình

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537603)
Thời hạn trên Moodle: **Due: Friday, 10 April 2026, 11:14 PM**. Cần kiểm tra lại trước khi nộp.

### BT3.2 - Xây dựng hệ thống client/server truy xuất thông tin files và folder của một đường dẫn

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537604)
Thời hạn trên Moodle: **Due: Friday, 10 April 2026, 11:14 PM**. Cần kiểm tra lại trước khi nộp.

> Mô tả: Xây dựng chương trình Client/Server theo giao thức UDP
>
> - Server chờ dữ liệu ở cổng 22333
>
> - Client gửi yêu cầu đến Server là một đường dẫn Path (ví dụ Path = "D:\Programs")
>
> - Server kiểm tra đường dẫn có hợp lệ không, nếu hợp lệ thì trả về nội dung là danh sách các files và folders theo đường dẫn đó. Ngược lại báo về  Client là "Đường dẫn không hợp lệ"
>
> - Client nhận thông tin và hiển thị cho người dùng danh sách thư mục và danh sách files (thư mục in chữ hoa và file in chữ thường)
>
> Yêu cầu:
>
> - Nộp ảnh chụp màn hình và file dự án
>
> Gợi ý: Xem lại bài tập EncodeFileList (Source code đính kèm) để biết cách truy xuất thông tin cấu trúc thư mục
>
> 	
>  EnDeCodeFileList.zip
> 27 February 2026, 10:55 PM

### BT3.3 - Xây dựng dịch vụ mã hoá đơn giản với giao thức UDP

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537605)
Thời hạn trên Moodle: **Due: Friday, 10 April 2026, 11:14 PM**. Cần kiểm tra lại trước khi nộp.

> Viết chương trình Client - Server cho mục đích mã hoá một chuỗi ký tự.
>
> Phương pháp mã hoá như sau:
>
> Client chọn chức năng mã hoá/giải mã bằng cách gửi 0/1 đến server.
>
>    - Với chức năng mã hoá, Client gửi tiếp một chuỗi S và một số nguyên bí mật N (N<= 5) đến server.
>
> Server đáp ứng yêu cầu của Client như sau:
>
>    - Nếu nhận 0 (chức năng mã hoá): Server nhận tiếp S và số nguyên N. Kí tự S[i] trong S sẽ được chuyển thành ký tự có mã ASCII là ASCII(S[i])+ N. Ví dụ N=2 thì kí tự 'A' chuyển thành 'C', '0' chuyển thành '2'.
>
>   - Nếu nhận 1 (chức năng giải mã): Server nhận tiếp chuỗi S và số nguyên N. Kí tự S[i] sẽ chuyển thành kí tự có mã ASCII là ASCII(S[i]) - N. Ví dụ N=1 thì kí tự 'C' thành kí tự 'B'.
>
> * Yêu cầu: 
>
> - Sinh viên hiện thực bằng giao thức UDP
>
> - Server phục vụ lần lượt nhiều Client. 
>
> - Tên Solution là MSSV_SimpleEnCodeUDP
>
> - Chụp hình minh chứng và gửi Source Code dự án.
