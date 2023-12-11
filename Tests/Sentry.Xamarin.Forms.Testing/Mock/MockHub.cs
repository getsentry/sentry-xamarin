﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sentry.Xamarin.Forms.Testing.Mock
{
    public class MockHub : IHub
    {
        public SentryId LastEventId => throw new NotImplementedException();

        public int CaptureEventCount = 0;
        public int FlushAsyncCount = 0;

        public bool IsEnabled => true;

        public void BindClient(ISentryClient client) { }

        public SentryId CaptureEvent(SentryEvent evt, Action<Scope> configureScope)
        {
            CaptureEventCount++;
            return evt.EventId;
        }

        public void CaptureTransaction(Transaction transaction) { }

        public void CaptureUserFeedback(UserFeedback userFeedback) { }

        public void ConfigureScope(Action<Scope> configureScope) { }

        public Task ConfigureScopeAsync(Func<Scope, Task> configureScope) => null;

        public Transaction CreateTransaction(string name, string operation) => null;

        public Task FlushAsync(TimeSpan timeout)
        {
            FlushAsyncCount++;
            return Task.Run(() => { });
        }

        public SentryTraceHeader GetSentryTrace() => null;

        public IDisposable PushScope() => null;

        public IDisposable PushScope<TState>(TState state) => null;

        public void WithScope(Action<Scope> scopeCallback) { }

        public ITransactionTracer StartTransaction(string name, string operation)
            => null;

        public ITransactionTracer StartTransaction(ITransactionContext context, IReadOnlyDictionary<string, object> customSamplingContext)
            => null;

        public SentryTraceHeader GetTraceHeader() => null;

        public ISpan GetSpan()
            => null;

        public void BindException(Exception exception, ISpan span) { }

        public void StartSession() { }

        public void PauseSession() { }

        public void ResumeSession() { }

        public void EndSession(SessionEndStatus status = SessionEndStatus.Exited) { }

        public void CaptureSession(SessionUpdate sessionUpdate) { }

        public SentryId CaptureEvent(SentryEvent evt, Hint hint, Scope scope = null) => SentryId.Empty;

        public void CaptureTransaction(Transaction transaction, Hint hint) { }

        public BaggageHeader GetBaggage() => null;

        public TransactionContext ContinueTrace(string traceHeader, string baggageHeader, string name = null, string operation = null) => null;

        public TransactionContext ContinueTrace(SentryTraceHeader traceHeader, BaggageHeader baggageHeader, string name = null, string operation = null) => null;

        public SentryId CaptureEvent(SentryEvent evt, Hint hint, Action<Scope> configureScope)
        {
            CaptureEventCount++;
            return evt.EventId;
        }

        public SentryId CaptureEvent(SentryEvent evt, Scope scope = null, Hint hint = null)
        {
            CaptureEventCount++;
            return evt.EventId;
        }

        public void CaptureTransaction(Transaction transaction, Scope scope, Hint hint) { }
    }
}
