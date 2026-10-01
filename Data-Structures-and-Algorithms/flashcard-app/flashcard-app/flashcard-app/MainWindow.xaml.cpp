// Set file encoding to UTF-8
#pragma execution_character_set("utf-8")

#include "pch.h"
#include "MainWindow.xaml.h"
#include "FlashcardPage.xaml.h"
#include "SettingsPage.xaml.h"
#include "LanguageConfig.h"
#if __has_include("MainWindow.g.cpp")
#include "MainWindow.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace Microsoft::UI::Xaml::Controls;

namespace winrt::flashcard_app::implementation
{
    MainWindow::MainWindow()
    {
        InitializeComponent();

        // Đặt tiêu đề cửa sổ theo ngôn ngữ
        Title(g_currentLanguage == AppLanguage::Vietnamese
            ? L"Flashcard App"
            : L"Flashcard App");

        // Mặc định mở FlashcardPage
        ContentFrame().Navigate(
            winrt::xaml_typename<flashcard_app::FlashcardPage>());
    }

    void MainWindow::NavView_SelectionChanged(
        NavigationView const& /*sender*/,
        NavigationViewSelectionChangedEventArgs const& args)
    {
        auto item = args.SelectedItem().as<NavigationViewItem>();
        auto tag  = winrt::unbox_value<winrt::hstring>(item.Tag());

        if (tag == L"flashcards")
        {
            ContentFrame().Navigate(
                winrt::xaml_typename<flashcard_app::FlashcardPage>());
            
            // Update window title based on current language
            Title(g_currentLanguage == AppLanguage::Vietnamese
                ? L"Flashcard App"
                : L"Flashcard App");
        }
        else if (tag == L"settings")
        {
            ContentFrame().Navigate(
                winrt::xaml_typename<flashcard_app::SettingsPage>());
        }
    }
}
