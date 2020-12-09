using Sentry;
using Sentry.Extensibility;
using Sentry.Protocol;
using System;
using System.Text.RegularExpressions;
using Android.OS;
using ContribSentry.Forms.Extensions;

namespace ContribSentry.Forms.Internals
{
    public partial class NativeEventProcessor : ISentryEventProcessor
    {
        private Lazy<AndroidContext> _androidContext = new Lazy<AndroidContext>(() => new AndroidContext());
        private SentryOptions _options;
        private volatile bool _androidContextLoaded = true;

        public NativeEventProcessor(SentryOptions options) => _options = options;
        private class AndroidContext
        {
            public long? MemorySize { get; }

            public string CpuModel { get; }
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

            public long? GetAvaliableRom()
            {
                try
                {
                    var statfs = new StatFs(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
                    return statfs.AvailableBytes;
                }
                catch { }
                return null;
            }

            public AndroidContext()
            {
                MemorySize = GetAvaliableMemory();
                CpuModel = GetCpuModel().FilterUnknown();
            }
        }

        public SentryEvent Process(SentryEvent @event)
        {
            if (_androidContextLoaded)
            {
                try
                {
                    Android.
                    var androidContext = _androidContext.Value;
                    @event.Contexts.Device.MemorySize = _androidContext.Value.MemorySize;
                    @event.Contexts.Device.StorageSize = _androidContext.Value.GetAvaliableRom();
                    if(_androidContext.Value.CpuModel is string cpuModel)
                    {
                        @event.SetTag("cpu.model", cpuModel);
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