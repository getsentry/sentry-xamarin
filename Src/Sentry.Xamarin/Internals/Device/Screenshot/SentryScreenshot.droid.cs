using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Views;
using Xamarin.Essentials;

namespace Sentry.Internals.Device.Screenshot
{
    internal static class SentryScreenshot
    {
        internal static Task<SentryScreenshotResult> CaptureAsync()
        {
            if (Platform.CurrentActivity?.WindowManager?.DefaultDisplay?.Flags.HasFlag(DisplayFlags.Secure) == true)
                throw new UnauthorizedAccessException("Unable to take a screenshot of a secure window.");

            var view = Platform.CurrentActivity?.Window?.DecorView?.RootView;
            if (view == null)
                throw new NullReferenceException("Unable to find the main window.");

            var bitmap = Bitmap.CreateBitmap(view.Width, view.Height, Bitmap.Config.Argb8888);

            using (var canvas = new Canvas(bitmap))
            {
                var drawable = view.Background;
                if (drawable != null)
                    drawable.Draw(canvas);
                else
                    canvas.DrawColor(Color.White);

                view.Draw(canvas);
            }

            var result = new SentryScreenshotResult(bitmap);

            return Task.FromResult(result);
        }
    }

    internal class SentryScreenshotResult
    {
        private readonly Bitmap _bmp;
        
        internal SentryScreenshotResult(Bitmap bmp) => _bmp = bmp;

        internal async Task<Stream> OpenReadAsync()
        {
            var stream = new MemoryStream();

            await _bmp.CompressAsync(Bitmap.CompressFormat.Jpeg, 50, stream).ConfigureAwait(false);
            stream.Position = 0;

            return stream;
        }
    }
}
