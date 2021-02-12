using Sentry.Xamarin.Internals;
using Xunit;

namespace Sentry.Xamarin.Forms.UWP.Tests.Internals
{
    public class FormsEventProcessorTests
    {
        [Fact]
        public void Register_ValidEvent_EventWithOperationalSystemInfo()
        {

            //Arrange
            var eventProcessor = new XamarinEventProcessor(new SentryXamarinOptions());
            var @event = new SentryEvent();

            //Act
            @event = eventProcessor.Process(@event);

            //Assert
            Assert.NotNull(@event.Contexts.OperatingSystem.Name);
            Assert.NotNull(@event.Contexts.OperatingSystem.Version);
        }

        [Fact]
        public void Register_ValidEvent_EventWithDeviceInfoSet()
        {

            //Arrange
            var eventProcessor = new XamarinEventProcessor(new SentryXamarinOptions());
            var @event = new SentryEvent();

            //Act
            @event = eventProcessor.Process(@event);

            //Assert
            Assert.NotNull(@event.Contexts.Device.Simulator);
            Assert.NotNull(@event.Contexts.Device.Manufacturer);
            Assert.NotNull(@event.Contexts.Device.Model);
        }

        [Fact]
        public void Register_ValidEvent_OSNameIsUWP()
        {
            //Arrange
            var eventProcessor = new XamarinEventProcessor(new SentryXamarinOptions());

            //Act
            var @event = eventProcessor.Process(new SentryEvent());

            //Assert
            Assert.Equal("UWP", @event.Contexts.OperatingSystem.Name);
        }

    }
}
