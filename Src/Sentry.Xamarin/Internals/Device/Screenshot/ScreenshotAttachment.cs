namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotAttachment : Attachment
    {
        public ScreenshotAttachment(IAttachmentContent content)
        : this(
        AttachmentType.Default,
        content,
        "screenshot",
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
