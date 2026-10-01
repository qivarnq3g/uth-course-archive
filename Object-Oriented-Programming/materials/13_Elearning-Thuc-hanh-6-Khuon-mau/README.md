# Elearning - Thực hành 6 - Khuôn mẫu []

- Học phần: **[012012200319] - Lập trình hướng đối tượng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=8358))
- Mục nguồn: [Moodle, mục 13](https://courses.ut.edu.vn/course/view.php?id=8358&section=13)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Bài kiểm tra | Trắc nghiệm - Chương 7 | [Mở trên Moodle](https://courses.ut.edu.vn/mod/quiz/view.php?id=190629) |
| Trang nội dung | Một số link tham khảo chủ đề khuôn mẫu và STL | [Mở trên Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=190630) |
| Bài nộp | Bài tập 1 - Làm quen với khuôn mẫu | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190631) |
| Bài nộp | Bài tập 2 - Sử dụng STL | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190632) |
| Bài lập trình | Câu 2.1. Đếm phần tử có phân biệt  Virtual programming lab | [Mở trên Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190633) |
| Bài lập trình | Câu 2.2. Thống kê sự xuất hiện của các phần tử  Virtual programming lab | [Mở trên Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190634) |
| Bài lập trình | Câu 2.3. Liệt kê vị trí xuất hiện của các phần tử  Virtual programming lab | [Mở trên Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190635) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Nội dung hiển thị trên trang

> Power up C++ with the Standard Template Library: Part 1
>
> Power up C++ with the Standard Template Library: Part 2
>
> Tra cứu các lớp STL: vector, map, stack, queue, set, ...
>
> Câu 2.1. Đếm phần tử có phân biệt
>
> Nhập một mảng các số nguyên không âm và in ra số lượng các phần tử khác nhau trong mảng. Lưu ý, quá trình nhập kết thúc khi nhận được một giá trị âm.
>
> Ví dụ:
>
> Input:
>
> 2 3 2 1 4 6 6 4 5 2 -1
> Output:
> 6
>
> Câu 2.2. Thống kê sự xuất hiện của các phần tử
>
> Nhập một mảng các số nguyên không âm và in ra danh sách các phần tử khác nhau trong mảng cùng với số lần xuất hiện của chúng.
>
> Lưu ý:
>
> - Quá trình nhập kết thúc khi nhận được một giá trị âm.
>
> - Kết quả được xuất ra theo thứ tự tăng dần của các phần tử
>
> Ví dụ:
>
> Input:
>
> 2 3 2 1 4 6 6 4 5 2 -1
>
> Output:
>
> 1 1
> 2 3
> 3 1
> 4 2
> 5 1
> 6 2
>
> Câu 2.3. Liệt kê vị trí xuất hiện của các phần tử
>
> Nhập một mảng các số nguyên không âm và in ra danh sách các phần tử khác nhau trong mảng cùng với vị trí xuất hiện của chúng.
>
> Lưu ý:
>
> - Quá trình nhập kết thúc khi nhận được một giá trị âm.
>
> - Kết quả được xuất ra theo thứ tự tăng dần của các phần tử
>
> Ví dụ:
>
> Input:
>
> 2 3 2 1 4 6 6 4 5 2 -1
>
> Output:
>
> 1 3
> 2 0 2 9
> 3 1
> 4 4 7
> 5 8
> 6 5 6

## Chi tiết hoạt động

### Một số link tham khảo chủ đề khuôn mẫu và STL

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=190630)

> Power up C++ with the Standard Template Library: Part 1
>
>
> Power up C++ with the Standard Template Library: Part 2
>
> Tra cứu các lớp STL: vector, map, stack, queue, set, ...

### Bài tập 1 - Làm quen với khuôn mẫu

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190631)

> 1. Cài đặt hàm hoanvi dùng khuôn mẫu. Viết chương trình main() gọi hàm hoanvi với hai đối số là các biến kiểu nguyên, kiểu thực và kiểu ký tự
>
> 2. Cài đặt lớp Point có thành phần toạ độ x, y. Có thể sử dụng Point với x, y là kiểu int (Point<int>), kiểu float (Point<float>) hoặc double (Point<double>). Viết phương phức distance để tính khoảng cách đến một Point khác và viết hàm main() sinh ngẫu nhiên 20 Point<int>, sắp xếp các Point theo thứ tự tăng dần khoảng cách đến gốc toạ độ (là điểm có toạ độ (0,0)).

### Bài tập 2 - Sử dụng STL

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190632)

> Sử dụng kiểu dữ liệu vector và map, giải quyết bài toán sau.
>
> Nhập vào một mảng các số nguyên không âm (kết thúc việc nhập bằng một giá trị âm) và:
>
> Câu 1: Đếm xem có bao nhiêu giá trị nguyên khác nhau trong mảng
>
> Câu 2: In ra màn hình số lần xuất hiện giá trị của các phần tử trong mảng
>
> Câu 3: In ra danh sách các giá trị khác nhau trong mảng, và các vị trí xuất hiện của chúng 
>
> Lưu ý: Với câu 2 và 3 thì các phần tử in ra cần sắp xếp tăng dần, thông tin mỗi phần tử nằm cùng một hàng. Các giá trị trên hàng cách nhau bởi khoảng trắng.
>
> Sinh viên Nộp dự án ở đây và chấm code các câu hỏi theo Virtual Programming Lab bên dưới.

## Tệp đã lưu

- [Câu 2.3. Liệt kê vị trí xuất hiện của các phần tử.zip](<Câu 2.2. Thống kê sự xuất hiện của các phần tử.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190635) — Nội dung tệp trùng byte; dùng chung bản lưu trong học phần.
- [Câu 2.2. Thống kê sự xuất hiện của các phần tử.zip](<Câu 2.2. Thống kê sự xuất hiện của các phần tử.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190634)
- [Câu 2.1. Đếm phần tử có phân biệt.zip](<Câu 2.1. Đếm phần tử có phân biệt.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190633)
