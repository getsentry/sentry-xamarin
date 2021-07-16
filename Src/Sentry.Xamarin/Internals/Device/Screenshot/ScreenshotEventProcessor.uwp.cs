#pragma warning disable CS0219 // Variable is assigned but its value is never used
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

using Sentry.Extensibility;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Xamarin.Essentials;

namespace Sentry.Internals.Device.Screenshot
{
    internal class ScreenshotEventProcessor : ISentryEventProcessor
    {
        public SentryEvent? Process(SentryEvent @event)
        {
            try
            {
                if (Capture() is { } stream)
                {
                    //@event.Contexts["Sentry::Screenshot"] = new ScreenshotAttachment(stream);
                    SentrySdk.ConfigureScope(s => s.AddAttachment(new ScreenshotAttachment(stream)));
                }
            }
            catch (Exception ex)
            {
                _ = ex;
            }
            return @event;
        }

        private SentryXamarinOptions _options { get; }

        public ScreenshotEventProcessor(SentryXamarinOptions options) => _options = options;

        //BURN THIS CODE ☺
        public Stream? Capture()
        {
            var @lock2 = new ManualResetEvent(false);
            var task2 = Task.Run<ScreenshotResult?>(async () =>
            {
                var task = new TaskCompletionSource<object>();
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        //var screenshot = await global::Xamarin.Essentials.Screenshot.CaptureAsync().ConfigureAwait(false);
                        throw new Exception("eee");
                        //task.SetResult(screenshot);
                    }
                    catch (Exception ex)
                    {
                        task.SetResult(ex);
                    }
                });
                await task.Task.ConfigureAwait(false);
                if (task.Task.Result is Exception ex)
                {
                    _options.DiagnosticLogger?.LogError("Failed to capture a screenshot", ex);
                }
                else if (task.Task.Result is ScreenshotResult screenshot)
                {
                    @lock2.Set();
                    return screenshot;
                }
                @lock2.Set();
                return null;
            });

            @lock2.WaitOne(TimeSpan.FromSeconds(5));

            if (task2.Result is { } screenshot)
            {
                return screenshot.OpenReadAsync(ScreenshotFormat.Jpeg).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return null;
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning restore CS0219 // Variable is assigned but its value is never used






































