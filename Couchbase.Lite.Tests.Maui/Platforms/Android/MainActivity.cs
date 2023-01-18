using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Xunit.Runners.Maui.VisualRunner;
using Xunit.Runners.Maui;

namespace Couchbase.Lite.Tests.Maui
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : MauiAppCompatActivity
	{
        #if !RUN_HEADLESS
        public static Context ActivityContext { get; private set; }

        IWindow _thisWindow;
        bool _testStarted = false;

        protected override void OnCreate(Bundle bundle)
        {
            ActivityContext = ApplicationContext;
            Support.Droid.Activate(ApplicationContext);

            // you cannot add more assemblies once calling base
            base.OnCreate(bundle);
        }

        protected async override void OnPostCreate(Bundle bundle)
        {
            base.OnPostCreate(bundle);
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
        #endif
    }
}