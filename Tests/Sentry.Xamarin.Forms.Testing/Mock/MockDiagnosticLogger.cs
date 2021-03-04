using Sentry.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sentry.Xamarin.Forms.Testing.Mock
{
    public class MockDiagnosticLogger : IDiagnosticLogger
    {
        private SentryLevel _level;
        private List<(SentryLevel, string, Exception, object[])> _logs = new List<(SentryLevel, string, Exception, object[])>();
        public MockDiagnosticLogger(SentryLevel level) => _level = level;

        public bool IsEnabled(SentryLevel level) => _level >= level;

        public void Log(SentryLevel logLevel, string message, Exception exception = null, params object[] args)
        {
            _logs.Add((logLevel, message, exception, args));
        }

        public int Count() => _logs.Count;

        public (SentryLevel, string, Exception, object[]) LastItem() => _logs.LastOrDefault();

        public bool Contains(string message) => _logs.Any(l => l.Item2.Contains(message));
        public bool Contains(SentryLevel level, string message) => _logs.Any(l => l.Item1 == level && l.Item2.Contains(message));
    }
}
