# SCRIPT THUYẾT TRÌNH — PHẦN 3 & 4
## Hoàng Việt Quang

---

Cảm ơn phần trình bày rất sâu sắc về cơ sở lý thuyết của bạn [Tên người 1] và phần phân tích bài toán của bạn [Tên người 2].

Tiếp theo, em xin phép trình bày hai phần quan trọng nhất, thể hiện rõ nhất cơ bắp kỹ thuật của nhóm chúng em: Thiết kế thuật toán và Cài đặt chương trình.

---

## SLIDE 1: Ý tưởng thuật toán & Chiến lược tham lam

Ý tưởng cốt lõi của thuật toán Dijkstra gồm ba bước lặp đi lặp lại:

Thứ nhất: tại mỗi bước, chọn đỉnh có khoảng cách nhỏ nhất trong số các đỉnh chưa được xét.

Thứ hai: cập nhật khoảng cách đến các đỉnh kề của nó – kỹ thuật được gọi là cập nhật khoảng cách.

Thứ ba: lặp lại cho đến khi xét hết tất cả các đỉnh.

Chiến lược được áp dụng ở đây là tham lam – Greedy. Nghĩa là thuật toán luôn luôn chọn đỉnh gần nhất tại thời điểm hiện tại. Mặc dù đây chỉ là quyết định cục bộ, nhưng nhờ tính chất đặc biệt của trọng số không âm, quyết định cục bộ này dẫn đến kết quả tối ưu toàn cục – tức đường đi ngắn nhất thực sự.

---

## SLIDE 2: Cấu trúc dữ liệu

Để cài đặt thuật toán, nhóm em sử dụng ba mảng chính:

Mảng distance – hay distances – lưu khoảng cách tạm thời từ đỉnh nguồn đến mỗi đỉnh.

Mảng parent lưu vết đường đi, cho phép truy ngược lại lộ trình từ đích về nguồn.

Và mảng visited để đánh dấu đỉnh đã được cố định khoảng cách.

Về cách lưu đồ thị, nhóm em dùng hai cách biểu diễn: lớp Graph sử dụng Danh sách kề – tiết kiệm bộ nhớ khi đồ thị thưa. Lớp MatrixGraph sử dụng Ma trận kề – cho phép truy cập cạnh với độ phức tạp O(1).

---

## SLIDE 3: Các biến thể cài đặt

Đặc biệt, em đã thiết kế hệ thống chạy song song hai biến thể Dijkstra trên cùng một dữ liệu đầu vào.

Biến thể thứ nhất: Danh sách kề kết hợp heap chọn nhỏ nhất, độ phức tạp O của E log V. Thuật toán sử dụng hàng đợi ưu tiên để luôn chọn đỉnh có khoảng cách nhỏ nhất, rồi cập nhật và đưa đỉnh mới vào heap khi tìm được đường đi tốt hơn.

Biến thể thứ hai: Ma trận kề, độ phức tạp O của V bình phương. Thuật toán quét tuyến tính toàn bộ đỉnh để tìm đỉnh nhỏ nhất, và cập nhật trực tiếp trên ma trận kề mà không cần cấu trúc heap.

Nhóm đã triển khai cả hai để so sánh hiệu năng thực tế.

---

## SLIDE 4: Cách thực hiện

*(Dừng lại — trỏ vào slide)*

Tiếp theo, để thầy cô thấy rõ cách nhóm em cài đặt chi tiết từng bước, em xin trình bày trên slide này.

*(Trỏ vào cột trái)*

Bên trái là biến thể Danh sách kề kết hợp heap chọn nhỏ nhất, độ phức tạp O của E log V.

Bước đầu tiên: khởi tạo khoảng cách của đỉnh bắt đầu bằng 0, đưa đỉnh bắt đầu vào heap.

Trong vòng lặp chính: lấy đỉnh u có khoảng cách nhỏ nhất từ heap ra. Với mỗi đỉnh kề v, nếu khoảng cách của u cộng trọng số cạnh nhỏ hơn khoảng cách hiện tại của v, thì cập nhật khoảng cách của v và đưa v vào heap.

