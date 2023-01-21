using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.LifecycleEvents;
using Xunit.Runners.Maui;
using Microsoft.Extensions.Logging;

namespace Couchbase.Lite.Tests.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
#if RUN_HEADLESS

            #if ANDROID
            builder.UseMauiApp<App>();
            builder.Services.AddTransient(svc => new HeadlessTestRunner("testresults.xml"));
            #elif IOS
            var runnerOptions = new HeadlessRunnerOptions
            {
                RequiresUIContext = true
            };

            var testOptions = new TestOptions
            {
                Assemblies =
                {
                    typeof(MauiProgram).Assembly
                }
            };

            builder.Services.AddTransient(svc => new HeadlessTestRunner(runnerOptions, testOptions));

            #endif
#else
            builder.ConfigureTests(new TestOptions
            {
                Assemblies =
                {
                    typeof(MauiProgram).Assembly
                }
            })
            .UseVisualRunner();
#endif

            return builder.Build();

            
        }
    }
}