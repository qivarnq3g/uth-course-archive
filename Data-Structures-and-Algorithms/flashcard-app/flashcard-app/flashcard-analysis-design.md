# PHẦN 1. PHÂN TÍCH VÀ THIẾT KẾ

## 1. Phân tích vấn đề

Ứng dụng cần quản lý các thẻ từ vựng (flashcard) để hỗ trợ người dùng học và ôn tập từ mới.  
Mỗi thẻ từ vựng có thể gồm: từ tiếng Anh, nghĩa tiếng Việt, ví dụ minh họa và mức độ ghi nhớ.  
Người dùng học bằng cách lần lượt xem từng thẻ, chuyển sang thẻ tiếp theo, quay lại thẻ trước, thêm thẻ mới, xóa thẻ không cần thiết và chỉnh sửa nội dung thẻ hiện có.  

### 1.1. Yêu cầu chức năng

Hệ thống Flashcard App cần hỗ trợ các chức năng chính sau:

#### 1.1.1. Thêm thẻ mới (Add Card)
- **Mô tả:** Tạo một flashcard mới với đầy đủ thông tin (từ tiếng Anh, nghĩa tiếng Việt, ví dụ minh họa, mức độ ghi nhớ từ 0-5)
- **Đầu vào:** Thông tin flashcard từ người dùng
- **Đầu ra:** Thẻ mới được thêm vào danh sách, thường ở cuối danh sách
- **Xử lý:** Tạo node mới, liên kết với danh sách hiện có

#### 1.1.2. Xóa thẻ (Delete Card)
- **Mô tả:** Xóa một flashcard đã học hoặc không còn sử dụng khỏi danh sách
- **Đầu vào:** Con trỏ tới thẻ cần xóa (hoặc từ khóa để tìm thẻ)
- **Đầu ra:** Thẻ được xóa, danh sách được cập nhật, bộ nhớ được giải phóng
- **Xử lý:** Cập nhật con trỏ của các node lân cận, giải phóng node bị xóa

#### 1.1.3. Chuyển thẻ (Navigate Card)
- **Mô tả:** Di chuyển tới thẻ tiếp theo (next) hoặc quay lại thẻ trước (previous) khi đang học
- **Đầu vào:** Con trỏ tới thẻ hiện tại, hướng di chuyển (next/previous)
- **Đầu ra:** Con trỏ tới thẻ mới (hoặc NULL nếu đã ở đầu/cuối)
- **Xử lý:** Sử dụng con trỏ `next` hoặc `prev` để di chuyển

#### 1.1.4. Tìm kiếm thẻ (Search Card)
- **Mô tả:** Tìm các flashcard theo từ khóa (ví dụ theo từ tiếng Anh)
- **Đầu vào:** Từ khóa tìm kiếm (string)
- **Đầu ra:** Con trỏ tới thẻ tìm thấy (hoặc NULL nếu không tìm thấy)
- **Xử lý:** Duyệt danh sách từ đầu đến cuối, so sánh từ khóa với trường `word`

#### 1.1.5. Cập nhật thẻ (Update Card)
- **Mô tả:** Chỉnh sửa nội dung của thẻ (từ, nghĩa, ví dụ, mức độ ghi nhớ)
- **Đầu vào:** Con trỏ tới thẻ cần cập nhật, dữ liệu mới
- **Đầu ra:** Thẻ được cập nhật với nội dung mới
- **Xử lý:** Gán giá trị mới vào các trường của node, giữ nguyên cấu trúc liên kết  

### 1.2. Yêu cầu phi chức năng (gợi ý)

- Chương trình được cài đặt bằng C++ dưới dạng console, giao diện menu đơn giản.  
- Cấu trúc mã nguồn rõ ràng, dễ mở rộng, thuận tiện cho việc đánh giá hiệu năng theo kích thước dữ liệu.  

---

## 2. Thiết kế cấu trúc dữ liệu

