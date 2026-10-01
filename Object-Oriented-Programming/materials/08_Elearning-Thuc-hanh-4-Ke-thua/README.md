# Elearning - Thực hành 4 - Kế thừa

- Học phần: **[012012200319] - Lập trình hướng đối tượng - 7480201390613** ([trang học phần](https://courses.ut.edu.vn/course/view.php?id=8358))
- Mục nguồn: [Moodle, mục 8](https://courses.ut.edu.vn/course/view.php?id=8358&section=8)
- Ghi nhận: 2026-10-01

## Hoạt động và tài liệu

| Loại | Tên trên Moodle | Nguồn |
|---|---|---|
| Bài kiểm tra | Điểm danh & Trắc nghiệm kiến thức | [Mở trên Moodle](https://courses.ut.edu.vn/mod/quiz/view.php?id=190585) |
| Trang nội dung | Bài tập thực hành 4 | [Mở trên Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=190586) |
| Bài lập trình | Câu a. Cài đặt lớp Bieuthuc  Virtual programming lab | [Mở trên Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190587) |
| Bài nộp | Câu b. Viết chương trình hoàn chỉnh cho phép lần lượt sinh các câu hỏi, hiển thị cấp độ khó và theo dõi số điểm của người chơi | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190589) |
| Bài lập trình | Câu c. Cài đặt lớp BieuthucCong, BieuthucTru, va BieuthucNhan  Virtual programming lab | [Mở trên Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190590) |
| Bài lập trình | Câu d - Xây dựng lớp đề thi có chức năng sinh ngẫu nhiên  Virtual programming lab | [Mở trên Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190591) |
| Bài lập trình | Câu e. Xây dựng các lớp biểu thức phức tạp hơn, ví dụ (a*a + b*b) hay (a#b)*(a#b), để tăng cấp độ khó cho trò chơi  Virtual programming lab | [Mở trên Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190592) |
| Bài lập trình | f. Kết hợp với lớp phân số Fraction (đã tạo từ chương trước)  Virtual programming lab | [Mở trên Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190593) |
| Bài nộp | Nộp dự án hoàn chỉnh | [Mở trên Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190594) |

Chỉ những tệp xuất hiện trong mục **Tệp đã lưu** mới được xác nhận đã tải về.

## Nội dung hiển thị trên trang

> Câu a. Cài đặt lớp Bieuthuc
> Câu c. Cài đặt lớp BieuthucCong, BieuthucTru, va BieuthucNhan
> Câu d - Xây dựng lớp đề thi có chức năng sinh ngẫu nhiên
> Câu e. Xây dựng các lớp biểu thức phức tạp hơn, ví dụ (a*a + b*b) hay (a#b)*(a#b), để tăng cấp độ khó cho trò chơi
> f. Kết hợp với lớp phân số Fraction (đã tạo từ chương trước)

## Chi tiết hoạt động

### Bài tập thực hành 4

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/page/view.php?id=190586)

> Xây dựng chương trình đố vui cho trẻ em với các câu hỏi là các biểu thức số học có dạng a # b = ?, với a, b là các số nguyên, và # là phép toán số học {+, -, *}. Ví dụ: 2 + 5 = ?. Chương trình sẽ sinh ra các biểu thức ngẫu nhiên (ngẫu nhiên về phép toán và giá trị a,b) và người chơi sẽ nhập câu trả lời là giá trị đúng của biểu thức. Cấp độ khó của câu hỏi sẽ được tăng 1 nếu người chơi đúng liên tiếp n câu hỏi (ví dụ n = 3), ngược lại nếu sai một câu thì cấp độ khó sẽ giảm 1. Cấp độ khó của câu hỏi xác định theo qui luật: 
>
> -Level 1: chỉ có phép toán cộng, 1<= a, b <= 10
>
> -Level 2: có phép toán + và - , 1<= a,b <= 10
>
> -Level 3: có phép toán + , - ,và * , 1<= a,b <= 10
>
> -Level 4: có phép toán + và - , 10<= a,b <= 100
>
> -Level 5+: có phép toán + , - và * , 10<= a,b <= 100
>
> a.Cài đặt lớp Bieuthuc với mô tả lớp như sau:
>
> class Bieuthuc
> {
> protected:
>     int a, b; // hai toán hạng
>     char pheptoan; // ký tự đại diện phép toán {+, -, *}
> public:
>     Bieuthuc(int level); // sinh ngẫu nhiên biểu thức theo quy luật
>     friend ostream& operator<<(ostream& out, Bieuthuc bt); // xuất ra theo dạng a # b
>     bool kiemtra(float traloi); //nhận câu trả lời và kiểm tra xem giá trị biểu thức có trùng câu trả lời không
>     float giatri(); //tính giá trị biểu thức
> };
>
>
>
> b.Thiết kế chương trình chính cho phép sinh ngẫu nhiên các câu hỏi, hiển thị cấp độ khó, và số điểm hiện tại của người chơi.
>
> c.Cài đặt lớp BieuthucCong, BieuthucTru, BieuthucNhan kế thừa từ lớp Bieuthuc để chỉ sinh ngẫu nhiên các biểu thức với phép toán +, - , *.
>
> d. Xây dựng lớp đề thi với các thành viên như sau:
>
> class Dethi {
> private:
>     vector<Bieuthuc> ds; //Chứa danh sách các câu hỏi là các biểu thức
> public:
>     Dethi(int n1, int n2, int n3, int level=1); //Sinh danh sách gồm n1 biểu thức chỉ gồm phép cộng,
>                                                                                    // n2 biểu thức chỉ gồm phép trừ, và
>                                                                                   // n3 biểu thức chỉ gồm phép nhân.
>     vector<Bieuthuc> danhsach() { return ds; }
>     void xuat(ostream& output=cout); // xuất danh sách các câu hỏi ra màn hình, các câu hỏi có dạng a # b = ?
>     int danhgia(istream& input=cin); //đọc n1+n2+n3 câu trả lời từ istream (có thể từ cin hoặc file), trả về số câu đúng.
> };
>
> e. Xây dựng các lớp biểu thức phức tạp hơn, ví dụ (a*a + b*b) hay (a#b)*(a#b), để tăng cấp độ khó cho trò chơi.  
>
> f.     Kết hợp với lớp Phân số (class Fraction) đã làm trong buổi thực hành trước để xây dựng lớp BieuthucPhanSo, tăng thêm cấp độ khó cho trò chơi
>
> Yêu cầu: - Nộp file Project
>
>                - Hình chụp màn hình thực thi

### Câu b. Viết chương trình hoàn chỉnh cho phép lần lượt sinh các câu hỏi, hiển thị cấp độ khó và theo dõi số điểm của người chơi

Nguồn: [Moodle](https://courses.ut.edu.vn/mod/assign/view.php?id=190589)

> Thiết kế chương trình chính cho phép sinh ngẫu nhiên các câu hỏi, hiển thị cấp độ khó, và số điểm hiện tại của người chơi

## Tệp đã lưu

- [f. Kết hợp với lớp phân số Fraction (đã tạo từ chương trước).zip](<f. Kết hợp với lớp phân số Fraction (đã tạo từ chương trước).zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190593)
- [Câu e. Xây dựng các lớp biểu thức phức tạp hơn, ví dụ (a_a + b_b) hay (a#b)_(a#b), để tăng cấp độ khó cho trò chơi.zip](<Câu e. Xây dựng các lớp biểu thức phức tạp hơn, ví dụ (a_a + b_b) hay (a%23b)_(a%23b), để tăng cấp độ khó cho trò chơi.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190592)
- [Câu d - Xây dựng lớp đề thi có chức năng sinh ngẫu nhiên.zip](<Câu d - Xây dựng lớp đề thi có chức năng sinh ngẫu nhiên.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190591)
- [Câu c. Cài đặt lớp BieuthucCong, BieuthucTru, va BieuthucNhan.zip](<Câu c. Cài đặt lớp BieuthucCong, BieuthucTru, va BieuthucNhan.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190590)
- [Câu a. Cài đặt lớp Bieuthuc.zip](<Câu a. Cài đặt lớp Bieuthuc.zip>) — [nguồn Moodle](https://courses.ut.edu.vn/mod/vpl/view.php?id=190587)
