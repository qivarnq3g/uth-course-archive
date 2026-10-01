# Elearning - Thực hành 5 - Đa hình

- Học phần: **[012012200319] - Lập trình hướng đối tượng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=8358))
- Mục nguồn: [Moodle, mục 11](https://courses.ut.edu.vn/course/view.php?id=8358&section=11)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Bài nộp | Bài 1. Minh hoạ đa hình | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190625) |
| Tệp | Chuong 5 - Da hinh  v2 case study | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=190622) |
| Bài nộp | Bài 2. Case study Payroll System | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190626) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Tệp đã lưu

- [Chuong 6 - Da hinh _v2_case study.ppt](<Chuong 6 - Da hinh _v2_case study.ppt>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=190622)

## Nội dung hiển thị trên trang

> Chuong 5 - Da hinh v2 case study

## Chi tiết hoạt động

### Bài 1. Minh hoạ đa hình

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190625)

> a. Hiện thực Lớp Shape, Rectangle, và Triangle như sau, chạy thử và giải thích kết quả xuất ra màn hình.
>
> b. Chỉnh sửa để kết quả xuất ra là diện tích của các hình thuộc lớp con tương ứng. 
>
> #include<iostream>
>
> usingnamespace std;
>
> classShape{
>
> protected:
>
> int width, height;
>
> public:
>
> Shape(int a =0,int b =0){
>
>          width = a;
>
>          height = b;
>
> }
>
> int area(){
>
>          cout <<"Parent class area :"<<endl;
>
> return0;
>
> }
>
> };
>
> classRectangle:publicShape{
>
> public:
>
> Rectangle(int a =0,int b =0):Shape(a, b){}
>
> int area (){
>
>          cout <<"Rectangle class area :"<<endl;
>
> return(width * height);
>
> }
>
> };
>
> classTriangle:publicShape{
>
> public:
>
> Triangle(int a =0,int b =0):Shape(a, b){}
>
> int area (){
>
>          cout <<"Triangle class area :"<<endl;
>
> return(width * height /2);
>
> }
>
> };
>
> // Main function for the program
>
> int main(){
>
> Shape*shape;
>
> Rectangle rec(10,7);
>
> Triangle  tri(10,5);
>
> // store the address of Rectangle
>
>    shape =&rec;
>
> // call rectangle area.
>
>    shape->area();
>
> // store the address of Triangle
>
>    shape =&tri;
>
> // call triangle area.
>
>    shape->area();
>
> return0;
>
> }

### Bài 2. Case study Payroll System

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190626)

> Xem lại slide Case Study của Chương 6, cài đặt các lớp 
>
> SEmployee: Salaried Employee
>
> CEmployee: Commission Employee
>
> SCEmployee: base-salary-plus-commission employees
>
> a. Tạo 10 đối tượng cho mỗi loại Employee ở trên, với tên (First, Last) là chuỗi gồm 2 ký tự sinh ngẫu nhiên, các thuộc tính khác cũng được sinh ngẫu nhiên. In danh sách này ra màn hình.
>
> b. Sắp xếp danh sách trên theo thứ tự giảm dần của thu nhập (giá trị của earnings()), và in danh sách ra màn hình. 
>
> Yêu cầu: Nộp source code + ảnh chụp màn hình thực thi
