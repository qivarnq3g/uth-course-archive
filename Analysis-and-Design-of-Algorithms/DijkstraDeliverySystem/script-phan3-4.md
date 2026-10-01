# Script Thuyết Trình — Phần 3 & 4
## Thiết Kế Thuật Toán + Cài Đặt Chương Trình
### Hoàng Việt Quang — Đề tài 10: Tối ưu hóa hệ thống giao hàng bằng Dijkstra

> **Thời lượng:** ~3 phút 30 giây
> **Lưu ý:** Mở `DijkstraSolver.cpp` trong Visual Studio khi đến slide "Cách thực hiện"

---

## Slide 1: Ý tưởng thuật toán & Chiến lược tham lam

*(Nhấn mạnh: "tham lam", "cập nhật khoảng cách", "quyết định cục bộ → tối ưu toàn cục")*

Ý tưởng cốt lõi của thuật toán Dijkstra gồm ba bước lặp đi lặp lại:

**Thứ nhất:** tại mỗi bước, chọn đỉnh có khoảng cách nhỏ nhất trong số các đỉnh **chưa được xét**.

**Thứ hai:** cập nhật khoảng cách đến các đỉnh kề của nó – kỹ thuật được gọi là **cập nhật khoảng cách**.

**Thứ ba:** lặp lại cho đến khi xét hết tất cả các đỉnh.

Chiến lược được áp dụng ở đây là **tham lam – Greedy**. Nghĩa là thuật toán luôn luôn chọn đỉnh gần nhất tại thời điểm hiện tại. Mặc dù đây chỉ là quyết định cục bộ, nhưng nhờ tính chất đặc biệt của trọng số không âm, quyết định cục bộ này dẫn đến kết quả **tối ưu toàn cục** – tức đường đi ngắn nhất thực sự.

---

## Slide 2: Cấu trúc dữ liệu

*(Nhấn mạnh: ba mảng chính)*

Để cài đặt thuật toán, nhóm em sử dụng **ba mảng chính**:

- Mảng **distance[]** lưu khoảng cách tạm thời từ đỉnh bắt đầu đến mỗi đỉnh
- Mảng **parent[]** lưu vết đường đi, cho phép truy ngược lại lộ trình từ đỉnh đích về đỉnh bắt đầu
- Mảng **visited[]** để đánh dấu đỉnh đã được cố định khoảng cách

Về cách biểu diễn đồ thị, nhóm em dùng hai cách:

- **Graph:** sử dụng **Danh sách kề** – tiết kiệm bộ nhớ khi đồ thị thưa
- **MatrixGraph:** sử dụng **Ma trận kề** – cho phép truy cập cạnh với độ phức tạp **O(1)**

---

## Slide 3: Các biến thể cài đặt

*(Nhấn mạnh: "O(E log V)", "đối chiếu kết quả")*

Đặc biệt, em đã thiết kế hệ thống chạy song song **hai biến thể** Dijkstra trên cùng một dữ liệu đầu vào:

- **Biến thể 1 – Danh sách kề + heap chọn nhỏ nhất:** sử dụng **hàng đợi ưu tiên** để chọn đỉnh nhỏ nhất với độ phức tạp **O(E log V)**. Khi tìm được đường đi tốt hơn, cập nhật và đưa đỉnh vào heap.
- **Biến thể 2 – Ma trận kề:** quét tuyến tính để chọn đỉnh nhỏ nhất với độ phức tạp **O(V²)**. Cập nhật trực tiếp trên ma trận mà không cần cấu trúc heap.

Nhóm đã triển khai cả hai để so sánh hiệu năng thực tế.

---

## Slide 4: Cách thực hiện *(NÓI THEO SLIDE + MỞ CODE)*

*(Dừng lại — trỏ vào slide)*

Tiếp theo, để thầy cô thấy rõ cách nhóm em cài đặt chi tiết từng bước, em xin trình bày trên slide này.

*(Trỏ vào cột trái)*

Bên trái là biến thể **Danh sách kề kết hợp heap**, độ phức tạp **O(E log V)**.

Bước đầu tiên: khởi tạo khoảng cách của đỉnh bắt đầu bằng 0, đưa đỉnh bắt đầu vào heap.

