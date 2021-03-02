using Sentry.Xamarin.Forms.Internals;
using Sentry.Xamarin.Forms.Testing.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sentry.Xamarin.Forms.UWP.Tests.Internals
{
    public class SentryXamarinFormsIntegrationTests
    {
        [Fact]
        public async Task Register_FormsNotInitialized_IgnoreIntegration()
        {
            //Assert
            var integration = new SentryXamarinFormsIntegration();
            var mockDiagnostic = new MockDiagnosticLogger(SentryLevel.Debug);
            var options = new SentryXamarinOptions()
            {
                Debug = true,
                DiagnosticLogger = mockDiagnostic,
                GetCurrentApplicationDelay = 1,
                GetCurrentApplicationMaxRetries = 1
            };
            var mockHub = new MockHub();
            integration.RegisterXamarinOptions(options);

            //Act
            integration.Register(mockHub, options);
            SentryXamarinFormsIntegration.Instance = null;
            await Task.Delay(options.GetCurrentApplicationDelay + 100);

            //Assert
            Assert.True(mockDiagnostic.Contains("Sentry.Xamarin.Forms timeout for tracking Application.Current. Navigation tracking is going to be disabled"));
        }

        [Fact]
        public async Task Register_FormsNotInitializedAndWithoutLogger_IgnoreIntegration()
        {
            //Assert
            var integration = new SentryXamarinFormsIntegration();
            var options = new SentryXamarinOptions()
            {
                GetCurrentApplicationDelay = 1,
                GetCurrentApplicationMaxRetries = 1
            };
            integration.RegisterXamarinOptions(options);
            var mockHub = new MockHub();

            //Act
            try
            {

                integration.Register(mockHub, options);
                SentryXamarinFormsIntegration.Instance = null;
                await Task.Delay(options.GetCurrentApplicationDelay + 15);
            }
            catch(Exception)
            {
                throw;
            }

            //Assert
        }
    }
}
