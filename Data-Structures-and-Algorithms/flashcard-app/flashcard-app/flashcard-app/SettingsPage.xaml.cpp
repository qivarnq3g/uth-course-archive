// Set file encoding to UTF-8
#pragma execution_character_set("utf-8")

#include "pch.h"
#include "SettingsPage.xaml.h"
#include "LanguageConfig.h"
#include <winrt/Windows.Storage.h>
#if __has_include("SettingsPage.g.cpp")
#include "SettingsPage.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace Microsoft::UI::Xaml::Controls;
using namespace winrt::Windows::Storage;

namespace winrt::flashcard_app::implementation
{
    SettingsPage::SettingsPage()
    {
        InitializeComponent();

        // Đọc ngôn ngữ hiện tại để set ComboBox ban đầu
        auto appData = ApplicationData::Current();
        auto values = appData.LocalSettings().Values();
        winrt::hstring tag = L"vi";

        if (auto boxed = values.TryLookup(L"language"))
        {
            tag = winrt::unbox_value<winrt::hstring>(boxed);
        }

        // Set ComboBox và g_currentLanguage mà không trigger SelectionChanged
        if (tag == L"vi")
        {
            LanguageCombo().SelectedIndex(0);
            g_currentLanguage = AppLanguage::Vietnamese;
        }
        else
        {
            LanguageCombo().SelectedIndex(1);
            g_currentLanguage = AppLanguage::English;
        }

        m_isInitialized = true; // Từ đây trở đi mới xử lý SelectionChanged

        // Update language texts
        UpdateLanguageTexts();
    }

    void SettingsPage::Page_Loaded(
        [[maybe_unused]] winrt::Windows::Foundation::IInspectable const& sender,
        [[maybe_unused]] winrt::Microsoft::UI::Xaml::RoutedEventArgs const& args)
    {
        // Update language texts when page is loaded
        UpdateLanguageTexts();
    }

    void SettingsPage::UpdateLanguageTexts()
    {
        if (g_currentLanguage == AppLanguage::Vietnamese)
        {
            // Vietnamese texts
            LanguageTitle().Text(L"Ngôn ngữ / Language");
            DeveloperTitle().Text(L"Thông tin người phát triển");
            DevNameText().Text(L"Hoàng Việt Quang");
            GitHubLabelText().Text(L"GitHub:");
        }
        else
        {
            // English texts
            LanguageTitle().Text(L"Language");
            DeveloperTitle().Text(L"About developer");
            DevNameText().Text(L"Hoang Viet Quang");
            GitHubLabelText().Text(L"GitHub:");
        }
    }

    void SettingsPage::LanguageCombo_SelectionChanged(
        winrt::Windows::Foundation::IInspectable const& sender,
        winrt::Microsoft::UI::Xaml::Controls::SelectionChangedEventArgs const&)
    {
        if (!m_isInitialized)
            return; // Lần SelectionChanged khi load ban đầu → bỏ qua

        auto combo = sender.as<ComboBox>();
        if (!combo.SelectedItem()) return;

        auto item = combo.SelectedItem().as<ComboBoxItem>();
        auto tag = winrt::unbox_value<winrt::hstring>(item.Tag());

        // Cập nhật biến global
        if (tag == L"vi")
        {
            g_currentLanguage = AppLanguage::Vietnamese;
        }
        else
        {
            g_currentLanguage = AppLanguage::English;
        }

        // Lưu vào LocalSettings
        auto appData = ApplicationData::Current();
        auto values = appData.LocalSettings().Values();
        values.Insert(L"language", winrt::box_value(tag));

        // Update language texts immediately
        UpdateLanguageTexts();
    }
}

