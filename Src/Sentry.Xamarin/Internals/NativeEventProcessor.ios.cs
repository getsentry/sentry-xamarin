using Foundation;
using Sentry.Extensibility;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sentry.Xamarin.Internals
{
    internal class NativeEventProcessor : ISentryEventProcessor
    {
        private Lazy<IosContext> _IosContext = new Lazy<IosContext>(() => new IosContext());
        private SentryOptions _options;
        private NativeStackTraceFactory _nativeStack;
        private string _regexNativeException = "(.*Name:\\s(?<Name>.*)Reason:\\s(?<Reason>.*))";
        private volatile bool _IosContextLoaded = true;

        public NativeEventProcessor(SentryOptions options)
        {
            _options = options;
            _nativeStack = new NativeStackTraceFactory(_options);
        }
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

        public SentryEvent Process(SentryEvent @event)
        {
            if (_IosContextLoaded)
            {
                try
                {
                    var IosContext = _IosContext.Value;
                    @event.Contexts.Device.MemorySize = _IosContext.Value.MemorySize;
                    @event.Contexts.Device.StorageSize = _IosContext.Value.GetStorageSize();

                    foreach (var exception in @event.SentryExceptions)
                    {
                        if (_nativeStack.IsNativeException(exception.Value))
                        {
                            var nativeStackTrace = exception.Value.Split('\n');
                            var match = Regex.Match(nativeStackTrace[0], _regexNativeException);
                            if (match.Success)
                            {
                                exception.Value = match.Groups["Reason"].Value;
                                exception.Type = match.Groups["Name"].Value;
                                exception.Stacktrace = _nativeStack.CreateNativeStackTrace(exception.Stacktrace, nativeStackTrace.Skip(1));
                            }
                        }
                    }
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
                _options.DiagnosticLogger?.Log(SentryLevel.Debug, "iOSEventProcessor disabled due to previous error.");
            }
            return @event;
        }
    }
}