Trong vòng lặp chính: lấy đỉnh u có khoảng cách nhỏ nhất từ heap ra. Với mỗi đỉnh kề v, nếu khoảng cách của u cộng trọng số cạnh nhỏ hơn khoảng cách hiện tại của v, thì cập nhật khoảng cách của v và đưa v vào heap.

Vòng lặp kết thúc khi heap trống hoặc khi đã đến đỉnh đích.

*(Trỏ vào cột phải)*

Bên phải là biến thể **Ma trận kề**, độ phức tạp **O(V²)**.

Tương tự, khởi tạo khoảng cách của đỉnh bắt đầu bằng 0, và tất cả **visited bằng false** – tức chưa có đỉnh nào được xét. Trong vòng lặp: chọn đỉnh u chưa xét có khoảng cách nhỏ nhất bằng cách duyệt tuyến tính, rồi đánh dấu u đã xét – visited bằng true. Với mọi đỉnh v, nếu có cạnh nối và v chưa xét, kiểm tra và cập nhật khoảng cách nếu tìm được đường đi tốt hơn.

Trong vòng lặp: chọn đỉnh u chưa xét có khoảng cách nhỏ nhất bằng cách duyệt tuyến tính, rồi đánh dấu u đã xét. Với mọi đỉnh v, nếu có cạnh nối và v chưa xét, kiểm tra và cập nhật khoảng cách nếu tìm được đường đi tốt hơn.

Vòng lặp kết thúc khi đã xét hết tất cả các đỉnh.

Tóm lại, biến thể Danh sách kề dùng heap để luôn chọn đỉnh gần nhất một cách nhanh chóng, còn biến thể Ma trận kề không dùng heap mà quét tuần tự từng đỉnh để tìm đỉnh gần nhất – chậm hơn nhưng đổi lại truy cập cạnh rất nhanh nhờ ma trận kề.

*(Dừng lại — mở Visual Studio, file DijkstraSolver.cpp, zoom toàn bộ font lên)*

Để thầy cô thấy rõ hơn code thực tế, em xin mở code ạ.

*(Mở file **DijkstraSolver.cpp**)*

Em mở file **DijkstraSolver.cpp** – đây là trái tim của hệ thống.

*(Cuộn đến dòng 24 — hàm `RunDijkstraOnce`)*

Đây là hàm `RunDijkstraOnce` – biến thể **Danh sách kề + heap**, tương ứng với cột trái trên slide.

*(Chỉ vào dòng 31)*

Em xin chỉ vào dòng quan trọng nhất:

```cpp
std::priority_queue<QueueEntry, std::vector<QueueEntry>, std::greater<QueueEntry>> minHeap;
```

Bộ so sánh `std::greater` – đây là dòng quan trọng nhất. priority_queue trong C++ mặc định là **max-heap**, tức phần tử lớn nhất được lấy ra trước. Nhưng thuật toán Dijkstra cần lấy đỉnh có khoảng cách nhỏ nhất. `std::greater` đảo ngược thứ tự này, biến max-heap thành min-heap, nhờ vậy đỉnh gần nhất luôn được xét trước tiên.

*(Di chuyển xuống dòng 41-44 – kỹ thuật bỏ qua phần tử cũ)*

Và đây là kỹ thuật **bỏ qua phần tử cũ** – em xin chỉ vào đây:

```cpp
if (currentDistance != distances[static_cast<size_t>(currentNode)]) {
    continue;  // bỏ qua phần tử cũ đã bị cập nhật bởi khoảng cách tốt hơn
}
```

Nhờ dòng này, chúng em **không cần xóa phần tử cũ** khỏi heap – chỉ cần bỏ qua khi nó được lấy ra vì đã bị thay thế bởi khoảng cách tốt hơn.

*(Chỉ vào dòng 46-49)*

Và đây là điều kiện dừng sớm: nếu đỉnh hiện tại chính là đỉnh đích, thuật toán dừng ngay mà không cần xét thêm đỉnh nào nữa.

*(Cuộn đến dòng 66 — hàm `RunDijkstraMatrixOnce`)*

