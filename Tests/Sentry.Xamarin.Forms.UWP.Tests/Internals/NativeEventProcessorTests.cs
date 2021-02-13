using Sentry.Xamarin.Internals;
using Xunit;

namespace Sentry.Xamarin.Forms.UWP.Tests.Internals
{
    public class NativeEventProcessorTests
    {
        [Fact]
        public void Register_ValidEvent_ContainFormatedWindowsNAme()
        {
            //Arrange
            var eventProcessor = new NativeEventProcessor(new SentryOptions());
            var @event = new SentryEvent();

            //Act
            @event = eventProcessor.Process(@event);

            //Act
            Assert.Equal("Windows", @event.Contexts.OperatingSystem.Name);
        }

        [Fact]
        public void Register_ValidEvent_ContainDeviceNameAndFamily()
        {
            //Arrange
            var eventProcessor = new NativeEventProcessor(new SentryOptions());
            var @event = new SentryEvent();

            //Act
            @event = eventProcessor.Process(@event);

            //Act
            Assert.NotNull(@event.Contexts.Device.Family);
            Assert.NotNull(@event.Contexts.Device.Name);
        }

        [Fact]
        public void Register_ValidEvent_ContainOperationalSystemNameAndVersion()
        {
            //Arrange
            var eventProcessor = new NativeEventProcessor(new SentryOptions());
            var @event = new SentryEvent();

            //Act
            @event = eventProcessor.Process(@event);

            //Act
            Assert.NotNull(@event.Contexts.OperatingSystem.Name);
            Assert.NotNull(@event.Contexts.OperatingSystem.Version);
        }
    }
}
