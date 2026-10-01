# [Bài giảng 07] - Xây dựng ứng dụng với dịch vụ: HTTP và Email

- Học phần: **[012012301302] - Lập trình mạng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=21009))
- Mục nguồn: [Moodle, mục 16](https://courses.ut.edu.vn/course/view.php?id=21009&section=16)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Trang nội dung | Tài liệu về HTTP Protocol | [Mở trên Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=537638) |
| Trang nội dung | Hướng dẫn sử dụng thư viện HTTPClient | [Mở trên Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=537639) |
| Bài kiểm tra | HTTP | [Mở trên Moodle](https://courses.ut.edu.vn/mod/quiz/view.php?id=537640) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Nội dung hiển thị trên trang

> [1]  https://www.jmarshall.com/easy/http/
>
> [2] https://www.tutorialspoint.com/http/index.htm
>
> HTTP Quiz

## Chi tiết hoạt động

### Tài liệu về HTTP Protocol

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=537638)

> [1]  https://www.jmarshall.com/easy/http/
>
> [2] https://www.tutorialspoint.com/http/index.htm

### Hướng dẫn sử dụng thư viện HTTPClient

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=537639)

> HTTP Client
>
>
> using System.Net;
> using System.IO;
> //…..
> //Link den mot http resource bat ky (webpage)
> String link = "http://www.sjc.com.vn/xml/tygiavang.xml”; 
> //Tao object thuc hien Http request
> HttpWebRequest request = WebRequest.CreateHttp(link);
> //Tao object nhan Http response
> HttpWebResponse response = (HttpWebResponse)request.GetResponse();
> //Doc du lieu 
> StreamReader sr = new StreamReader(response.GetResponseStream(),Encoding.UTF8);
> string content = sr.ReadToEnd();
> sr.Close();
> //Xu ly du lieu (o day xu ly du lieu XML chua thong tin gia vang)
> System.Console.WriteLine(content);
>
>
> //Doc them: How to: Parse XML with XmlReader
> https://msdn.microsoft.com/en-us/library/cc189056(v=vs.95).aspx
