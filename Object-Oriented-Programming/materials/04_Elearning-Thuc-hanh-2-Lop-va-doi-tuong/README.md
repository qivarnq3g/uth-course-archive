# Elearning - Thực hành 2- Lớp và đối tượng

- Học phần: **[012012200319] - Lập trình hướng đối tượng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=8358))
- Mục nguồn: [Moodle, mục 4](https://courses.ut.edu.vn/course/view.php?id=8358&section=4)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Bài kiểm tra | Trắc nghiệm kiến thức Chương 2 - Lớp và đối tượng | [Mở trên Moodle](https://courses.ut.edu.vn/mod/quiz/view.php?id=190566) |
| Bài nộp | Bài tập thực hành - Quản lý đề thi trắc nghiệm | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190567) |
| Bài nộp | Xây dựng phần mềm quản lý điểm quá trình và xét điều kiện dự thi cho sinh viên | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190569) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Nội dung hiển thị trên trang

> Due: Friday, 18 April 2025, 8:54 AM
> Due: Friday, 18 April 2025, 8:55 AM

## Chi tiết hoạt động

### Bài tập thực hành - Quản lý đề thi trắc nghiệm

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190567)
Thời hạn trên Moodle: **Due: Friday, 18 April 2025, 8:54 AM**. Cần kiểm tra lại trước khi nộp.

> Xây dựng ứng dụng quản lý đề thi trắc nghiệm
>
> •Đề thi trắc nghiệm bao gồm nhiều câu hỏi trắc nghiệm
>
> •Mỗi câu hỏi trắc nghiệm bao gồm các thành phần:
>
>      -Nội dung câu hỏi
>
>      -Nội dung đáp án A
>
>      -Nội dung đáp án B
>
>      -Đáp án đúng
>
> •Ứng dụng gồm các chức năng sau:
>
>     -Tạo một đề thi trắc nghiệm mới
>
>     -Thêm một câu hỏi mới từ bàn phím vào đề thi hiện hành
>
>     - In danh sách câu hỏi hiện đang có trong đề thi
>
>     - Lưu danh sách câu hỏi vào file *.TTN; tên file được nhập từ bàn phím (ví dụ LTHDT-CN15B.TTN)
>
>     - Đọc nội dung một file  *.TTN vào bộ nhớ
>
>     - Cho thi và tính số câu trả lời đúng của thí sinh 
>
> *Yêu cầu:
>
>      a) Tạo lớp CauhoiTN với các thuộc tính như yêu cầu, các phương thức bao gồm:
>
>            •nhap(): nhập liệu từ bàn phím
>
>            •docfile(ifstream& f): đọc dữ liệu một câu hỏi từ file stream f
>
>            •ghifile(ofstream& f): ghi nội dung câu hỏi vào file stream f
>
>            •kiemtra(): hiện câu hỏi, và cho người dùng nhập câu trả lời, kiểm tra câu trả lời có đúng đáp án không
>
>            •xuat(): xuất câu hỏi ra màn hình (bao gồm đáp án đúng)
>
>    b) Xây dựng thực đơn (Menu) để người dùng có thể lựa chọn chức năng (bao gồm chức năng thoát chương trình)
>
>   c) Viết hàm bool giongnhau(CauhoiTN cau1, CauhoiTN cau2) với lớp CauHoiTN để kiểm tra hai câu hỏi có trùng nhau hay không (dùng từ khoá friend)
>
> d) Thực thi chương trình, nhập dữ liệu ít nhất 5 câu hỏi và chụp các màn hình khi thử nghiệm các chức năng. Nộp file word có các màn hình được chụp.

### Xây dựng phần mềm quản lý điểm quá trình và xét điều kiện dự thi cho sinh viên

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190569)
Thời hạn trên Moodle: **Due: Friday, 18 April 2025, 8:55 AM**. Cần kiểm tra lại trước khi nộp.

> Xây dựng phần mềm quản lý điểm quá trình và xét điều kiện dự thi cho sinh viên. Yêu cầu như sau:
>
> 1.     Thông tin quản lý cho từng sinh viên:
>
> a.     Mã số SV: string
>
> b.     Họ tên: string
>
> c.     Số buổi vắng: int
>
> d.     Điểm thực hành: float (DiemTH)
>
> e.     Điểm giữa kỳ: float (DiemGK)
>
>  
>
> 2.     Cho phép nhập/xuất danh sách từ bàn phím/màn hình và từ file
>
> 3.     Tính điểm quá trình (DiemQT) theo công thức
>
> DiemQT = (10 – số buổi vắng*2)*0.2 +DiemTH*0.3 + DiemGK*0.5
>
> nếu số buổi vắng <=5
>
> ngược lại, DiemQT = 0
>
> 4.     In danh sách sinh viên đủ điều kiện dự thi (có DiemQT>=3)
>
> 5.     In danh sách các sinh viên có họ là “Nguyen”
>
> 6.     (*) In danh sách các sinh viên có cùng tên với một sinh viên khác
