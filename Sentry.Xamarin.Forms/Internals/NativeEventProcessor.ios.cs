using Foundation;
using Sentry;
using Sentry.Extensibility;
using Sentry.Protocol;
using System;

namespace Sentry.Xamarin.Forms.Internals
{
    /// <summary>
    /// An event processor that populates the Event with iOS specific Tags.
    /// </summary>
    public partial class NativeEventProcessor : ISentryEventProcessor
    {
        private Lazy<IosContext> _IosContext = new Lazy<IosContext>(() => new IosContext());
        private SentryOptions _options;
        private volatile bool _IosContextLoaded = true;

        /// <summary>
        /// The NativeEventProcessor contructor.
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public NativeEventProcessor(SentryOptions options) => _options = options;
        private class IosContext
        {
            internal long? MemorySize { get; }
            internal string Device { get; }

            internal long GetStorageSize()
                => (long)NSFileManager.DefaultManager.GetFileSystemAttributes(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).FreeSize;

            internal IosContext()
            {
                MemorySize = (long)NSProcessInfo.ProcessInfo.PhysicalMemory;
                var model = new DeviceModel();
                Device = model.GetModel();
            }
        }

        /// <summary>
        /// Applies the iOS Tags and Context.
        /// </summary>
        /// <param name="event">The event to be applied.</param>
        /// <returns>The Sentry event.</returns>
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