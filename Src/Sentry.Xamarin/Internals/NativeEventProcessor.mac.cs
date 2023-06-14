using Foundation;
using Sentry.Extensibility;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sentry.Xamarin.Internals
{
    internal class NativeEventProcessor : ISentryEventProcessor
    {
        private Lazy<MacContext> _MacContext = new Lazy<MacContext>(() => new MacContext());
        private SentryXamarinOptions _options;
        private volatile bool _MacContextLoaded = true;

        public NativeEventProcessor(SentryXamarinOptions options) => _options = options;
        private class MacContext
        {
            internal long? MemorySize { get; }
            internal string Device { get; }
            internal long GetStorageSize()
                => (long)NSFileManager.DefaultManager.GetFileSystemAttributes(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).FreeSize;

            internal MacContext()
            {
                MemorySize = (long)NSProcessInfo.ProcessInfo.PhysicalMemory;
                var model = new DeviceModel();
                Device = model.Machine;
            }
        }

        public SentryEvent Process(SentryEvent @event)
        {
            if (_MacContextLoaded)
            {
                try
                {
                    var MacContext = _MacContext.Value;
                    @event.Contexts.Device.MemorySize = _MacContext.Value.MemorySize;
                    @event.Contexts.Device.StorageSize = _MacContext.Value.GetStorageSize();
                }
                catch (Exception ex)
                {
                    _options.DiagnosticLogger?.Log(SentryLevel.Error, "Failed to add MacEventProcessor into event.", ex);
                    //In case of any failure, this process function will be disabled to avoid throwing exceptions for future events.
                    _MacContextLoaded = false;
                }
            }
            else
            {
                _options.DiagnosticLogger?.Log(SentryLevel.Debug, "MacEventProcessor disabled due to previous error.");
            }
            return @event;
        }
    }
}