Đề bài yêu cầu dùng Danh sách Liên kết Đôi (Doubly Linked List) để lưu trữ các thẻ từ vựng.  
Cấu trúc này cho phép di chuyển hai chiều giữa các thẻ và xóa/chèn thẻ linh hoạt, phù hợp với hành vi "lật thẻ" trong ứng dụng flashcard.  

### 2.1. Lý do chọn danh sách liên kết đôi

- Hỗ trợ con trỏ tới phần tử trước (`prev`) và sau (`next`), rất phù hợp cho thao tác next/previous giữa các thẻ.  
- Khi xóa một thẻ, chỉ cần cập nhật con trỏ của các node lân cận, không phải dồn phần tử như mảng.  
- Thêm thẻ mới ở đầu/cuối danh sách có độ phức tạp tốt, phù hợp với việc thường xuyên thêm thẻ mới.  

### 2.2. Cấu trúc node flashcard

Mỗi node trong danh sách biểu diễn một thẻ từ vựng:

```
struct Flashcard {
    std::string word;     // Từ tiếng Anh
    std::string meaning;  // Nghĩa tiếng Việt
    std::string example;  // Ví dụ câu
    int level;            // Mức độ ghi nhớ (0-5)

    Flashcard* prev;      // Con trỏ tới thẻ trước
    Flashcard* next;      // Con trỏ tới thẻ sau
};
```

Mỗi node `Flashcard` lưu dữ liệu của một thẻ cùng hai con trỏ `prev` và `next` để liên kết với các node lân cận trong danh sách.  

### 2.3. Cấu trúc danh sách liên kết đôi

Để quản lý toàn bộ tập thẻ, ta xây dựng lớp danh sách liên kết đôi như sau:

```
class DoublyLinkedList {
private:
    Flashcard* head;  // Node đầu danh sách
    Flashcard* tail;  // Node cuối danh sách
    int size;         // Số lượng thẻ

public:
    DoublyLinkedList();
    ~DoublyLinkedList();

    void append(const Flashcard& card);   // Thêm thẻ ở cuối
    void remove(Flashcard* node);         // Xóa một thẻ
    Flashcard* find(const std::string& keyword); // Tìm thẻ theo từ khóa
    void update(Flashcard* node, const Flashcard& newData); // Cập nhật thẻ
    Flashcard* getNext(Flashcard* current); // Chuyển sang thẻ tiếp theo
    Flashcard* getPrevious(Flashcard* current); // Quay lại thẻ trước
    // Các hàm hỗ trợ khác...
};
```

### 2.4. Sơ đồ cấu trúc Doubly Linked List

Về mặt trực quan, danh sách gồm chuỗi node `Flashcard` nối với nhau bằng các mũi tên hai chiều từ `head` đến `tail`. Dưới đây là sơ đồ minh họa:

```
    ┌─────────────┐      ┌─────────────┐      ┌─────────────┐      ┌─────────────┐
    │   Flashcard │      │   Flashcard │      │   Flashcard │      │   Flashcard │
    │   Node 1    │◄────►│   Node 2    │◄────►│   Node 3    │◄────►│   Node 4    │
    ├─────────────┤      ├─────────────┤      ├─────────────┤      ├─────────────┤
    │ word: "cat" │      │ word: "dog" │      │ word: "bird"│      │ word: "fish"│
    │ meaning:... │      │ meaning:... │      │ meaning:... │      │ meaning:... │
    │ example:... │      │ example:... │      │ example:... │      │ example:... │
    │ level: 3    │      │ level: 2    │      │ level: 4    │      │ level: 1    │
    │ prev: NULL  │      │ prev: ──────┘      │ prev: ──────┘      │ prev: ──────┘
    │ next: ──────┼─────►│ next: ──────┼─────►│ next: ──────┼─────►│ next: NULL  │
    └─────────────┘      └─────────────┘      └─────────────┘      └─────────────┘
         ▲                                                                    ▲
         │                                                                    │
       head                                                                  tail
```

