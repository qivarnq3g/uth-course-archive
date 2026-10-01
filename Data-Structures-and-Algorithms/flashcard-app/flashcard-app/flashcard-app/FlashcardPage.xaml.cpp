#pragma execution_character_set("utf-8")

#include "pch.h"
#include "FlashcardPage.xaml.h"
#include "LanguageConfig.h"
#include <chrono>
#include <winrt/Windows.ApplicationModel.DataTransfer.h>
#if __has_include("FlashcardPage.g.cpp")
#include "FlashcardPage.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace Microsoft::UI::Xaml::Controls;
using namespace winrt::Windows::ApplicationModel::DataTransfer;

namespace winrt::flashcard_app::implementation
{
    void FlashcardPage::Page_Loaded(
        [[maybe_unused]] IInspectable const& sender,
        [[maybe_unused]] RoutedEventArgs const& args)
    {
        UpdateLanguageTexts();
        UpdateUI();
    }

    void FlashcardPage::UpdateLanguageTexts()
    {
        if (g_currentLanguage == AppLanguage::Vietnamese)
        {
            AddToEndButton().Content(winrt::box_value(L"Thêm cuối"));
            AddToStartButton().Content(winrt::box_value(L"Thêm đầu"));
            DeleteButton().Content(winrt::box_value(L"Xóa"));
            DeleteCurrentButton().Content(winrt::box_value(L"Xóa thẻ hiện tại"));
            FindButton().Content(winrt::box_value(L"Tìm"));
            PreviousButtonText().Text(L"Trước");
            NextButtonText().Text(L"Sau");
            TraverseForwardButtonText().Text(L"Duyệt xuôi");
            TraverseBackwardButtonText().Text(L"Duyệt ngược");
            CurrentCardLabel().Text(L"Thẻ từ vựng hiện tại");
            NewCardInput().PlaceholderText(L"Nhập từ vựng...");
            DeleteCardInput().PlaceholderText(L"Nhập từ cần xóa...");
            FindIndexInput().PlaceholderText(L"Nhập vị trí (0,1,2...)");
            AddCardSectionLabel().Text(L"Thêm thẻ mới");
            DeleteCardSectionLabel().Text(L"Xóa thẻ");
            FindCardSectionLabel().Text(L"Tìm thẻ");
            TraverseSectionLabel().Text(L"Duyệt danh sách");
            ListSectionLabel().Text(L"Danh sách thẻ");
            EditCardSectionLabel().Text(L"Sửa thẻ");
            EditCardInput().PlaceholderText(L"Nhập nội dung mới...");
            EditButton().Content(winrt::box_value(L"Sửa"));
            PerformanceTestSectionLabel().Text(L"Performance Test");
            PerformanceTestDescription().Text(L"Test hiệu năng với các kích thước khác nhau");
            Test10Button().Content(winrt::box_value(L"Test 10"));
            Test100Button().Content(winrt::box_value(L"Test 100"));
            Test1000Button().Content(winrt::box_value(L"Test 1,000"));
            Test10000Button().Content(winrt::box_value(L"Test 10,000"));
            TestResultText().Text(L"Chưa có kết quả test. Click nút test để bắt đầu.");
            CopyResultsButton().Content(winrt::box_value(L"Copy"));
        }
        else
        {
            AddToEndButton().Content(winrt::box_value(L"Add to end"));
            AddToStartButton().Content(winrt::box_value(L"Add to front"));
            DeleteButton().Content(winrt::box_value(L"Delete"));
            DeleteCurrentButton().Content(winrt::box_value(L"Delete current card"));
            FindButton().Content(winrt::box_value(L"Find"));
            PreviousButtonText().Text(L"Previous");
            NextButtonText().Text(L"Next");
            TraverseForwardButtonText().Text(L"Traverse forward");
            TraverseBackwardButtonText().Text(L"Traverse backward");
            CurrentCardLabel().Text(L"Current flashcard");
            NewCardInput().PlaceholderText(L"Enter vocabulary...");
            DeleteCardInput().PlaceholderText(L"Enter word to delete...");
            FindIndexInput().PlaceholderText(L"Enter position (0,1,2...)");
            AddCardSectionLabel().Text(L"Add new card");
            DeleteCardSectionLabel().Text(L"Delete card");
            FindCardSectionLabel().Text(L"Find card");
            TraverseSectionLabel().Text(L"Traverse list");
            ListSectionLabel().Text(L"Card list");
            EditCardSectionLabel().Text(L"Edit card");
            EditCardInput().PlaceholderText(L"Enter new content...");
            EditButton().Content(winrt::box_value(L"Edit"));
            PerformanceTestSectionLabel().Text(L"Performance Test");
            PerformanceTestDescription().Text(L"Test performance with different sizes");
            Test10Button().Content(winrt::box_value(L"Test 10"));
            Test100Button().Content(winrt::box_value(L"Test 100"));
            Test1000Button().Content(winrt::box_value(L"Test 1,000"));
            Test10000Button().Content(winrt::box_value(L"Test 10,000"));
            TestResultText().Text(L"No test results yet. Click test button to start.");
            CopyResultsButton().Content(winrt::box_value(L"Copy"));
        }
    }

