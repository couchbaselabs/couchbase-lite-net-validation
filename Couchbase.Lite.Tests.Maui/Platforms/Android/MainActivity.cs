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
        public static Context? ActivityContext { get; private set; }

        private IWindow? _thisWindow;

        protected override void OnCreate(Bundle? bundle)
        {
            ActivityContext = ApplicationContext;
            Support.Droid.Activate(ApplicationContext!);

            // you cannot add more assemblies once calling base
            base.OnCreate(bundle);
        }

        protected override async void OnPostCreate(Bundle? bundle)
        {
            try {
                base.OnPostCreate(bundle);
                var app = TestServices.Services.GetRequiredService<IApplication>();
                _thisWindow = app.Windows[0];
                var p = ((NavigationPage)_thisWindow.Content!).CurrentPage;
                await ((HomeViewModel)((NavigationPage)_thisWindow.Content).CurrentPage.BindingContext).StartAssemblyScanAsync();
            }
            catch (Exception e) {
                Android.Util.Log.Error("App", $"OnPostCreate Exception: {e}");
            }
        }

        #endif
    }
}