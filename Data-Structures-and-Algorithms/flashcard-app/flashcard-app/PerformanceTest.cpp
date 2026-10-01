#include "DoublyLinkedList.h"
#include <chrono>
#include <fstream>
#include <iostream>
#include <vector>
#include <iomanip>

struct TestResult {
    int size;
    double insertTime;  // ms
    double findTime;    // μs
    double deleteTime;  // μs
};

void WarmUp() {
    DoublyLinkedList warmupList;
    for (int i = 0; i < 100; i++) {
        warmupList.append("Warmup " + std::to_string(i));
    }
}

TestResult TestPerformance(int cardCount) {
    TestResult result;
    result.size = cardCount;
    
    DoublyLinkedList list;
    
    auto start = std::chrono::high_resolution_clock::now();
    for (int i = 0; i < cardCount; i++) {
        std::string card = "Question " + std::to_string(i) + " / Answer " + std::to_string(i);
        list.append(card);
    }
    auto end = std::chrono::high_resolution_clock::now();
    result.insertTime = std::chrono::duration<double, std::milli>(end - start).count();
    
    start = std::chrono::high_resolution_clock::now();
    auto midCard = list.findByIndex(cardCount / 2);
    end = std::chrono::high_resolution_clock::now();
    result.findTime = std::chrono::duration<double, std::micro>(end - start).count();
    
    Node* firstCard = list.findByIndex(0);
    if (firstCard) {
        start = std::chrono::high_resolution_clock::now();
        list.deleteNode(firstCard->data);
        end = std::chrono::high_resolution_clock::now();
        result.deleteTime = std::chrono::duration<double, std::micro>(end - start).count();
    } else {
        result.deleteTime = 0.0;
    }
    
    return result;
}

void DisplayResults(const std::vector<TestResult>& results) {
    std::cout << "\n";
    std::cout << "════════════════════════════════════════════════════════════\n";
    std::cout << "  📊 PERFORMANCE BENCHMARK RESULTS - DOUBLY LINKED LIST\n";
    std::cout << "════════════════════════════════════════════════════════════\n";
    std::cout << "\n";
    
    std::cout << std::left << std::setw(12) << "Size" 
              << std::setw(20) << "Insert (ms)" 
              << std::setw(20) << "Find (μs)" 
              << std::setw(20) << "Delete (μs)" 
              << std::endl;
    std::cout << std::string(72, '-') << std::endl;
    
    for (const auto& r : results) {
        std::cout << std::left << std::setw(12) << r.size
                  << std::fixed << std::setprecision(4)
                  << std::setw(20) << r.insertTime
                  << std::setw(20) << r.findTime
                  << std::setw(20) << r.deleteTime
                  << std::endl;
    }
    
    std::cout << "\n";
    std::cout << "════════════════════════════════════════════════════════════\n";
    std::cout << "\n";
    
    for (const auto& r : results) {
        std::cout << "\n=== Test với " << r.size << " thẻ ===" << std::endl;
        std::cout << "Thời gian thêm: " << std::fixed << std::setprecision(4) << r.insertTime << " ms" << std::endl;
        std::cout << "Thời gian tìm:  " << r.findTime << " μs" << std::endl;
        std::cout << "Thời gian xóa:  " << r.deleteTime << " μs" << std::endl;
    }
}

void ExportToCSV(const std::vector<TestResult>& results) {
    std::ofstream file("performance_results.csv");
    if (!file.is_open()) {
        std::cerr << "Lỗi: Không thể tạo file CSV!" << std::endl;
        return;
    }
    
    file << "Size,Insert(ms),Find(μs),Delete(μs)\n";
    
    for (const auto& r : results) {
        file << r.size << ","
             << std::fixed << std::setprecision(4)
             << r.insertTime << ","
             << r.findTime << ","
             << r.deleteTime << "\n";
    }
    
    file.close();
    std::cout << "✓ Đã xuất kết quả ra file: performance_results.csv\n" << std::endl;
}

int main() {
    std::cout << "════════════════════════════════════════════════════════════\n";
    std::cout << "  ⚡ PERFORMANCE TEST - DOUBLY LINKED LIST\n";
    std::cout << "════════════════════════════════════════════════════════════\n";
    std::cout << "\n";
    std::cout << "Chuẩn test: 10, 100, 1,000, 10,000 nodes (theo Microsoft best practices)\n";
    std::cout << "Metrics: Insert (ms), Find (μs), Delete (μs)\n";
    std::cout << "\n";
    std::cout << "Đang warm-up cache...\n";
    
    WarmUp();
    
    std::cout << "Warm-up hoàn tất. Bắt đầu test...\n";
    std::cout << "\n";
    
    std::vector<int> testSizes = {10, 100, 1000, 10000};
    std::vector<TestResult> results;
    
    for (int size : testSizes) {
        std::cout << "Đang test với " << size << " nodes...\n";
        TestResult result = TestPerformance(size);
        results.push_back(result);
    }
    
    DisplayResults(results);
    
    ExportToCSV(results);
    
    std::cout << "\n";
    std::cout << "✓ Test hoàn tất!\n";
    std::cout << "\n";
    std::cout << "📝 Lưu ý:\n";
    std::cout << "  - Insert: O(1) - Thời gian hằng số\n";
    std::cout << "  - Find:   O(n) - Thời gian tăng tuyến tính\n";
    std::cout << "  - Delete: O(1) - Thời gian hằng số (khi biết node)\n";
    std::cout << "\n";
    
    return 0;
}
