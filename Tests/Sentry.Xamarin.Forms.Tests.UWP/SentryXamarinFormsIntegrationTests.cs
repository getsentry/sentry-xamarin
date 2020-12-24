using Sentry.Xamarin.Forms.Internals;
using Xunit;

namespace Sentry.Xamarin.Forms.Tests.UWP
{
    public class SentryXamarinFormsIntegrationTests
    {
        [Fact]
        public void ConfigureSentryOptions_ReleaseSetIfNotInformed()
        {
            //Arrange
            var options = new SentryOptions();
            options.Release = null;
            var integration = new SentryXamarinFormsIntegration();

            //Act
            integration.ConfigureSentryOptions(options);

            //Assert
            Assert.NotNull(options.Release);
        }

        [Fact]
        public void ConfigureSentryOptions_ReleaseNotSetIfInformed()
        {
            //Arrange
            var options = new SentryOptions();
            options.Release = "myrelease@1.1";
            var integration = new SentryXamarinFormsIntegration();

            //Act
            integration.ConfigureSentryOptions(options);

            //Assert
            Assert.Equal(options.Release, options.Release);
        }
        
        [Fact]
        public void RegisterNativeIntegrations_NativeIntegrationEnabled_NativeIntegrationRegistered()
        {
            //Arrange
            var options = new SentryOptions();
            var xamarinOptions = new SentryXamarinOptions();
            var integration = new SentryXamarinFormsIntegration();

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
            var integration = new SentryXamarinFormsIntegration();

            //Act
            integration.RegisterNativeIntegrations(default, options, xamarinOptions);

            //Assert
            Assert.Null(integration.Nativeintegration);
        }
    }
}
