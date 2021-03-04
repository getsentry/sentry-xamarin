using Sentry.Xamarin.Forms.Testing.Mock;
using Sentry.Xamarin.Internals;
using System;
using Xunit;

namespace Sentry.Xamarin.Forms.UWP.Tests.Internals
{
    public class NativeIntegrationTests
    {
        /// <summary>
        /// Mock is not supported by .NET Native so we have to manually Mock the Hub.
        /// </summary>

        [Fact]
        public void Unregister_DoesntCrashifNotRegistered()
        {
            //Arrange
            var integration = new NativeIntegration(new SentryXamarinOptions());

            //Act
            integration.Unregister();
        }

        [Fact]
        public void Handle_RegisterUnhandleException()
        {
            //Arrange
            var integration = new NativeIntegration(new SentryXamarinOptions());
            var hub = new MockHub();

            integration.Register(hub, new SentryOptions());

            //Act
            try
            {
                integration.Handle(new Exception());
            }
            finally
            {
                integration.Unregister();
            }

            //Assert
            Assert.Equal(1, hub.CaptureEventCount);
            Assert.Equal(1, hub.FlushAsyncCount);
        }
    }
}
