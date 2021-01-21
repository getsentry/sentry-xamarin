using Sentry.Extensibility;
using Sentry.Protocol;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sentry.Xamarin.Internals
{
    internal class NativeStackTraceFactory : SentryStackTraceFactory
    {
        private static string _nativeRegexFormat => "(?<id>\\d+)\\s+(?<method>[a-zA-Z\\.-_?]+)\\s+(?<offset>0x[0-9a-fA-F]+)\\s+(?<function>.+?(?=\\s\\+))\\s+\\+\\s+(?<line>\\d+)";
        //"(?<id>\d+)\s+(?<method>[a-zA-Z\.-?]+)\s+(?<offset>0x[0-9a-fA-F]+)\s+(?<function>.+?(?=\s\+))\s+\+\s+(?<line>\d+)"
        private readonly SentryOptions _options;

        public NativeStackTraceFactory(SentryOptions options) : base(options) => _options = options;
        /// <summary>
        /// Creates a <see cref="SentryStackTrace" /> from the optional <see cref="Exception" />.
        /// </summary>
        /// <param name="exception">The exception to create the stacktrace from.</param>
        /// <returns>A Sentry stack trace.</returns>
        public override SentryStackTrace? Create(Exception? exception = null)
        {
            if (exception == null)
            {
                _options.DiagnosticLogger?.Log(SentryLevel.Debug, "No Exception to collect Native stack trace.");
                return base.Create(exception);
            }

            if (!IsNativeException(exception.Message))
            {
                _options.DiagnosticLogger?.Log(SentryLevel.Debug, "Not a Native exception, calling fallback.");
                return base.Create(exception);
            }
            return base.Create(exception);
//            var callbackStackTrace = base.Create(exception);
//            return CreateNativeStackTrace(callbackStackTrace, exception);
        }

        internal bool IsNativeException(string exceptionValue)
            => exceptionValue.StartsWith("Objective-C exception");

        internal SentryStackTrace CreateNativeStackTrace(SentryStackTrace managedStackTrace, IEnumerable<string> nativeStackTrace)
        {
            var nativeFramesList = new List<SentryStackFrame>();
            foreach(var nativeFrame in nativeStackTrace)
            {
                var match = Regex.Match(nativeFrame, _nativeRegexFormat);
                if (match.Success)
                {
                    nativeFramesList.Add(new SentryStackFrame()
                    {
                        Platform = "native",
                        Function = match.Groups["function"].Value,
                        Package = match.Groups["method"].Value,
                        InstructionAddress = match.Groups["offset"].Value
                    });
                }
            }
            nativeFramesList.AddRange(managedStackTrace.Frames);
            managedStackTrace.Frames = nativeFramesList;
            return managedStackTrace;
        }
    }
}
