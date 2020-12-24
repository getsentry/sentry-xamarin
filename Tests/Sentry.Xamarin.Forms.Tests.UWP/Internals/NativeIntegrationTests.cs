using Moq;
using Sentry.Xamarin.Forms.Internals;
using System;
using Xunit;

namespace Sentry.Xamarin.Forms.Tests.UWP.Internals
{
    public class NativeIntegrationTests
    {
        [Fact]
        public void Unregister_DoesntCrashifNotRegistered()
        {
            //Arrange
            var integration = new NativeIntegration(new SentryXamarinOptions());

            //Act
            integration.Unregister();
        }

        [Fact]
        public void OnSleep_SleepBreadcrumb()
        {
            //Arrange
            var integration = new NativeIntegration(new SentryXamarinOptions());
            var hub = new Mock<IHub>();
            hub.Setup(x => x.IsEnabled).Returns(true);
            var hubObj = hub.Object;

            var exception = new Exception();
            integration.Register(hubObj, new SentryOptions());

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
            hub.Verify(x => x.CaptureEvent(It.IsAny<SentryEvent>(), null), Times.Once());
            hub.Verify(x => x.FlushAsync(It.IsAny<TimeSpan>()), Times.Once());
        }
    }
}
