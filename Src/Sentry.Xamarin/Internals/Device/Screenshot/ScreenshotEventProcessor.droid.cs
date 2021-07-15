using Sentry.Extensibility;
using Android.Graphics;
using Android.Views;
using Xamarin.Essentials;
using Android.App;
using System.IO;
using System.Threading.Tasks;

namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotEventProcessor : ISentryEventProcessor
    {
        public SentryEvent? Process(SentryEvent @event)
        {
            var image = Capture();
            if (image != null)
            {
                SentrySdk.ConfigureScope(s =>
                {
                    s.AddAttachment(new ScreenshotAttachment(image));
                });
            }
            return @event;
        }

        private SentryOptions _options { get; }

        private Activity? _currentActivity => Platform.CurrentActivity;

        public ScreenshotEventProcessor(SentryOptions options) => _options = options;

        internal byte[] Capture()
        {
            
            if (_currentActivity?.WindowManager.DefaultDisplay.Flags.HasFlag(DisplayFlags.Secure) == true)
            {
                _options.DiagnosticLogger?.LogWarning("Unable to take a screenshot of a secure window.");
            }

            var view = _currentActivity?.Window?.DecorView?.RootView;
            if (view == null)
            {
                _options.DiagnosticLogger?.LogWarning("Unable to find the main window.");
            }

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

            //Perhaps opt if this code will compress or not since it could be performance heavy.
            byte[] jpegData;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 30, stream);
                jpegData = stream.ToArray();
            }

            return jpegData;
        }
    }
}
