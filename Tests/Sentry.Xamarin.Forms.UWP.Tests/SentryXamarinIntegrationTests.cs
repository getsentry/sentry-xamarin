using Xunit;

namespace Sentry.Xamarin.Forms.UWP.Tests
{
    public class SentryXamarinIntegrationTests
    {
        [Fact]
        public void RegisterNativeIntegrations_NativeIntegrationEnabled_NativeIntegrationRegistered()
        {
            //Arrange
            var xamarinOptions = new SentryXamarinOptions()
            {
                NativeIntegrationEnabled = true
            };

            //Act
            var registered = xamarinOptions.RegisterNativeIntegrations();

            //Assert
            Assert.True(registered);
        }

        [Fact]
        public void RegisterNativeIntegrations_NativeIntegrationDisabled_NativeIntegrationNotSet()
        {
            //Arrange
            var xamarinOptions = new SentryXamarinOptions()
            {
                NativeIntegrationEnabled = false
            };

            //Act
            var registered = xamarinOptions.RegisterNativeIntegrations();

            //Assert
            Assert.False(registered);
        }
    }
}