    void FlashcardPage::ShowStatus(winrt::hstring const& message, InfoBarSeverity severity)
    {
        StatusBar().Message(message);
        StatusBar().Severity(severity);
        StatusBar().IsOpen(true);
    }

    void FlashcardPage::UpdateUI()
    {
        if (!m_list) return;

        Node* current = m_list->getCurrent();
        int currentIdx = m_list->getCurrentIndex();
        int size = m_list->getSize();

        if (current && currentIdx >= 0) {
            CurrentCardText().Text(current->data);
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                winrt::hstring indexText = L"Vị trí: " + winrt::to_hstring(currentIdx) + L"/" + winrt::to_hstring(size - 1);
                CardIndexText().Text(indexText);
            } else {
                winrt::hstring indexText = L"Position: " + winrt::to_hstring(currentIdx) + L"/" + winrt::to_hstring(size - 1);
                CardIndexText().Text(indexText);
            }
        } else {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                CurrentCardText().Text(L"Chưa có thẻ");
                CardIndexText().Text(L"Vị trí: -/-");
            } else {
                CurrentCardText().Text(L"No card");
                CardIndexText().Text(L"Position: -/-");
            }
        }

        if (size == 0) {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ListDisplay().Text(L"Danh sách trống");
            } else {
                ListDisplay().Text(L"List is empty");
            }
        } else {
            ListDisplay().Text(m_list->getAllCardsAsString());
        }
    }

    void FlashcardPage::Previous_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        int currentIdx = m_list->getCurrentIndex();
        if (currentIdx > 0) {
            m_list->setCurrentIndex(currentIdx - 1);
            UpdateUI();
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Đã chuyển đến thẻ trước", InfoBarSeverity::Success);
            } else {
                ShowStatus(L"Moved to previous card", InfoBarSeverity::Success);
            }
        } else {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Đã ở thẻ đầu tiên", InfoBarSeverity::Warning);
            } else {
                ShowStatus(L"Already at first card", InfoBarSeverity::Warning);
            }
        }
    }

    void FlashcardPage::Next_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        int currentIdx = m_list->getCurrentIndex();
        int size = m_list->getSize();
        
        if (currentIdx < size - 1) {
            m_list->setCurrentIndex(currentIdx + 1);
            UpdateUI();
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Đã chuyển đến thẻ sau", InfoBarSeverity::Success);
            } else {
                ShowStatus(L"Moved to next card", InfoBarSeverity::Success);
            }
        } else {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Đã ở thẻ cuối cùng", InfoBarSeverity::Warning);
            } else {
                ShowStatus(L"Already at last card", InfoBarSeverity::Warning);
            }
        }
    }

    void FlashcardPage::AddToEnd_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        winrt::hstring input = NewCardInput().Text();
        std::wstring value{ input };

        if (value.empty()) {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Vui lòng nhập từ vựng", InfoBarSeverity::Warning);
            } else {
                ShowStatus(L"Please enter vocabulary", InfoBarSeverity::Warning);
            }
            return;
        }

        m_list->append(value);
        NewCardInput().Text(L"");
        UpdateUI();
        if (g_currentLanguage == AppLanguage::Vietnamese) {
            ShowStatus(L"Đã thêm thẻ vào cuối danh sách", InfoBarSeverity::Success);
        } else {
            ShowStatus(L"Added card to end of list", InfoBarSeverity::Success);
        }
    }

    void FlashcardPage::AddToStart_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        winrt::hstring input = NewCardInput().Text();
        std::wstring value{ input };

        if (value.empty()) {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Vui lòng nhập từ vựng", InfoBarSeverity::Warning);
            } else {
                ShowStatus(L"Please enter vocabulary", InfoBarSeverity::Warning);
            }
            return;
        }

        m_list->prepend(value);
        NewCardInput().Text(L"");
        UpdateUI();
        if (g_currentLanguage == AppLanguage::Vietnamese) {
            ShowStatus(L"Đã thêm thẻ vào đầu danh sách", InfoBarSeverity::Success);
        } else {
            ShowStatus(L"Added card to start of list", InfoBarSeverity::Success);
        }
    }

    void FlashcardPage::Delete_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        winrt::hstring input = DeleteCardInput().Text();
        std::wstring value{ input };

        if (value.empty()) {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Vui lòng nhập từ cần xóa", InfoBarSeverity::Warning);
            } else {
                ShowStatus(L"Please enter word to delete", InfoBarSeverity::Warning);
            }
            return;
        }

        if (m_list->deleteNode(value)) {
            DeleteCardInput().Text(L"");
            UpdateUI();
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Đã xóa thẻ thành công", InfoBarSeverity::Success);
            } else {
                ShowStatus(L"Card deleted successfully", InfoBarSeverity::Success);
            }
        } else {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Không tìm thấy thẻ để xóa", InfoBarSeverity::Error);
            } else {
                ShowStatus(L"Card not found to delete", InfoBarSeverity::Error);
            }
        }
    }

    void FlashcardPage::DeleteCurrent_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        if (m_list->deleteCurrent()) {
            UpdateUI();
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Đã xóa thẻ hiện tại", InfoBarSeverity::Success);
            } else {
                ShowStatus(L"Deleted current card successfully", InfoBarSeverity::Success);
            }
        } else {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Không có thẻ hiện tại để xóa", InfoBarSeverity::Error);
            } else {
                ShowStatus(L"No current card to delete", InfoBarSeverity::Error);
            }
        }
    }

    void FlashcardPage::Find_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        int index = static_cast<int>(FindIndexInput().Value());
        Node* found = m_list->findByIndex(index);

        if (found) {
            UpdateUI();
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Đã tìm thấy thẻ tại vị trí " + winrt::to_hstring(index), InfoBarSeverity::Success);
            } else {
                ShowStatus(L"Found card at position " + winrt::to_hstring(index), InfoBarSeverity::Success);
            }
        } else {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Không tìm thấy thẻ tại vị trí " + winrt::to_hstring(index), InfoBarSeverity::Error);
            } else {
                ShowStatus(L"Card not found at position " + winrt::to_hstring(index), InfoBarSeverity::Error);
            }
        }
    }

    void FlashcardPage::Edit_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        winrt::hstring input = EditCardInput().Text();
        std::wstring newValue{ input };

        if (newValue.empty()) {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Vui lòng nhập nội dung mới", InfoBarSeverity::Warning);
            } else {
                ShowStatus(L"Please enter new content", InfoBarSeverity::Warning);
            }
            return;
        }

        if (m_list->updateCurrent(newValue)) {
            EditCardInput().Text(L"");
            UpdateUI();
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Đã cập nhật thẻ hiện tại", InfoBarSeverity::Success);
            } else {
                ShowStatus(L"Updated current card successfully", InfoBarSeverity::Success);
            }
        } else {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Không có thẻ hiện tại để sửa", InfoBarSeverity::Error);
            } else {
                ShowStatus(L"No current card to update", InfoBarSeverity::Error);
            }
        }
    }

    void FlashcardPage::TraverseForward_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        int size = m_list->getSize();
        if (size == 0) {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ListDisplay().Text(L"Danh sách trống");
            } else {
                ListDisplay().Text(L"List is empty");
            }
        } else {
            ListDisplay().Text(m_list->getAllCardsAsString());
        }
        if (g_currentLanguage == AppLanguage::Vietnamese) {
            ShowStatus(L"Đã duyệt danh sách từ đầu đến cuối", InfoBarSeverity::Informational);
        } else {
            ShowStatus(L"Traversed list from start to end", InfoBarSeverity::Informational);
        }
    }

    void FlashcardPage::TraverseBackward_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        if (!m_list) return;

        int size = m_list->getSize();
        if (size == 0) {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ListDisplay().Text(L"Danh sách trống");
            } else {
                ListDisplay().Text(L"List is empty");
            }
        } else {
            ListDisplay().Text(m_list->getAllCardsAsStringReverse());
        }
        if (g_currentLanguage == AppLanguage::Vietnamese) {
            ShowStatus(L"Đã duyệt danh sách từ cuối về đầu", InfoBarSeverity::Informational);
        } else {
            ShowStatus(L"Traversed list from end to start", InfoBarSeverity::Informational);
        }
    }

    void FlashcardPage::TestPerformance(int cardCount)
    {
        if (!m_list) return;

        m_list.reset();
        m_list = std::make_unique<DoublyLinkedList>();

        {
            DoublyLinkedList warmup;
            for (int i = 0; i < 100; i++) {
                std::wstring warmupStr = L"Warmup " + std::to_wstring(i);
                warmup.append(warmupStr);
            }
        }

        std::wstring result;
        result = L"Performance Test - " + std::to_wstring(cardCount) + L" thẻ:\n\n";

        auto start = std::chrono::high_resolution_clock::now();
        auto end = std::chrono::high_resolution_clock::now();

        start = std::chrono::high_resolution_clock::now();
        for (int i = 0; i < cardCount; i++) {
            std::wstring card = L"Thẻ số " + std::to_wstring(i + 1);
            m_list->append(card);
        }
        end = std::chrono::high_resolution_clock::now();
        auto insertTailTime = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();
        std::wstring insertTailLine = L"Thêm cuối: " + std::to_wstring(insertTailTime) + L" ms - O(1)\n";
        result += insertTailLine;

        start = std::chrono::high_resolution_clock::now();
        m_list->prepend(L"Thẻ đầu");
        end = std::chrono::high_resolution_clock::now();
        auto insertHeadTime = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();
        std::wstring insertHeadLine = L"Thêm đầu: " + std::to_wstring(insertHeadTime) + L" μs - O(1)\n";
        result += insertHeadLine;

        int middlePos = m_list->getSize() / 2;
        start = std::chrono::high_resolution_clock::now();
        m_list->insertAt(middlePos, L"Thẻ giữa");
        end = std::chrono::high_resolution_clock::now();
        auto insertMiddleTime = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();
        std::wstring insertMiddleLine = L"Thêm giữa: " + std::to_wstring(insertMiddleTime) + L" μs - O(n)\n";
        result += insertMiddleLine;

        start = std::chrono::high_resolution_clock::now();
        Node* found = m_list->findByIndex(cardCount / 2);
        end = std::chrono::high_resolution_clock::now();
        auto findIndexTime = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();
        std::wstring findIndexLine = L"Tìm theo vị trí: " + std::to_wstring(findIndexTime) + L" μs - O(n)\n";
        result += findIndexLine;

        std::wstring searchValue = L"Thẻ số " + std::to_wstring(cardCount / 2);
        start = std::chrono::high_resolution_clock::now();
        Node* searchResult = m_list->search(searchValue);
        end = std::chrono::high_resolution_clock::now();
        auto searchTime = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();
        std::wstring searchLine = L"Tìm theo giá trị: " + std::to_wstring(searchTime) + L" μs - O(n)\n";
        result += searchLine;

        start = std::chrono::high_resolution_clock::now();
        int forwardCount = m_list->traverseForwardCount();
        end = std::chrono::high_resolution_clock::now();
        auto traverseForwardTime = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();
        std::wstring traverseForwardLine = L"Duyệt xuôi: " + std::to_wstring(traverseForwardTime) + L" μs - O(n)\n";
        result += traverseForwardLine;

        start = std::chrono::high_resolution_clock::now();
        int backwardCount = m_list->traverseBackwardCount();
        end = std::chrono::high_resolution_clock::now();
        auto traverseBackwardTime = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();
        std::wstring traverseBackwardLine = L"Duyệt ngược: " + std::to_wstring(traverseBackwardTime) + L" μs - O(n)\n";
        result += traverseBackwardLine;

        start = std::chrono::high_resolution_clock::now();
        m_list->deleteNode(L"Thẻ đầu");
        end = std::chrono::high_resolution_clock::now();
        auto deleteHeadTime = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();
        std::wstring deleteHeadLine = L"Xóa đầu: " + std::to_wstring(deleteHeadTime) + L" μs - O(1)\n";
        result += deleteHeadLine;

        int tailIndex = m_list->getSize() - 1;
        Node* tailNode = m_list->findByIndex(tailIndex);
        if (tailNode) {
            start = std::chrono::high_resolution_clock::now();
            m_list->deleteNode(tailNode->data);
            end = std::chrono::high_resolution_clock::now();
            auto deleteTailTime = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();
            std::wstring deleteTailLine = L"Xóa cuối: " + std::to_wstring(deleteTailTime) + L" μs - O(1)\n";
            result += deleteTailLine;
        }

        middlePos = m_list->getSize() / 2;
        Node* middleNode = m_list->findByIndex(middlePos);
        if (middleNode) {
            start = std::chrono::high_resolution_clock::now();
            m_list->deleteNode(middleNode->data);
            end = std::chrono::high_resolution_clock::now();
            auto deleteMiddleTime = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();
            std::wstring deleteMiddleLine = L"Xóa giữa: " + std::to_wstring(deleteMiddleTime) + L" μs - O(n)\n";
            result += deleteMiddleLine;
        }

        result += L"\n--- Tóm tắt ---\n";
        result += L"Số thẻ sau test: " + std::to_wstring(m_list->getSize()) + L" thẻ\n";

        winrt::hstring hresult(result);
        TestResultText().Text(hresult);
        
        UpdateUI();

        std::wstring statusMsgStr;
        if (g_currentLanguage == AppLanguage::Vietnamese) {
            statusMsgStr = L"Đã hoàn tất test đầy đủ với " + std::to_wstring(cardCount) + L" thẻ";
        } else {
            statusMsgStr = L"Completed comprehensive test with " + std::to_wstring(cardCount) + L" cards";
        }
        winrt::hstring statusMsg(statusMsgStr);
        ShowStatus(statusMsg, InfoBarSeverity::Success);
    }

    void FlashcardPage::Test10_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        TestPerformance(10);
    }

    void FlashcardPage::Test100_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        TestPerformance(100);
    }

    void FlashcardPage::Test1000_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        TestPerformance(1000);
    }

    void FlashcardPage::Test10000_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        TestPerformance(10000);
    }

    void FlashcardPage::CopyResults_Click([[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender, [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        winrt::hstring resultText = TestResultText().Text();
        
        if (resultText.empty() || resultText == L"Chưa có kết quả test. Click nút test để bắt đầu." || resultText == L"No test results yet. Click test button to start.") {
            if (g_currentLanguage == AppLanguage::Vietnamese) {
                ShowStatus(L"Không có dữ liệu để copy", InfoBarSeverity::Warning);
            } else {
                ShowStatus(L"No data to copy", InfoBarSeverity::Warning);
            }
            return;
        }

        DataPackage dataPackage;
        dataPackage.SetText(resultText);
        Clipboard::SetContent(dataPackage);

        if (g_currentLanguage == AppLanguage::Vietnamese) {
            ShowStatus(L"Đã copy kết quả test vào clipboard", InfoBarSeverity::Success);
        } else {
            ShowStatus(L"Test results copied to clipboard", InfoBarSeverity::Success);
        }
    }
}

