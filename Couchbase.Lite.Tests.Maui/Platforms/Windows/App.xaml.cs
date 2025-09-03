using System.Diagnostics;
using Microsoft.UI.Xaml;
using Xunit.Runners.Maui.VisualRunner;
using Xunit.Runners.Maui;

namespace Couchbase.Lite.Tests.Maui.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MiddleApp
    {
        private IWindow? _thisWindow;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            try {
                base.OnLaunched(args);

                var app = TestServices.Services.GetService<IApplication>()!;
                _thisWindow = app.Windows[0];
                var p = ((NavigationPage)_thisWindow.Content!).CurrentPage;
                await ((HomeViewModel)((NavigationPage)_thisWindow.Content).CurrentPage.BindingContext).StartAssemblyScanAsync();
            }
            catch (Exception e) {
                Debug.WriteLine($"OnLaunched Exception: {e}");
            }
        }
    }

    public class MiddleApp : MauiWinUIApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
