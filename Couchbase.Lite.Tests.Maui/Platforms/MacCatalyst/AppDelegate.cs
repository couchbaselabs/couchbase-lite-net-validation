using Foundation;
using UIKit;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

#if !RUN_HEADLESS
using Xunit.Runners.Maui;
using Xunit.Runners.Maui.VisualRunner;
#endif

namespace Couchbase.Lite.Tests.Maui
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
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
        }

#else

        HeadlessTestRunner runner = null;
        // TODO: https://github.com/xamarin/xamarin-macios/issues/12555
        readonly static string[] EnvVarNames = {
            "NUNIT_AUTOSTART",
            "NUNIT_AUTOEXIT",
            "NUNIT_ENABLE_NETWORK",
            "DISABLE_SYSTEM_PERMISSION_TESTS",
            "NUNIT_HOSTNAME",
            "NUNIT_TRANSPORT",
            "NUNIT_LOG_FILE",
            "NUNIT_HOSTPORT",
            "USE_TCP_TUNNEL",
            "RUN_END_TAG",
            "NUNIT_ENABLE_XML_OUTPUT",
            "NUNIT_ENABLE_XML_MODE",
            "NUNIT_XML_VERSION",
            "NUNIT_SORTNAMES",
            "NUNIT_RUN_ALL",
            "NUNIT_SKIPPED_METHODS",
            "NUNIT_SKIPPED_CLASSES",
        };

        readonly static Dictionary<string, string?> EnvVars = new();

        static AppDelegate()
        {
            // copy into dictionary for later
            foreach (var envvar in EnvVarNames)
            {
                EnvVars[envvar] = Environment.GetEnvironmentVariable(envvar);
            }
        }

        public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
        {
            SetEnvironmentVariables();
            return base.WillFinishLaunching(application, launchOptions);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            runner = AppDelegate.Current.Services.GetRequiredService<HeadlessTestRunner>();
            _ = runner.RunTestsAsync();

            return base.FinishedLaunching(application, launchOptions);
        }

        static void SetEnvironmentVariables()
        {
            // read from dictionary
            foreach (var envvar in EnvVars)
            {
                Console.WriteLine($"  {envvar.Key} = '{envvar.Value}'");
                Environment.SetEnvironmentVariable(envvar.Key, envvar.Value);
            }
        }

#endif

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}