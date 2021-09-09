using System;
using System.IO;
using Sentry.Extensibility;
using Xamarin.Essentials;
using static Xamarin.Essentials.Screenshot;

namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotAttachmentContent : IAttachmentContent
    {
        private SentryXamarinOptions _options { get; }
        public ScreenshotAttachmentContent(SentryXamarinOptions options) => _options = options;

        public Stream GetStream()
        {
            try
            {
                var screenStream = CaptureAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                return screenStream.OpenReadAsync(ScreenshotFormat.Jpeg).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _options.DiagnosticLogger?.LogError("Failed to capture a screenshot.", ex);
            }
            return new MemoryStream();
        }
    }
}
