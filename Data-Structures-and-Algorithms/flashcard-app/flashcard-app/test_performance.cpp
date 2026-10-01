#include "DoublyLinkedList.h"
#include <iostream>
#include <chrono>
#include <iomanip>

void testPerformance(int size) {
    DoublyLinkedList dll;
    
    std::cout << "\n=== TEST VỚI " << size << " THẺ TỪ VỰNG ===\n";
    
    // Test append
    auto start = std::chrono::high_resolution_clock::now();
    for (int i = 0; i < size; i++) {
        dll.append("word" + std::to_string(i));
    }
    auto end = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::microseconds>(end - start);
    std::cout << "Append " << size << " thẻ: " << duration.count() << " microseconds\n";
    
    // Test prepend
    start = std::chrono::high_resolution_clock::now();
    dll.prepend("first");
    end = std::chrono::high_resolution_clock::now();
    duration = std::chrono::duration_cast<std::chrono::microseconds>(end - start);
    std::cout << "Prepend 1 thẻ: " << duration.count() << " microseconds\n";
    
    // Test findByIndex (tìm ở giữa)
    start = std::chrono::high_resolution_clock::now();
    Node* found = dll.findByIndex(size / 2);
    end = std::chrono::high_resolution_clock::now();
    duration = std::chrono::duration_cast<std::chrono::microseconds>(end - start);
    if (found) {
        std::cout << "findByIndex(" << size / 2 << "): " << duration.count() << " microseconds\n";
    }
    
    // Test deleteNode (xóa ở giữa)
    start = std::chrono::high_resolution_clock::now();
    dll.deleteNode("word" + std::to_string(size / 2));
    end = std::chrono::high_resolution_clock::now();
    duration = std::chrono::duration_cast<std::chrono::microseconds>(end - start);
    std::cout << "deleteNode (giữa): " << duration.count() << " microseconds\n";
    
    // Test traverseForward
    start = std::chrono::high_resolution_clock::now();
    dll.traverseForward();
    end = std::chrono::high_resolution_clock::now();
    duration = std::chrono::duration_cast<std::chrono::microseconds>(end - start);
    std::cout << "traverseForward: " << duration.count() << " microseconds\n";
    
    // Test traverseBackward
    start = std::chrono::high_resolution_clock::now();
    dll.traverseBackward();
    end = std::chrono::high_resolution_clock::now();
    duration = std::chrono::duration_cast<std::chrono::microseconds>(end - start);
    std::cout << "traverseBackward: " << duration.count() << " microseconds\n";
}

int main() {
    std::cout << "=== ĐÁNH GIÁ HIỆU NĂNG DOUBLY LINKED LIST ===\n";
    
    testPerformance(10);
    testPerformance(100);
    testPerformance(1000);
    
    std::cout << "\n=== KẾT THÚC TEST ===\n";
    return 0;
}

