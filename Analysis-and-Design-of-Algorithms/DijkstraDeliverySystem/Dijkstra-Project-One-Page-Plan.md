# Dijkstra Delivery System - One-Page Project Plan

## 1. Project Overview
**Topic 10**: Optimizing a Delivery System with Dijkstra’s Algorithm  
**Course**: Analysis and Design of Algorithms (Final Project)  

Mục tiêu: Xây dựng ứng dụng WinUI 3 C++ (Unpackaged) mô phỏng hệ thống giao hàng, tìm đường đi ngắn nhất bằng thuật toán Dijkstra (Greedy strategy) trên đồ thị có trọng số dương, đồng thời đánh giá hiệu năng thực tế so với lý thuyết.

## 2. Scope

**In-scope**:
- Mô hình hóa mạng lưới giao hàng dưới dạng hai cấu trúc: adjacency list và adjacency matrix.
- Implement 2 biến thể Dijkstra:
	- Adjacency list + `std::priority_queue` (min-heap).
	- Adjacency matrix + linear scan.
- Hiển thị đồ thị và highlight đường đi ngắn nhất trên Canvas.
- Đo và phân tích thời gian chạy (Best / Average / Worst case) cho cả 2 biến thể.
- Đối chiếu tính đúng đắn giữa 2 biến thể (cross-check distance/reachability).
- Giao diện WinUI 3 responsive, hỗ trợ animation highlight.

**Out-of-scope**:
- Trọng số âm (không phù hợp với Dijkstra).
- Tối ưu hóa đa xe / đa điểm đến đồng thời.
- Yếu tố logistics thực tế (traffic, thời gian bốc dỡ…).

**Technology Stack**:
- C++20/23 + WinUI 3 Desktop (C++/WinRT) – Unpackaged
- Visualization: Canvas / ItemsRepeater + Polyline

## 3. Input / Output

**Input**: Danh sách nodes + edges có trọng số dương (hỗ trợ nhập UI và file JSON).  
**Output**: 
- Chi phí ngắn nhất
- Lộ trình chi tiết (A → C → D → F)
- Thông báo khi không tồn tại đường đi

## 4. Test Cases Chính

- TC1: Đồ thị nhỏ – kiểm tra kết quả đúng
- TC2: Nhiều đường đi – chọn đường tối ưu
- TC3: Không tồn tại đường đi
- TC4: Source = Target
- TC5: Đồ thị lớn (50–100 nodes) – đo hiệu năng và so sánh Big-O

## 5. Definition of Done (DoD)

- Thuật toán trả kết quả đúng trên tất cả test cases.
- Có đo runtime và bảng so sánh lý thuyết – thực nghiệm.
- Có so sánh độ phức tạp theo cấu trúc đồ thị: list+heap vs matrix+scan.
- Giao diện vẽ đồ thị + highlight đường đi mượt mà.
- Code sạch, tách biệt logic thuật toán và UI.
- Slide thuyết trình đầy đủ 7 mục bắt buộc của đề bài.
- Repo GitHub có commit history rõ ràng.

## 6. Timeline (7 ngày)

- Day 1: Thiết kế Architecture + Test cases
- Day 2–3: Implement core Dijkstra + unit test
- Day 4–5: WinUI 3 UI + graph visualization + animation
- Day 6: Benchmark + phân tích hiệu năng
- Day 7: Slide + rehearsal + final submission

## 7. Rủi ro & Mitigation

- Rủi ro: UI lag khi đồ thị lớn → Giải pháp: giới hạn kích thước demo + batch update
- Rủi ro: Sai predecessor khi trace path → Giải pháp: test case cố định + validation
- Rủi ro: Demo lỗi → Giải pháp: chuẩn bị 2 dataset backup + video demo dự phòng