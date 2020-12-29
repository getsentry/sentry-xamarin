using Sentry.Xamarin.Forms.Internals;
using Xunit;

namespace Sentry.Xamarin.Forms.Tests.UWP
{
    public class SentryXamarinFormsIntegrationTests
    {        
        [Fact]
        public void RegisterNativeIntegrations_NativeIntegrationEnabled_NativeIntegrationRegistered()
        {
            //Arrange
            var options = new SentryOptions();
            var xamarinOptions = new SentryXamarinOptions();
            var integration = new SentryXamarinFormsIntegration(xamarinOptions);

            //Act
            integration.RegisterNativeIntegrations(default, options, xamarinOptions);
            integration.Nativeintegration.Unregister();

            //Assert
            Assert.NotNull(integration.Nativeintegration);
        }

        [Fact]
        public void RegisterNativeIntegrations_NativeIntegrationDisabled_NativeIntegrationNotSet()
        {
            //Arrange
            var options = new SentryOptions();
            var xamarinOptions = new SentryXamarinOptions()
            {
                NativeIntegrationEnabled = false
            };
            var integration = new SentryXamarinFormsIntegration(xamarinOptions);

            //Act
            integration.RegisterNativeIntegrations(default, options, xamarinOptions);

            //Assert
            Assert.Null(integration.Nativeintegration);
        }
    }
}