Còn đây là hàm `RunDijkstraMatrixOnce` – biến thể **Ma trận kề + quét tuyến tính**, tương ứng với cột phải trên slide.

*(Chỉ vào dòng 71)*

Ở đây, chúng em dùng mảng `visited` để đánh dấu đỉnh đã được cố định khoảng cách – tương ứng với bước visited bằng false ở khởi tạo trên slide. Mỗi vòng lặp, thay vì dùng heap, chúng em **quét tuyến tính** toàn bộ V đỉnh để tìm đỉnh nhỏ nhất chưa thăm.

*(Cuộn xuống dòng 212 — vòng đo hiệu năng trong hàm `Solve`)*

Đây là vòng đo hiệu năng trong hàm `Solve` – nơi hệ thống chạy song song cả hai biến thể. Tại đây, sau mỗi lần chạy, thuật toán tự động đối chiếu kết quả giữa hai biến thể để đảm bảo tính đúng đắn tuyệt đối.

*(Đóng file code lại)*

Em xin đóng file code lại ạ.

---

## Slide 5: Luồng xử lý & Truy vết đường đi

*(Pause nhẹ 1 giây)*

Tóm lại, luồng xử lý của hệ thống gồm ba bước chính:

**Bước 1:** Chạy Dijkstra với biến thể Danh sách kề + heap
**Bước 2:** Chạy Dijkstra với biến thể Ma trận kề
**Bước 3:** Đối chiếu kết quả – nếu hai kết quả không khớp nhau, báo lỗi ngay lập tức

Để truy vết đường đi thực tế, hệ thống sử dụng mảng **parent[]** đi ngược từ đỉnh đích về đỉnh bắt đầu – đây chính là lộ trình tối ưu mà người giao hàng sẽ đi.

---

## Slide 6: Đánh giá Thuật toán

*(Nhấn mạnh hai con số O-lớn)*

Về ưu điểm, thuật toán Dijkstra rất **đơn giản, dễ cài đặt** và cực kỳ hiệu quả trên mạng lưới giao thông thực tế nơi trọng số luôn không âm.

Về hạn chế: thuật toán **không áp dụng được** nếu đồ thị có cạnh trọng số âm.

Về độ phức tạp:

- Biến thể ma trận kề: **O(V²)**
- Biến thể dùng hàng đợi ưu tiên – tức heap: **O(E log V)**

---

## Slide 7: Cài đặt chương trình

*(Giọng tự tin hơn, cao hơn một chút)*

Chuyển sang phần **Cài đặt chương trình** – đây chính là nơi nhóm em biến ý tưởng thành sản phẩm thực tế.

Về **công nghệ**, toàn bộ hệ thống được xây dựng bằng **C++20/23**, kết hợp framework giao diện hiện đại **WinUI 3 (C++/WinRT)** và Windows App SDK.

Về **kiến trúc**, phần mềm được chia thành bốn file chính:

- **Graph:** lưu đồ thị dưới dạng danh sách kề
- **MatrixGraph:** quản lý ma trận kề
- **DijkstraSolver:** trái tim hệ thống, chứa cả hai biến thể thuật toán cùng phần đo hiệu năng
- **MainWindow:** xử lý giao diện người dùng

*(Nhấn mạnh: "WinUI 3", "đối chiếu kết quả", "đo hiệu năng")*

---

## Slide 8 & 9: Chức năng chính

*(Pause nhẹ 1 giây sau khi liệt kê)*

Nhờ kiến trúc tách biệt hoàn toàn giữa phần xử lý logic và giao diện, phần mềm cung cấp đầy đủ **bốn chức năng chính**:

**Thứ nhất – Nhập dữ liệu:** cho phép người dùng chọn test case với số đỉnh khác nhau.

**Thứ hai – Chọn điểm bắt đầu và điểm kết thúc:** chọn điểm bắt đầu và điểm kết thúc trực quan ngay trên giao diện.

**Thứ ba – Chạy thuật toán:** chạy song song hai biến thể Dijkstra rồi tự động đối chiếu kết quả, kèm đo hiệu năng với số lần lặp tùy chỉnh và thanh tiến trình thời gian thực.

