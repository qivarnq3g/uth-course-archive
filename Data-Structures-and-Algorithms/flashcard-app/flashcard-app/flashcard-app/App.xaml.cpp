#include "pch.h"
#include "App.xaml.h"
#include "MainWindow.xaml.h"
#include "LanguageConfig.h"
#include <winrt/Windows.Storage.h>

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace winrt::Windows::Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::flashcard_app::implementation
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    App::App()
    {
        // Xaml objects should not call InitializeComponent during construction.
        // See https://github.com/microsoft/cppwinrt/tree/master/nuget#initializecomponent

        // Load saved language preference
        auto appData = ApplicationData::Current();
        auto values = appData.LocalSettings().Values();

        if (auto boxed = values.TryLookup(L"language"))
        {
            auto lang = winrt::unbox_value<winrt::hstring>(boxed);
            if (lang == L"vi")
            {
                g_currentLanguage = AppLanguage::Vietnamese;
            }
            else
            {
                g_currentLanguage = AppLanguage::English;
            }
        }
        else
        {
            g_currentLanguage = AppLanguage::Vietnamese;
        }

#if defined _DEBUG && !defined DISABLE_XAML_GENERATED_BREAK_ON_UNHANDLED_EXCEPTION
        UnhandledException([](IInspectable const&, UnhandledExceptionEventArgs const& e)
        {
            if (IsDebuggerPresent())
            {
                auto errorMessage = e.Message();
                __debugbreak();
            }
        });
#endif
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    void App::OnLaunched([[maybe_unused]] LaunchActivatedEventArgs const& e)
    {
        window = make<MainWindow>();
        window.Activate();
    }
}
