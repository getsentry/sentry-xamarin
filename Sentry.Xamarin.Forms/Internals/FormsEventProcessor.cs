using Sentry.Xamarin.Forms.Extensions;
using Sentry.Extensibility;
using Sentry.Protocol;
using System;
using Xamarin.Essentials;

namespace Sentry.Xamarin.Forms.Internals
{
    public partial class XamarinFormsEventProcessor : ISentryEventProcessor
    {
        private Lazy<FormsContext> _formsContext = new Lazy<FormsContext>(() => new FormsContext());
        private SentryOptions _options;
        private volatile bool _formsContextLoaded = true;
        private volatile bool _ConnectivityStatusAllowed = true;

        private class FormsContext
        {
            public string Manufacturer { get; }
            public string Model { get; }
            public string Platform { get; }
            public string PlatformVersion { get; }
            public bool IsEmulator { get; }
            public string Type { get; }
            public float ScreenDensity { get; }
            public string ScreenResolution { get; }

            public FormsContext()
            {
                Manufacturer = DeviceInfo.Manufacturer.FilterUnknownOrEmpty();
                Model = DeviceInfo.Model.FilterUnknownOrEmpty();
                Platform = DeviceInfo.Platform.ToString().ToUpper();
                PlatformVersion = DeviceInfo.VersionString;
                IsEmulator = DeviceInfo.DeviceType != DeviceType.Physical;
                Type = DeviceInfo.Idiom.ToString();
                try
                {
                    ScreenResolution = $"{DeviceDisplay.MainDisplayInfo.Height}x{DeviceDisplay.MainDisplayInfo.Width}";
                    ScreenDensity = (float)DeviceDisplay.MainDisplayInfo.Density;
                }
                //xUnit Throws Exception because no Screen is present.
                catch { }
            }
        }

        public XamarinFormsEventProcessor(SentryOptions options) => _options = options;

        public SentryEvent Process(SentryEvent @event)
        {
            if (_formsContextLoaded)
            {
                try
                {
                    var formsContext = _formsContext.Value;
                    @event.Contexts.Device.Simulator = formsContext.IsEmulator;
                    @event.Contexts.Device.Manufacturer = formsContext.Manufacturer;
                    @event.Contexts.Device.Model = formsContext.Model;
                    @event.Contexts.OperatingSystem.Name = formsContext.Platform;
                    @event.Contexts.OperatingSystem.Version = formsContext.PlatformVersion;
                    @event.Contexts.Device.ScreenResolution = formsContext.ScreenResolution;
                    @event.Contexts.Device.ScreenDensity = formsContext.ScreenDensity;
                    if (_ConnectivityStatusAllowed)
                    {
                        @event.Contexts.Device.IsOnline = Connectivity.NetworkAccess == NetworkAccess.Internet;
                    }
                }
                catch(PermissionException pe)
                {
                    _options.DiagnosticLogger?.Log(SentryLevel.Error, "Failed to capture ConnectivityStatus FormsEventProcessor into event.", pe);
                    _ConnectivityStatusAllowed = false;
                }
                catch (Exception ex)
                {
                    _options.DiagnosticLogger?.Log(SentryLevel.Error, "Failed to add FormsEventProcessor into event.", ex);
                    //In case of any failure, this process function will be disabled to avoid throwing exceptions for future events.
                    _formsContextLoaded = false;
                }
            }
            else
            {
                _options.DiagnosticLogger.Log(SentryLevel.Debug, "FormsEventProcessor disabled due to previous error.");
            }
            return @event;
        }
    }
}