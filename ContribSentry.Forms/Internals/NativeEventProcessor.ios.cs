using Foundation;
using Sentry;
using Sentry.Extensibility;
using Sentry.Protocol;
using System;

namespace ContribSentry.Forms.Internals
{
    public class NativeEventProcessor : ISentryEventProcessor
    {
        private Lazy<IosContext> _IosContext = new Lazy<IosContext>(() => new IosContext());
        private SentryOptions _options;
        private volatile bool _IosContextLoaded = true;
        public NativeEventProcessor(SentryOptions options) => _options = options;
        private class IosContext
        {
            public long? MemorySize { get; }

            public long GetStorageSize()
                => (long)NSFileManager.DefaultManager.GetFileSystemAttributes(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).FreeSize;

            public IosContext()
            {
                MemorySize = (long)NSProcessInfo.ProcessInfo.PhysicalMemory;
            }
        }

        public SentryEvent Process(SentryEvent @event)
        {
            if (_IosContextLoaded)
            {
                try
                {
                    var IosContext = _IosContext.Value;
                    @event.Contexts.Device.MemorySize = _IosContext.Value.MemorySize;
                    @event.Contexts.Device.StorageSize = _IosContext.Value.GetStorageSize();
                }
                catch (Exception ex)
                {
                    _options.DiagnosticLogger?.Log(SentryLevel.Error, "Failed to add iOSEventProcessor into event.", ex);
                    //In case of any failure, this process function will be disabled to avoid throwing exceptions for future events.
                    _IosContextLoaded = false;
                }
            }
            else
            {
                _options.DiagnosticLogger.Log(SentryLevel.Debug, "iOSEventProcessor disabled due to previous error.");
            }
            return @event;
        }
    }
}