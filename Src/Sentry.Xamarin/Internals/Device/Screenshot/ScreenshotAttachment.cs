namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotAttachment : SentryAttachment
    {
        public ScreenshotAttachment(IAttachmentContent content)
        : this(
        AttachmentType.Default,
        content,
        "screenshot.jpg",
        "image/jpeg")
        {
        }

        private ScreenshotAttachment(
            AttachmentType type,
            IAttachmentContent content,
            string fileName,
            string? contentType)
            : base(type, content, fileName, contentType)
        {
        }
    }
}
