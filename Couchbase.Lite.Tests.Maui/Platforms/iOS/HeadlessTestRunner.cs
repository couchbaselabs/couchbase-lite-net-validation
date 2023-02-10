using Microsoft.DotNet.XHarness.TestRunners.Common;
using Microsoft.DotNet.XHarness.TestRunners.Xunit;
using UIKit;
using Xunit.Runners.Maui;

namespace Couchbase.Lite.Tests.Maui
{
    public class HeadlessTestRunner : iOSApplicationEntryPoint
    {
        readonly HeadlessRunnerOptions _runnerOptions;
        readonly TestOptions _options;

        public HeadlessTestRunner(HeadlessRunnerOptions runnerOptions, TestOptions options)
        {
            _runnerOptions = runnerOptions;
            _options = options;
        }

        protected override bool LogExcludedTests => true;

        protected override int? MaxParallelThreads => Environment.ProcessorCount;

        protected override IDevice Device { get; } = new TestDevice();

        protected override IEnumerable<TestAssemblyInfo> GetTestAssemblies() =>
            _options.Assemblies
                .Distinct()
                .Select(assembly => new TestAssemblyInfo(assembly, assembly.Location));

        protected override void TerminateWithSuccess()
        {
            var s = new ObjCRuntime.Selector("terminateWithSuccess");
            UIApplication.SharedApplication.PerformSelector(s, UIApplication.SharedApplication, 0);
        }

        protected override TestRunner GetTestRunner(LogWriter logWriter)
        {
            var testRunner = base.GetTestRunner(logWriter);
            if (_options.SkipCategories?.Count > 0)
                testRunner.SkipCategories(_options.SkipCategories);
            return testRunner;
        }

        public async Task RunTestsAsync()
        {
            TestStarted += OnTestStarted;
            TestCompleted += OnTestCompleted;
            TestsCompleted += OnTestsCompleted;

            await RunAsync().ConfigureAwait(false);

            TestsCompleted -= OnTestsCompleted;

            void OnTestsCompleted(object? sender, TestRunResult results)
            {
                var message =
                    $"Tests run: {results.ExecutedTests} " +
                    $"Passed: {results.PassedTests} " +
                    $"Inconclusive: {results.InconclusiveTests} " +
                    $"Failed: {results.FailedTests} " +
                    $"Ignored: {results.SkippedTests}";

                Console.WriteLine(message);
            }

            void OnTestCompleted(object sender, (string name, TestResult result) arg)
            {
                Console.WriteLine($"[{arg.result}] {arg.name}");
            }

            void OnTestStarted(object sender, string name)
            {
                Console.WriteLine($"{name} started!");
            }
        }
    }
}
