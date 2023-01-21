using Foundation;
using UIKit;

#if !RUN_HEADLESS
using Xunit.Runners.Maui;
using Xunit.Runners.Maui.VisualRunner;
#endif

namespace Couchbase.Lite.Tests.Maui
{
    [Register("AppDelegate")]
#if RUN_HEADLESS
    public class AppDelegate : MauiTestApplicationDelegate
#else
    public class AppDelegate : MauiUIApplicationDelegate
#endif
    {

#if !RUN_HEADLESS

        bool _testStarted = false;
        IWindow _thisWindow;

        public override async void OnActivated(UIApplication application)
        {
            base.OnActivated(application);

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

#else

        //HeadlessTestRunner runner = null;

        //public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        //{
        //    runner = AppDelegate.Current.Services.GetRequiredService<HeadlessTestRunner>();
        //    _ = runner.RunTestsAsync();
            
        //    return base.FinishedLaunching(application, launchOptions);
        //}

#endif

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}