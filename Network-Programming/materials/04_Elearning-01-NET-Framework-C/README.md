# [Elearning 01] - .NET Framework & C#

- Học phần: **[012012301302] - Lập trình mạng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=21009))
- Mục nguồn: [Moodle, mục 4](https://courses.ut.edu.vn/course/view.php?id=21009&section=4)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Bài nộp | BT1.1 - Làm quen với C# - Giải thuật tìm USCLN | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537583) |
| Bài nộp | BT1.2 - Làm quen với GUI - Ứng dụng tính USCLN có GUI | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537584) |
| Bài nộp | BT1.3 - Xử lý chuỗi | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537585) |
| Bài nộp | BT1.4 - Sử dụng thư viện có sẵn - Chương trình DNS Lookup | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537586) |
| Bài nộp | BT1.5 - GUI cho ứng dụng Chat | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537587) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Nội dung hiển thị trên trang

> Due: Saturday, 21 March 2026, 11:09 AM
> Due: Saturday, 21 March 2026, 11:09 AM
> Due: Saturday, 21 March 2026, 11:14 AM
> Due: Saturday, 21 March 2026, 11:09 AM
> Due: Saturday, 21 March 2026, 11:14 AM

## Chi tiết hoạt động

### BT1.1 - Làm quen với C# - Giải thuật tìm USCLN

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537583)
Thời hạn trên Moodle: **Due: Saturday, 21 March 2026, 11:09 AM**. Cần kiểm tra lại trước khi nộp.

> Tạo một Project dạng Console, viết chượng trình nhập vào hai số nguyên dương a và b, tính USCLN của chúng và xuất ra màn hình.
>
> USCLN(a,b) được tính theo giải thuật sau:
>
> B1. Nếu a == b thì kết luận USCLN là a và kết thúc
>
> B2. Nếu a > b thì thay a bằng a - b và quay lại B1.
>
> B3. Nếu a < b thì thay b bằng b - a và quay lại B1.
>
> * Yêu cầu:
>
> - Đặt tên dự án theo định dạng XXXXX_USCLN, với XXXX được thay bằng mã số SV của người nộp bài 
>
> - Dòng đầu tiên xuất ra màn hình chứa thông tin: MSSV, Họ tên SV, và tên bài tập 
>
> - Nộp hai file:(1)  File ảnh chụp màn hình lúc chạy và (2) file nén toàn bộ source dự án

### BT1.2 - Làm quen với GUI - Ứng dụng tính USCLN có GUI

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537584)
Thời hạn trên Moodle: **Due: Saturday, 21 March 2026, 11:09 AM**. Cần kiểm tra lại trước khi nộp.

> Viết lại chương trình tìm USCLN theo giao diện như sau:
>
> Yêu cầu:
>
> - Nộp ảnh chụp màn hình với a là các chữ số của MSSV, b bất kỳ
>
> - Nộp file nén source code

### BT1.3 - Xử lý chuỗi

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537585)
Thời hạn trên Moodle: **Due: Saturday, 21 March 2026, 11:14 AM**. Cần kiểm tra lại trước khi nộp.

> Viết chương trình nhập một chuỗi từ bàn phím, xuất ra màn hình các thông tin sau:
>
> 1. Chuỗi sau khi đổi tất cả các ký tự trong chuỗi thành chữ hoa
>
> 2. Chèn [MSSV] vào vị trí giữa của chuỗi, với MSSV thay bằng mã số sinh viên thật
>
> 3. Đếm số từ có trong chuỗi (các từ cách nhau bằng khoảng trắng, dấu chấm, dấu phẩy)
>
> 4. Xuất ra màn hình từ dài nhất trong chuỗi
>
> 5. Sắp xếp các từ trong chuỗi theo thứ tự ABC và ghép thành một chuỗi mới
>
> * Yêu cầu:
>
> - Nộp file ảnh chụp màn hình và file nén source code dự án

### BT1.4 - Sử dụng thư viện có sẵn - Chương trình DNS Lookup

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537586)
Thời hạn trên Moodle: **Due: Saturday, 21 March 2026, 11:09 AM**. Cần kiểm tra lại trước khi nộp.

> Xây dựng chương trình MyDNS để trả về thông tin của một tên miền được nhập vào từ bàn phím hoặc truyền từ tham số dòng lệnh. Có nghĩa là khi thực thi chương trình không có tham số thì cho phép nhập tên miền vào để truy vấn. Ngược lại, nếu có tham số dòng lệnh (ví dụ "C:\>MyDNS.exe    google.com") thì truy vấn tên miền đưa vào mà không cần nhập nữa.
>
> Gợi ý: sử dụng lớp có sẵn System.Net.Dns và System.Net.IPHostEntry.
>
> *Yêu cầu:
>
> - Đặt tên dự án là MSSV_MyDns
>
> - Nộp file chụp màn hình thực thi và file dự án
>
> 	
>  MyDNS.rar
> 27 February 2026, 10:55 PM

### BT1.5 - GUI cho ứng dụng Chat

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=537587)
Thời hạn trên Moodle: **Due: Saturday, 21 March 2026, 11:14 AM**. Cần kiểm tra lại trước khi nộp.

> Thực hiện theo nội dung file đính kèm 
>
> 	
>  Bai tap Csharp GUI.docx
> 27 February 2026, 10:55 PM