**Thứ tư – Hiển thị đường đi và chi phí:** hiển thị lộ trình tối ưu cùng tổng chi phí, kèm **hiệu ứng làm nổi bật đường đi** mượt mà ngay trên vùng vẽ.

---

## Kết thúc

*(Pause 1 giây, mỉm cười tự tin, nhìn thẳng khán giả)*

Và đó chính là toàn bộ phần **Thiết kế thuật toán** cùng **Cài đặt chương trình** của nhóm em.

Vậy hệ thống này hoạt động thực tế trên các đồ thị phức tạp như thế nào và hiệu năng ra sao? Em xin nhường lời lại cho bạn [Tên người 3] trình bày phần **Thử nghiệm và đánh giá** chi tiết.

Em xin cảm ơn!

---

## Ghi chú khi thực hành

### Thứ tự thao tác trong slide "Cách thực hiện"

```
1. [Trỏ slide] → Nói theo nội dung cột trái (Danh sách kề + heap)
2. [Trỏ slide] → Nói theo nội dung cột phải (Ma trận kề)
3. [Dừng lại]  → Mở Visual Studio
4. [Mở DijkstraSolver.cpp]
5. [Cuộn dòng 24]  → Hàm RunDijkstraOnce, chỉ dòng 31 (hàng đợi ưu tiên)
6. [Cuộn dòng 41-44] → Kỹ thuật bỏ qua phần tử cũ
7. [Chỉ dòng 46-49]  → Điều kiện dừng sớm khi đến đỉnh đích
8. [Cuộn dòng 66] → Hàm RunDijkstraMatrixOnce, chỉ dòng 71 (mảng visited[])
9. [Cuộn dòng 212] → Vòng đo hiệu năng trong hàm Solve
10. [Đóng file]
```

### Nhấn mạnh từ khóa (highlight khi nói)

| Từ/cụm | Lý do |
|--------|-------|
| **quyết định cục bộ → tối ưu toàn cục** | Điểm mấu chốt của chiến lược tham lam |
| **O(E log V)** | Độ phức tạp chuẩn |
| **O(V²)** | Độ phức tạp biến thể 2 |
| **hàng đợi ưu tiên** | Tên đúng trên slide |
| **kỹ thuật bỏ qua phần tử cũ** | Giám khảo sẽ hỏi nếu không nói |
| **std::greater<>** | Chứng minh hiểu heap thực sự |
| **WinUI 3** | Công nghệ hiện đại |
| **hiệu ứng làm nổi bật đường đi** | Hiệu ứng trực quan gây ấn tượng |

### Các câu hỏi dự đoán từ giám khảo

| Câu hỏi | Trả lời ngắn gọn |
|---------|-----------------|
| "Tại sao dùng `std::greater`?" | Hàng đợi ưu tiên mặc định là max-heap. `std::greater` đảo thứ tự thành min-heap – luôn lấy đỉnh có khoảng cách nhỏ nhất. |
| "Kỹ thuật bỏ qua phần tử cũ là gì?" | Không xóa phần tử cũ khỏi heap khi tìm đường tốt hơn. Chỉ bỏ qua khi nó được lấy ra vì đã lỗi thời. |
| "Khi nào dùng Danh sách kề + heap, khi nào Ma trận kề + quét?" | Đồ thị thưa (E ≈ V): Danh sách kề + heap nhanh hơn. Đồ thị đặc (E ≈ V²): Ma trận kề + quét cạnh tranh hơn vì không có chi phí heap. |
| "Sao phải đối chiếu kết quả?" | Đảm bảo tính đúng đắn tuyệt đối – cả hai biến thể phải trả về cùng kết quả trên cùng dữ liệu đầu vào. |
| "Tại sao slide ghi O(E log V) mà slide kia ghi O(V²)?" | O(E log V) là độ phức tạp khi dùng heap, tối ưu với đồ thị thưa. O(V²) là độ phức tạp khi dùng ma trận kề, phù hợp với đồ thị đặc. |

---

*Script được đối chiếu kỹ với tất cả 9 slide thực tế và DijkstraSolver.cpp*
