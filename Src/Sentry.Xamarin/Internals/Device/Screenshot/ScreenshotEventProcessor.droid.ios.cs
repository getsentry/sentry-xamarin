using Sentry.Extensibility;
using Xamarin.Essentials;
using System.IO;
using System;

namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotEventProcessor : ISentryEventProcessor
    {
        private SentryXamarinOptions _options { get; }

        public ScreenshotEventProcessor(SentryXamarinOptions options) => _options = options;

        public SentryEvent? Process(SentryEvent @event)
        {
            try
            {
                if (_options.SessionLogger?.IsBackground() == false)
                {
                    var stream = Capture();
                    if (stream != null)
                    {
                        SentrySdk.ConfigureScope(s => s.AddAttachment(new ScreenshotAttachment(stream)));
                        //@event.Contexts["Sentry::Screenshot"] = new ScreenshotAttachment(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                _options.DiagnosticLogger?.LogError("Failed to capture a screenshot", ex);
            }
            return @event;
        }


        internal Stream Capture()
        {
            var screenStream = global::Xamarin.Essentials.Screenshot.CaptureAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return screenStream.OpenReadAsync(ScreenshotFormat.Jpeg).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
