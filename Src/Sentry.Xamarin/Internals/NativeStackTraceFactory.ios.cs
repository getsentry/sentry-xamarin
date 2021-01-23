using Sentry.Extensibility;
using Sentry.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Sentry.Xamarin.Internals
{
    internal class NativeStackTraceFactory : SentryStackTraceFactory
    {
        private static string _nativeRegexFormat => "(?<id>\\d+)\\s+(?<method>[a-zA-Z\\.-_?]+)\\s+(?<offset>0x[0-9a-fA-F]+)\\s+(?<function>.+?(?=\\s\\+))\\s+\\+\\s+(?<line>\\d+)";
        //"(?<id>\d+)\s+(?<method>[a-zA-Z\.-?]+)\s+(?<offset>0x[0-9a-fA-F]+)\s+(?<function>.+?(?=\s\+))\s+\+\s+(?<line>\d+)"
        private readonly SentryXamarinOptions _options;

        public NativeStackTraceFactory(SentryXamarinOptions options) : base(options) => _options = options;
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
        }

        internal bool IsNativeException(string exceptionValue)
            => exceptionValue.StartsWith("Objective-C exception");

        internal SentryStackTrace CreateNativeStackTrace(SentryStackTrace managedStackTrace, IEnumerable<string> nativeStackTrace)
        {
            var nativeFramesList = new List<SentryStackFrame>();
            for (int i = nativeStackTrace.Count() - 1; i >= 0; i--)
            {
                var match = Regex.Match(nativeStackTrace.ElementAt(i), _nativeRegexFormat);
                if (match.Success)
                {
                    var method = match.Groups["method"].Value;
                    nativeFramesList.Add(new SentryStackFrame()
                    {
                        Platform = "native",
                        Function = match.Groups["function"].Value,
                        Package = method,
                        InstructionAddress = match.Groups["offset"].Value,
                        InApp = method == _options.ProjectName
                    });
                }
            }
            managedStackTrace.Frames = managedStackTrace.Frames.Concat(nativeFramesList).ToList();
            return managedStackTrace;
        }
    }
}