**Giải thích sơ đồ:**
- Mỗi node chứa dữ liệu flashcard (word, meaning, example, level) và hai con trỏ `prev` và `next`
- Node đầu tiên có `prev = NULL`, node cuối cùng có `next = NULL`
- Con trỏ `head` trỏ tới node đầu danh sách
- Con trỏ `tail` trỏ tới node cuối danh sách
- Các node được nối với nhau theo hai chiều: mỗi node có thể truy cập node trước (qua `prev`) và node sau (qua `next`)  

---

## 3. Thiết kế giải thuật

Phần này mô tả các bước xử lý chính (pseudocode) tương ứng với những chức năng quan trọng của hệ thống.  

### 3.1. Giải thuật thêm thẻ mới (thêm ở cuối)

Mục tiêu: thêm một node flashcard mới vào cuối danh sách.

Pseudocode:

```
Function append(newCard):
    // Tạo node mới chứa dữ liệu flashcard
    newNode = new Flashcard
    newNode.word = newCard.word
    newNode.meaning = newCard.meaning
    newNode.example = newCard.example
    newNode.level = newCard.level
    
    // Xử lý trường hợp danh sách rỗng
    If head == NULL:
        head = newNode
        tail = newNode
        newNode.prev = NULL
        newNode.next = NULL
    Else:
        // Danh sách không rỗng: thêm vào cuối
        newNode.prev = tail
        newNode.next = NULL
        tail.next = newNode
        tail = newNode
    
    size = size + 1
    return newNode
```

**Độ phức tạp:** O(1) vì chỉ cần cập nhật con trỏ `tail` và liên kết node mới, không cần duyệt danh sách.

### 3.2. Giải thuật xóa thẻ hiện tại

Mục tiêu: xóa một node flashcard khỏi danh sách và giải phóng bộ nhớ.

Giả sử `current` trỏ tới thẻ hiện tại muốn xóa.

Pseudocode:

```
Function remove(current):
    If current == NULL:
        return  // Không có gì để xóa
    
    If size == 1:  // Danh sách chỉ có 1 phần tử
        head = NULL
        tail = NULL
        delete current
        size = 0
        return
    
    If current == head:  // Xóa node đầu
        head = current.next
        head.prev = NULL
        delete current
        size = size - 1
        return
    
    If current == tail:  // Xóa node cuối
        tail = current.prev
        tail.next = NULL
        delete current
        size = size - 1
        return
    
    // Xóa node ở giữa
    current.prev.next = current.next
    current.next.prev = current.prev
    delete current
    size = size - 1
```

**Độ phức tạp:** O(1) vì chỉ cần cập nhật con trỏ, không cần duyệt toàn bộ danh sách.  

### 3.3. Giải thuật tìm kiếm thẻ theo từ khóa

Mục tiêu: tìm một flashcard trong danh sách dựa trên từ khóa (thường là từ tiếng Anh).

Ý tưởng: duyệt từ đầu danh sách đến cuối và so sánh trường `word` với từ khóa.

Pseudocode:

```
Function find(keyword):
    current = head
    
    while current != NULL:
        // So sánh từ khóa với trường word (có thể so sánh chính xác hoặc chứa từ khóa)
        if current.word == keyword OR current.word contains keyword:
            return current  // Tìm thấy
        current = current.next
    
    return NULL  // Không tìm thấy
```

**Độ phức tạp:** O(n) trong trường hợp xấu nhất, với n là số lượng thẻ trong danh sách. Phải duyệt từ đầu đến cuối để tìm thẻ phù hợp.

**Lưu ý:** Có thể mở rộng để tìm kiếm theo nhiều tiêu chí khác (nghĩa, ví dụ) hoặc tìm tất cả các thẻ phù hợp thay vì chỉ thẻ đầu tiên.  

### 3.4. Giải thuật cập nhật nội dung thẻ

Mục tiêu: thay đổi nội dung của một thẻ flashcard đã tồn tại (từ, nghĩa, ví dụ, mức độ).

Pseudocode:

```
Function update(current, newData):
    If current == NULL:
        return false  // Không tìm thấy thẻ để cập nhật
    
    // Cập nhật các trường dữ liệu, giữ nguyên liên kết prev và next
    current.word = newData.word
    current.meaning = newData.meaning
    current.example = newData.example
    current.level = newData.level
    
    return true  // Cập nhật thành công
```

