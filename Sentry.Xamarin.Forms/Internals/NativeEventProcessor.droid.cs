using System;
using System.Text.RegularExpressions;
using Android.OS;
using Sentry.Extensibility;
using Sentry.Xamarin.Forms.Extensions;

namespace Sentry.Xamarin.Forms.Internals
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
            private long? GetAvaliableMemory()
            {
                try
                {
                    var reader = new Java.IO.RandomAccessFile("/proc/meminfo", "r");
                    var memory = reader.ReadLine();
                    reader.Close();
                    memory = Regex.Match(memory, @"\d+").Value;
                    return long.Parse(memory) * 1024; //convert KB to Bytes
                }
                catch { }
                return null;
            }

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

            internal AndroidContext()
            {
                MemorySize = GetAvaliableMemory();
                CpuModel = GetCpuModel().FilterUnknownOrEmpty();
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
                    @event.Contexts.Device.StorageSize = _androidContext.Value.GetAvaliableRom();
                    if(_androidContext.Value.CpuModel != null)
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
                _options.DiagnosticLogger.Log(SentryLevel.Debug, "AndroidEventProcessor disabled due to previous error.");
            }
            return @event;
        }
    }
}