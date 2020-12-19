using Sentry.Xamarin.Forms.Internals;
using Xunit;

namespace Sentry.Xamarin.Forms.Tests.UWP.Internals
{
    public class FormsEventProcessorTests
    {
        [Fact]
        public void Register_ValidEvent_EventWithOperationalSystemInfo()
        {

            //Arrange
            var eventProcessor = new XamarinFormsEventProcessor(new SentryOptions());
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
            var eventProcessor = new XamarinFormsEventProcessor(new SentryOptions());
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
            var eventProcessor = new XamarinFormsEventProcessor(default);

            //Act
            var @event = eventProcessor.Process(new SentryEvent());

            //Assert
            Assert.Equal("UWP", @event.Contexts.OperatingSystem.Name);
        }

    }
}
