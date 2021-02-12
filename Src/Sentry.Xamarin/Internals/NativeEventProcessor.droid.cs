using System;
using Android.App;
using Android.Content;
using Android.OS;
using Sentry.Extensibility;
using Sentry.Xamarin.Extensions;

namespace Sentry.Xamarin.Internals
{
    internal class NativeEventProcessor : ISentryEventProcessor
    {
        private Lazy<AndroidContext> _androidContext = new Lazy<AndroidContext>(() => new AndroidContext());
        private SentryOptions _options;
        private volatile bool _androidContextLoaded = true;

        /// <summary>
        /// The NativeEventProcessor contructor.
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public NativeEventProcessor(SentryOptions options) => _options = options;
        private class AndroidContext
        {
            internal long? MemorySize { get; }
            internal string CpuModel { get; }
            private string GetCpuModel()
            {
                var modelKey = "Hardware";
                try
                {
                    string model = null;
                    var reader = new Java.IO.RandomAccessFile("/proc/cpuinfo", "r");
                    do
                    {
                        model = reader.ReadLine();
                    } while (model != null && model.Contains(modelKey) == false);
                    reader.Close();
                    if (model?.Contains(modelKey) == true)
                    {
                        return model.Replace($"{modelKey}\t:", "");
                    }
                }
                catch { }
                return $"{Build.Board}";
            }

            internal long? GetAvaliableRom()
            {
                try
                {
                    var statfs = new StatFs(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
                    return statfs.AvailableBytes;
                }
                catch { }
                return null;
            }

            internal long? GetFreeMemory()
                => GetMemoryInfo()?.AvailMem;

            internal ActivityManager.MemoryInfo GetMemoryInfo()
            {
                var activityManager = (ActivityManager)global::Xamarin.Essentials.Platform.AppContext.GetSystemService(Context.ActivityService);
                if (activityManager != null)
                {
                    var memoryManager = new ActivityManager.MemoryInfo();
                    activityManager.GetMemoryInfo(memoryManager);
                    return memoryManager;
                }
                return null;
            }

            internal AndroidContext()
            {
                CpuModel = GetCpuModel().FilterUnknownOrEmpty();
                var activityManager = (ActivityManager)global::Xamarin.Essentials.Platform.AppContext.GetSystemService(Context.ActivityService);
                if (activityManager != null)
                {
                    MemorySize = GetMemoryInfo()?.TotalMem;
                }
            }
        }

        /// <summary>
        /// Applies the Android Tags and Context.
        /// </summary>
        /// <param name="event">The event to be applied.</param>
        /// <returns>The Sentry event.</returns>
        public SentryEvent Process(SentryEvent @event)
        {
            if (_androidContextLoaded)
            {
                try
                {
                    var androidContext = _androidContext.Value;
                    @event.Contexts.Device.MemorySize = _androidContext.Value.MemorySize;
                    @event.Contexts.Device.FreeMemory = _androidContext.Value.GetFreeMemory();
                    @event.Contexts.Device.StorageSize = _androidContext.Value.GetAvaliableRom();
                    if (_androidContext.Value.CpuModel != null)
                    {
                        @event.SetTag("cpu.model", _androidContext.Value.CpuModel);
                    }
                }
                catch (Exception ex)
                {
                    _options.DiagnosticLogger?.Log(SentryLevel.Error, "Failed to add AndroidEventProcessor into event.", ex);
                    //In case of any failure, this process function will be disabled to avoid throwing exceptions for future events.
                    _androidContextLoaded = false;
                }
            }
            else
            {
                _options.DiagnosticLogger?.Log(SentryLevel.Debug, "AndroidEventProcessor disabled due to previous error.");
            }
            return @event;
        }
    }
}