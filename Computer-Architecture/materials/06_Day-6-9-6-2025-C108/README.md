# Day 6 (9/6/2025) - C108

- Học phần: **[012012500005] - Kiến trúc máy tính - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=8419))
- Mục nguồn: [Moodle, mục 6](https://courses.ut.edu.vn/course/view.php?id=8419&section=6)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Bài nộp | Introduction to Assembly  Bài tập | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=273174) |
| Tệp | C5 Computer Organization and Architecture 10th - William Stallings | [Mở trên Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=273162) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Tệp đã lưu

- [C5 Computer Organization and Architecture 10th - William Stallings.pdf](<C5 Computer Organization and Architecture 10th - William Stallings.pdf>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/resource/view.php?id=273162)

## Nội dung hiển thị trên trang

> Chapter 5
> 5.1 Semiconductor Main Memory
> 5.2 Error Correction
> 5.3 DDR DRAM
>
> --------------------------------------------
>
> EMU
> Introduction to Assembly
> Opened: Monday, 9 June 2025, 12:00 AM
> Due: Monday, 9 June 2025, 11:59 PM
> Hoàn thành

## Chi tiết hoạt động

### Introduction to Assembly

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=273174)
Thời hạn trên Moodle: **Due: Monday, 9 June 2025, 11:59 PM**. Cần kiểm tra lại trước khi nộp.

> ; The easiest way to print out "Hello, World!"
>
> name "hi"
>
> org 100h
>
> jmp start       ; jump over data declaration
>
> msg:    db      "Hello, World!",0Ah, 24h
>
> start:  mov     dx, msg  ; load offset of msg into dx.
>         mov     ah, 09h  ; print function is 9.
>         int     21h      ; do it!
>         
>         mov     ah, 0 
>         int     16h      ; wait for any key....
>         
> ret ; return to operating system.
