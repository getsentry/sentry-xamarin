using System.IO;
using Xamarin.Essentials;
using static Xamarin.Essentials.Screenshot;

namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotAttachmentContent : IAttachmentContent
    {
        public Stream GetStream()
        {
                var screenStream = CaptureAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                return screenStream.OpenReadAsync(ScreenshotFormat.Jpeg).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
