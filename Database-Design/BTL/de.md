Phần 1.
Làm bài 1.7 ERD Hệ thống Thông tin Khách sạn
Các yêu cầu: 1,2,4 và 5.
(Như vậy phần ER và mô hình Quan hệ thì làm trên Drawio còn cài đặt database thì trên PostgreSQL)
Phần 2.
Tìm hiểu Function và Trigger của PostgreSQL để cài đặt các RBTV là các phụ thuộc hàm sau:
1.
R1(A B C) A -> B
2.
R2(A B C D E) AB -> CD
2 quan hệ trên tạo trong database: DB_FinalTest
Ngoài ra mỗi cá nhân còn có 1 bài trắc nghiệm về Phụ thuộc hàm.


1.7 ERD Hệ thống Thông tin Khách sạn
Hệ thống thông tin quản lý khách sạn (Hotel Management Information System) được thiết kế để hỗ trợ các hoạt động
quản lý của khách sạn, bao gồm quản lý thông tin khách hàng, đặt phòng, dịch vụ, nhân viên, và thanh toán. Hệ thống
này giúp tối ưu hóa quy trình quản lý, nâng cao hiệu quả hoạt động và cải thiện trải nghiệm khách hàng.
Khi một khách hàng mới đăng ký lưu trú tại khách sạn, thông tin của họ sẽ được nhập vào hệ thống. Bất kỳ thay đổi
nào về thông tin cá nhân hoặc trạng thái đặt phòng của khách hàng đều được cập nhật thường xuyên. Hệ thống sẽ lưu trữ
thông tin chi tiết về các phòng, bao gồm tình trạng phòng và giá cả. Khi có thay đổi về tình trạng phòng (ví dụ: phòng
đang bảo trì, phòng đã được đặt), hệ thống sẽ cập nhật ngay lập tức. Quá trình đặt phòng sẽ được ghi nhận với thông tin
về khách hàng, phòng, ngày đặt phòng, ngày nhận phòng, và ngày trả phòng. Hệ thống sẽ cập nhật trạng thái đặt phòng
sau mỗi giao dịch. Thông tin về nhân viên và các nhiệm vụ được phân công sẽ được lưu trữ trong hệ thống. Điều này
giúp quản lý hiệu quả các nhiệm vụ hàng ngày và đảm bảo chất lượng dịch vụ. Hệ thống sẽ lưu trữ thông tin về các dịch
vụ mà khách sạn cung cấp. Khi khách hàng sử dụng dịch vụ, thông tin sẽ được ghi nhận và liên kết với hóa đơn. Hệ
thống sẽ tạo hóa đơn cho mỗi đặt phòng và dịch vụ khách hàng sử dụng. Thông tin về hóa đơn sẽ bao gồm tổng số tiền,
ngày lập hóa đơn, và trạng thái thanh toán.
Thông tin khách hàng bao gồm mã khách hàng, họ tên, địa chỉ, email, số điện thoại, và lịch sử lưu trú.
Thông tin phòng bao gồm mã phòng, loại phòng, tình trạng phòng (có sẵn, đang sử dụng, đang bảo trì), giá phòng, và
các tiện nghi đi kèm.
Thông tin đặt phòng bao gồm mã đặt phòng, mã khách hàng, mã phòng, ngày đặt phòng, ngày nhận phòng, ngày trả
phòng, và tình trạng đặt phòng.
Thông tin nhân viên bao gồm mã nhân viên, họ tên, chức vụ, địa chỉ email, số điện thoại, và danh sách các nhiệm vụ
phụ trách.
Thông tin về các dịch vụ cung cấp bởi khách sạn như nhà hàng, spa, giặt ủi, và các dịch vụ phòng. Thông tin bao gồm
mã dịch vụ, tên dịch vụ, mô tả, và giá cả.
Thông tin hóa đơn bao gồm mã hóa đơn, mã đặt phòng, mã khách hàng, mã dịch vụ (nếu có), tổng số tiền, ngày lập
hóa đơn, và trạng thái thanh toán.
Một khách hàng có thể thực hiện nhiều đặt phòng và mỗi đặt phòng liên quan đến một khách hàng duy nhất.
Một phòng có thể được đặt nhiều lần và mỗi đặt phòng liên quan đến một phòng duy nhất.
Một khách hàng có thể sử dụng nhiều dịch vụ khác nhau và mỗi dịch vụ có thể được sử dụng bởi nhiều khách hàng.
Một nhân viên có thể phụ trách nhiều nhiệm vụ và mỗi nhiệm vụ có thể được thực hiện bởi nhiều nhân viên.
Mỗi đặt phòng có thể tạo ra một hoặc nhiều hóa đơn và mỗi hóa đơn liên quan đến một đặt phòng duy nhất.
Yêu cầu:
1. Vẽ sơ đồ ERD:
• Hãy vẽ ERD cho hệ thống thông tin quản lý khách sạn dựa trên yêu cầu đã nêu trên.
• Xác định các thực thể (entities), mối quan hệ (relationships), và thuộc tính (attributes) của từng thực thể.
7/17
UTH Relational Database Design Exercises- 2025 Nguyễn Văn Diêu
2. Mô tả chi tiết:
• Mô tả chi tiết về mỗi thực thể, mối quan hệ và thuộc tính trong ERD.
• Cho biết các quy tắc mối quan hệ, ví dụ: mối quan hệ 1-n, n-n.
3. Xác Định Nhu Cầu Bảo Mật và Quyền Truy Cập:
• Đặt câu hỏi về quyền truy cập thông tin và bảo mật, ví dụ: ai có quyền truy cập danh sách thanh toán?
4. Biến đổi sang mô hình Quan hệ.
5. Chuẩn hóa và cài đặt Database lên một DBMS.