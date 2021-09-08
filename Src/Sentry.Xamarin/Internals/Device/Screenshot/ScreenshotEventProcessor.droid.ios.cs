using Sentry.Extensibility;
using System.IO;
using System;

namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotEventProcessor : ISentryEventProcessor, IAttachmentContent
    {
        private SentryXamarinOptions _options { get; }
        private bool _attachmentWasRead { get; set; }

        /// <summary>
        /// Used to flag if the screenshot to be captured should be skipped or not.
        /// </summary>
        private bool _skipScreenshotCapture { get; set; }

        public ScreenshotEventProcessor(SentryXamarinOptions options) => _options = options;

        public SentryEvent? Process(SentryEvent @event)
        {
            // Small logic to re-attach the screenshot attachment in case of user clearing the attachments.
            if (_attachmentWasRead is false)
            {
                SentrySdk.ConfigureScope(s => s.AddAttachment(new ScreenshotAttachment(this)));
            }

            if (@event.Level < SentryLevel.Error)
            {
                _skipScreenshotCapture = true;
            }
            _attachmentWasRead = false;
            return @event;
        }

        private Stream Capture()
        {
            var screenStream = SentryScreenshot.CaptureAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return screenStream.OpenReadAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public Stream GetStream()
        {
            _attachmentWasRead = true;
            try
            {
                if (_options.SessionLogger.IsBackground() is false && _skipScreenshotCapture is false)
                {
                    return Capture();
                }

                if (_skipScreenshotCapture is false)
                {
                    _options.DiagnosticLogger?.LogWarning("Skipping screenshot because app is in background.");
                }
                else
                {
                    _skipScreenshotCapture = false;
                }
            }
            catch (Exception ex)
            {
                _options.DiagnosticLogger?.LogError("Failed to capture a screenshot.", ex);
            }
            return new MemoryStream();
        }
    }
}
