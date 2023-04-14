using System.IO;
using System.Reflection;

using Android.App;
using Android.Content;
using Android.OS;

using Xunit.Runners.ResultChannels;
using Xunit.Runners.UI;
using Xunit.Sdk;

namespace Couchbase.Lite.Tests.Android
{
    [Activity(Label = "CBLUnit", MainLauncher = true, Theme = "@android:style/Theme.Material.Light",
        Name = "test.activity")]
    public class MainActivity : RunnerActivity
    {
        #region Properties

        public static Context ActivityContext { get; private set; }

        #endregion

        #region Overrides

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ActivityContext = ApplicationContext;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Couchbase.Lite.Support.Droid.Activate(ApplicationContext);

            // tests can be inside the main assembly
            AddTestAssembly(Assembly.GetExecutingAssembly());

            AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);
            AutoStart = true;

            // This doesn't seem to work on Android...at least not on GenyMotion
            TerminateAfterExecution = true;
            using (var str = GetType().Assembly.GetManifestResourceStream("result_ip"))
            using (var sr = new StreamReader(str)) {
                ResultChannel = new TextWriterResultChannel(new TcpTextWriter(sr.ReadToEnd().TrimEnd(), 54321));
            }

            base.OnCreate(savedInstanceState);
        }

        #endregion
    }
}