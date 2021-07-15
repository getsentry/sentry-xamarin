using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotAttachmentContent : IAttachmentContent
    {
        private MemoryStream _stream { get; }

        public ScreenshotAttachmentContent(byte[] image) => _stream = new MemoryStream(image);

        public Stream GetStream()
        {
            var stream = new MemoryStream();
            _stream.CopyTo(stream);
            _stream.Seek(0, SeekOrigin.Begin);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}