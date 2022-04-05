using Sentry.Xamarin.Forms.Internals;
using Sentry.Xamarin.Forms.Testing.Mock;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;

namespace Sentry.Xamarin.Forms.UWP.Tests.Internals
{
    public class FormsApplicationListenerTests
    {
        [Fact]
        // Test
        public async Task Register_FormsNotInitialized_HooksNotInvoked()
        {
            //Assert
            var mockDiagnostic = new MockDiagnosticLogger(SentryLevel.Debug);
            var options = new SentryXamarinOptions()
            {
                Debug = true,
                DiagnosticLogger = mockDiagnostic,
                GetCurrentApplicationDelay = 1,
                GetCurrentApplicationMaxRetries = 1
            };
            var integration = new FormsApplicationListener(options);
            var mockHub = new MockHub();
            Action<Application> badListener = (_) => throw null;
            integration.AddListener(badListener);

            //Act
            integration.Invoke();

            await Task.Delay(options.GetCurrentApplicationDelay + 100);

            //Assert
            Assert.True(mockDiagnostic.Contains("Sentry.Xamarin.Forms timeout for tracking Application.Current. Navigation tracking is going to be disabled"));
        }

        [Fact]
        public async Task Register_FormsNotInitializedAndWithoutLogger_IgnoreIntegration()
        {
            //Assert
            var options = new SentryXamarinOptions()
            {
                GetCurrentApplicationDelay = 1,
                GetCurrentApplicationMaxRetries = 1
            };
            var integration = new FormsApplicationListener(options);
            var mockHub = new MockHub();
            Action<Application> badListener = (_) => throw null;
            integration.AddListener(badListener);

            //Act
            integration.Invoke();

            await Task.Delay(options.GetCurrentApplicationDelay + 100);

            //Assert

        }

    }
}
