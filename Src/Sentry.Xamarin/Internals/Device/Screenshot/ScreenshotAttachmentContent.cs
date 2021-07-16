using System.IO;

namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotAttachmentContent : IAttachmentContent
    {
        private byte[] _data { get; }

        /// <summary>
        /// The content ctor
        /// </summary>
        /// <param name="stream">A Ready once stream containing the image data.</param>
        public ScreenshotAttachmentContent(Stream stream)
        {
            _data = new byte[stream.Length];
            stream.Read(_data, 0, _data.Length);
        }
        /// <summary>
        /// Get a stream from the attachment data.
        /// </summary>
        /// <returns>A Memory stream containing the data.</returns>
        public Stream GetStream()
            => new MemoryStream(_data);
    }
}