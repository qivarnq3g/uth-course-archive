# [Case study] Xây dựng ứng dụng với dịch vụ HTTP & Email

- Học phần: **[012012301302] - Lập trình mạng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=21009))
- Mục nguồn: [Moodle, mục 17](https://courses.ut.edu.vn/course/view.php?id=21009&section=17)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Bài nộp | ứng dụng cảnh báo giá vàng | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537642) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Nội dung hiển thị trên trang

> Xây dựng ứng dụng với chức năng gửi email.
>
> 1. Tìm hiểu giao thức SMTP:
>
> https://en.wikipedia.org/wiki/Simple_Mail_Transfer_Protocol
>
> 2. Tìm hiểu lớp SmtpClient của .NET và vận dụng để tương tác với dịch vụ Gmail
>
> 3. Viết ứng dụng cảnh báo giá vàng qua email như sau:
>
> - Ứng dụng truy xuất nguồn dữ liệu cung cấp giá vàng (tìm các API free) định kỳ (ví dụ 1h/lần)
>
> - Nếu giá vàng vượt ngưỡng Max và Min (do người dùng đặt trước) thì gửi một email báo về một địa chỉ (cũng do người dùng đặt trước)
>
> Lưu ý: Tham khảo ví dụ tuần trước để biết cách truy cập và phân tích file XML/Json trực tuyến
>
> Gợi ý một số nguồn dữ liệu:
>
> https://metalpriceapi.com/gold
>
> Due: Friday, 8 May 2026, 12:00 AM

## Chi tiết hoạt động

### Ứng dụng cảnh báo giá vàng

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537642)
Thời hạn trên Moodle: **Due: Friday, 8 May 2026, 12:00 AM**. Cần kiểm tra lại trước khi nộp.

> Viết ứng dụng cảnh báo giá vàng qua email như sau:
>
>  - Ứng dụng truy xuất nguồn XML/Json cung cấp giá vàng (search "API Gold Price" để tìm nguồn dữ liệu) định kỳ (ví dụ 1h/lần)
>
>  - Nếu giá vàng vượt ngưỡng Max và Min (do người dùng đặt trước) thì gửi một email báo về một địa chỉ (cũng do người dùng đặt trước)
>
> Lưu ý: 
>
> - Tham khảo ví dụ tuần trước để biết cách truy cập và phân tích file XML trực tuyến
>
> - Tham khảo http://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail (và các link tương tự) cho chức năng gửi mail với gmail
