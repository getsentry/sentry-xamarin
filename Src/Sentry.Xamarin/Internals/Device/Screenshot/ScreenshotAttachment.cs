using System.IO;

namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotAttachment : Attachment
    {
        public ScreenshotAttachment(Stream image)
        : this(
        AttachmentType.Default,
        new ScreenshotAttachmentContent(image),
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
