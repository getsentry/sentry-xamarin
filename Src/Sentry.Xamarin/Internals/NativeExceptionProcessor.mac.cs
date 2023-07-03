using Foundation;
using Sentry.Extensibility;
using System;
using System.Linq;

namespace Sentry.Xamarin.Internals
{
    internal class NativeExceptionProcessor : ISentryEventExceptionProcessor
    {
        private NativeStackTraceFactory _nativeStack;

        public NativeExceptionProcessor(SentryXamarinOptions options)
        {
            _nativeStack = new NativeStackTraceFactory(options);
        }

        public void Process(Exception exception, SentryEvent sentryEvent)
        {
            if (exception is ObjCException objCException)
            {
                var managedException = sentryEvent.SentryExceptions.First();
                managedException.Value = objCException.Reason;
                managedException.Type = objCException.Name;
                managedException.Stacktrace = _nativeStack.CreateNativeStackTrace(managedException.Stacktrace, objCException.NSException.CallStackSymbols);
            }
        }
    }
}
