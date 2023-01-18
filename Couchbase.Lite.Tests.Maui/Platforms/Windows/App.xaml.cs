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
        IWindow _thisWindow;
        bool _testStarted = false;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            #if DEBUG
            Support.WinUI.CheckVersion();
            #endif
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            var app = TestServices.Services.GetService<IApplication>();
            _thisWindow = app.Windows[0];
            var p = ((NavigationPage)_thisWindow.Content).CurrentPage;
            await ((HomeViewModel)((NavigationPage)_thisWindow.Content).CurrentPage.BindingContext).StartAssemblyScanAsync();
            p.Loaded += (sender, e) =>
            {
                StartTests();
            };
        }

        private void StartTests()
        {
            var command = ((HomeViewModel)((NavigationPage)_thisWindow.Content).CurrentPage.BindingContext).RunEverythingCommand;
            var isBusy = ((HomeViewModel)((NavigationPage)_thisWindow.Content).CurrentPage.BindingContext).IsBusy;
            if (!_testStarted) {
                _testStarted = true;
                command.Execute(() => !isBusy);
            }
        }
    }

    public class MiddleApp : MauiWinUIApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