Vòng lặp kết thúc khi heap trống hoặc khi đã đến đỉnh đích.

*(Trỏ vào cột phải)*

Bên phải là biến thể Ma trận kề, độ phức tạp O của V bình phương.

Tương tự, khởi tạo khoảng cách của đỉnh bắt đầu bằng 0, và tất cả visited bằng false – tức chưa có đỉnh nào được xét. Trong vòng lặp: chọn đỉnh u chưa xét có khoảng cách nhỏ nhất bằng cách duyệt tuyến tính, rồi đánh dấu u đã xét – visited bằng true. Với mọi đỉnh v, nếu có cạnh nối và v chưa xét, kiểm tra và cập nhật khoảng cách nếu tìm được đường đi tốt hơn.

Vòng lặp kết thúc khi đã xét hết tất cả các đỉnh.

Tóm lại, biến thể Danh sách kề dùng heap để luôn chọn đỉnh gần nhất một cách nhanh chóng, còn biến thể Ma trận kề không dùng heap mà quét tuần tự từng đỉnh để tìm đỉnh gần nhất – chậm hơn nhưng đổi lại truy cập cạnh rất nhanh nhờ ma trận kề.

*(Dừng lại — mở Visual Studio, file DijkstraSolver.cpp, zoom font lên)*

Để thầy cô thấy rõ hơn code thực tế, em xin mở code ạ.

Em mở file DijkstraSolver.cpp – đây là trái tim của hệ thống.

*(Cuộn đến dòng 24 — hàm RunDijkstraOnce)*

Đây là hàm RunDijkstraOnce – biến thể Danh sách kề kết hợp heap, tương ứng với cột trái trên slide.

*(Chỉ vào dòng 31)*

Em xin chỉ vào dòng quan trọng nhất:

std::priority_queue, QueueEntry, vector QueueEntry, std::greater QueueEntry, minHeap.

Bộ so sánh std::greater – đây là dòng quan trọng nhất. priority_queue trong C++ mặc định là max-heap, tức phần tử lớn nhất được lấy ra trước. Nhưng thuật toán Dijkstra cần lấy đỉnh có khoảng cách nhỏ nhất. std::greater đảo ngược thứ tự này, biến max-heap thành min-heap, nhờ vậy đỉnh gần nhất luôn được xét trước tiên.

*(Di chuyển xuống dòng 41-44 – kỹ thuật bỏ qua phần tử cũ)*

Và đây là kỹ thuật bỏ qua phần tử cũ – em xin chỉ vào đây:

if currentDistance khác distances, static cast size t, currentNode, thì continue – bỏ qua phần tử cũ đã bị cập nhật bởi khoảng cách tốt hơn.

Nhờ dòng này, chúng em không cần xóa phần tử cũ khỏi heap – chỉ cần bỏ qua khi nó được lấy ra vì đã bị thay thế bởi khoảng cách tốt hơn.

*(Chỉ vào dòng 46-49)*

Và đây là điều kiện dừng sớm: nếu đỉnh hiện tại chính là đỉnh đích, thuật toán dừng ngay mà không cần xét thêm đỉnh nào nữa.

*(Cuộn đến dòng 66 — hàm RunDijkstraMatrixOnce)*

Còn đây là hàm RunDijkstraMatrixOnce – biến thể Ma trận kề kết hợp quét tuyến tính, tương ứng với cột phải trên slide.

*(Chỉ vào dòng 71)*

Ở đây, chúng em dùng mảng visited để đánh dấu đỉnh đã được cố định khoảng cách – tương ứng với bước visited bằng false ở khởi tạo trên slide. Mỗi vòng lặp, thay vì dùng heap, chúng em quét tuyến tính toàn bộ V đỉnh để tìm đỉnh nhỏ nhất chưa thăm.

*(Cuộn xuống dòng 212 — vòng đo hiệu năng trong hàm Solve)*

