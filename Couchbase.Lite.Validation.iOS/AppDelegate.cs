using System.IO;
using System.Reflection;

using Foundation;
using UIKit;

using Xunit.Runner;
using Xunit.Runners.ResultChannels;
using Xunit.Runners.UI;
using Xunit.Sdk;

namespace Couchbase.Lite.Validation.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : RunnerAppDelegate
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // We need this to ensure the execution assembly is part of the app bundle
            AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);


            // tests can be inside the main assembly
            AddTestAssembly(Assembly.GetExecutingAssembly());
            AutoStart = true;
            TerminateAfterExecution = true;
            using (var str = GetType().Assembly.GetManifestResourceStream("result_ip"))
            using (var sr = new StreamReader(str))
            {
                ResultChannel = new TextWriterResultChannel(new TcpTextWriter(sr.ReadToEnd().TrimEnd(), 12345));
            }

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}

