using System.IO;
using System.Threading.Tasks;
using UIKit;


namespace Sentry.Internals.Device.Screenshot
{
    internal static class SentryScreenshot
    {
        internal static Task<SentryScreenshotResult> CaptureAsync()
        {
            var img = UIScreen.MainScreen.Capture();
            var result = new SentryScreenshotResult(img);

            return Task.FromResult(result);
        }
    }

    internal class SentryScreenshotResult
    {
        readonly UIImage uiImage;

        internal SentryScreenshotResult(UIImage image)
        {
            uiImage = image;
        }

        internal Task<Stream> OpenReadAsync()
        {
            var data = uiImage.AsJPEG(0.5f);

            return Task.FromResult(data.AsStream());
        }
    }

}