Đây là vòng đo hiệu năng trong hàm Solve – nơi hệ thống chạy song song cả hai biến thể. Tại đây, sau mỗi lần chạy, thuật toán tự động đối chiếu kết quả giữa hai biến thể để đảm bảo tính đúng đắn tuyệt đối.

*(Đóng file code lại)*

Em xin đóng file code lại ạ.

---

## SLIDE 5: Luồng xử lý & Truy vết đường đi

*(Pause nhẹ 1 giây)*

Tóm lại, luồng xử lý của hệ thống gồm ba bước chính:

Bước một: chạy Dijkstra với biến thể Heap.

Bước hai: chạy Dijkstra với biến thể Ma trận kề.

Bước ba: đối chiếu kết quả – nếu hai kết quả không khớp nhau, báo lỗi ngay lập tức.

Để truy vết đường đi thực tế, hệ thống sử dụng mảng parent từ đỉnh đích đi ngược về đỉnh bắt đầu – đây chính là lộ trình tối ưu mà người giao hàng sẽ đi.

---

## SLIDE 6: Đánh giá Thuật toán

Về đánh giá, ưu điểm lớn nhất của Dijkstra là sự đơn giản, dễ cài đặt và cực kỳ hiệu quả trên mạng lưới giao thông thực tế nơi trọng số luôn không âm.

Hạn chế là: thuật toán không áp dụng được nếu đồ thị có cạnh trọng số âm.

Về độ phức tạp: biến thể ma trận kề đạt O của V bình phương. Biến thể dùng hàng đợi ưu tiên – tức heap – đạt O của E log V.

---

## SLIDE 7: Cài đặt chương trình

*(Giọng tự tin hơn)*

Chuyển sang phần Cài đặt chương trình – đây chính là nơi nhóm em biến ý tưởng thành sản phẩm thực tế.

Về công nghệ, toàn bộ hệ thống được xây dựng bằng C++ chuẩn 20 slash 23, kết hợp framework giao diện hiện đại WinUI 3 – tức C++ slash WinRT – và Windows App SDK.

Về kiến trúc, phần mềm được chia thành bốn file chính: Graph – lưu đồ thị dưới dạng danh sách kề. MatrixGraph – quản lý ma trận kề. DijkstraSolver – trái tim hệ thống, chứa cả hai biến thể thuật toán cùng phần đo hiệu năng. Và MainWindow – xử lý giao diện người dùng.

---

## SLIDE 8 & 9: Chức năng chính

*(Pause nhẹ 1 giây sau khi liệt kê)*

Nhờ kiến trúc tách biệt hoàn toàn giữa phần xử lý logic và giao diện, phần mềm cung cấp đầy đủ bốn chức năng chính:

Thứ nhất: Nhập dữ liệu – cho phép người dùng chọn test case với số đỉnh khác nhau.

Thứ hai: Chọn điểm bắt đầu và điểm kết thúc – chọn điểm bắt đầu và điểm kết thúc trực quan ngay trên giao diện.

Thứ ba: Chạy thuật toán – chạy song song hai biến thể Dijkstra rồi tự động đối chiếu kết quả, kèm đo hiệu năng với số lần lặp tùy chỉnh và thanh tiến trình thời gian thực.

Thứ tư: Hiển thị đường đi và chi phí – hiển thị lộ trình tối ưu cùng tổng chi phí, kèm hiệu ứng làm nổi bật đường đi mượt mà ngay trên vùng vẽ.

---

## Kết thúc

*(Pause 1 giây, mỉm cười tự tin, nhìn thẳng khán giả)*

Và đó chính là toàn bộ phần Thiết kế thuật toán cùng Cài đặt chương trình của nhóm em.

Vậy hệ thống này hoạt động thực tế trên các đồ thị phức tạp như thế nào và hiệu năng ra sao? Em xin nhường lời lại cho bạn [Tên người 3] trình bày phần Thử nghiệm và đánh giá chi tiết.

Em xin cảm ơn!