**Lưu ý:** 
- Hàm này chỉ cập nhật dữ liệu, không thay đổi cấu trúc danh sách (giữ nguyên `prev` và `next`)
- Độ phức tạp: O(1) vì chỉ cần gán giá trị mới  

### 3.5. Giải thuật chuyển thẻ (next / previous)

Mục tiêu: di chuyển con trỏ hiện tại sang thẻ tiếp theo hoặc quay lại thẻ trước.

#### 3.5.1. Chuyển sang thẻ tiếp theo

Pseudocode:

```
Function getNext(current):
    If current == NULL:
        return NULL
    
    If current.next != NULL:
        return current.next
    Else:
        return NULL  // Đã ở cuối danh sách
```

#### 3.5.2. Quay lại thẻ trước

Pseudocode:

```
Function getPrevious(current):
    If current == NULL:
        return NULL
    
    If current.prev != NULL:
        return current.prev
    Else:
        return NULL  // Đã ở đầu danh sách
```

**Độ phức tạp:** O(1) vì chỉ cần truy cập con trỏ `next` hoặc `prev`.

**Lưu ý:** Cách cài đặt này tận dụng trực tiếp hai con trỏ `prev` và `next` của danh sách liên kết đôi, phù hợp với yêu cầu "chuyển qua/lại giữa các thẻ" của ứng dụng flashcard. Đây là ưu điểm chính của DLL so với Singly Linked List.

---

## 4. Tóm tắt

### 4.1. Tổng kết thiết kế

Tài liệu này đã trình bày:

1. **Phân tích yêu cầu nghiệp vụ:** Xác định 5 chức năng chính của Flashcard App (thêm, xóa, chuyển, tìm, cập nhật thẻ) cùng với mô tả chi tiết từng thao tác.

2. **Thiết kế cấu trúc dữ liệu:** 
   - Cấu trúc node `Flashcard` với các trường dữ liệu và hai con trỏ `prev`/`next`
   - Lớp `DoublyLinkedList` quản lý danh sách với `head`, `tail` và `size`
   - Sơ đồ minh họa cấu trúc DLL với các node được nối hai chiều

3. **Thiết kế giải thuật:** 
   - Pseudocode đầy đủ cho 5 thao tác chính
   - Phân tích độ phức tạp thời gian cho từng thao tác
   - Giải thích cách thức hoạt động của mỗi giải thuật

### 4.2. Độ phức tạp các thao tác

| Thao tác | Độ phức tạp thời gian | Ghi chú |
|----------|----------------------|---------|
| Thêm thẻ (append) | O(1) | Thêm ở cuối, chỉ cần cập nhật con trỏ |
| Xóa thẻ (remove) | O(1) | Cập nhật con trỏ của node lân cận |
| Tìm kiếm (find) | O(n) | Phải duyệt từ đầu đến cuối trong trường hợp xấu nhất |
| Cập nhật (update) | O(1) | Chỉ cần gán giá trị mới |
| Chuyển thẻ (next/prev) | O(1) | Truy cập trực tiếp con trỏ |

### 4.3. Lợi ích của Doubly Linked List

- **Duyệt hai chiều:** Dễ dàng di chuyển tiến/lùi giữa các thẻ, phù hợp với ứng dụng flashcard
- **Xóa hiệu quả:** Xóa node ở bất kỳ vị trí nào đều có độ phức tạp O(1) khi đã có con trỏ
- **Linh hoạt:** Có thể thêm/xóa ở đầu hoặc cuối danh sách một cách dễ dàng
- **Không cần dồn dữ liệu:** Khác với mảng, không cần di chuyển các phần tử khi xóa

---

**Ghi chú:** Tài liệu này là Phần 1 - Phân tích & Thiết kế của báo cáo nhóm. Các phần tiếp theo (cài đặt code, kiểm thử, đánh giá) sẽ được bổ sung sau khi hoàn thành việc phát triển và review code.  

