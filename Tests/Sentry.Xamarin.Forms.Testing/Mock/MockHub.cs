using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sentry.Protocol.Envelopes;

namespace Sentry.Xamarin.Forms.Testing.Mock
{
    public class MockHub : IHub
    {
        public SentryId LastEventId => throw new NotImplementedException();

        public int CaptureEventCount = 0;
        public int FlushAsyncCount = 0;

        public bool IsEnabled => true;

        public IMetricAggregator Metrics => throw new NotImplementedException();

        public void BindClient(ISentryClient client) { }

        public SentryId CaptureEvent(SentryEvent evt, Action<Scope> configureScope)
        {
            CaptureEventCount++;
            return evt.EventId;
        }

        public void CaptureTransaction(SentryTransaction transaction) { }

        public void CaptureUserFeedback(UserFeedback userFeedback) { }

        public void ConfigureScope(Action<Scope> configureScope) { }

        public Task ConfigureScopeAsync(Func<Scope, Task> configureScope) => null;

        public SentryTransaction CreateTransaction(string name, string operation) => null;

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

        public SentryId CaptureEvent(SentryEvent evt, SentryHint hint, Scope scope = null) => SentryId.Empty;

        public void CaptureTransaction(SentryTransaction transaction, SentryHint hint) { }

        public BaggageHeader GetBaggage() => null;

        public TransactionContext ContinueTrace(string traceHeader, string baggageHeader, string name = null, string operation = null) => null;

        public TransactionContext ContinueTrace(SentryTraceHeader traceHeader, BaggageHeader baggageHeader, string name = null, string operation = null) => null;

        public SentryId CaptureEvent(SentryEvent evt, SentryHint hint, Action<Scope> configureScope)
        {
            CaptureEventCount++;
            return evt.EventId;
        }

        public SentryId CaptureEvent(SentryEvent evt, Scope scope = null, SentryHint hint = null)
        {
            CaptureEventCount++;
            return evt.EventId;
        }

        public void CaptureTransaction(SentryTransaction transaction, Scope scope, SentryHint hint) { }

        public bool CaptureEnvelope(Envelope envelope) => true;

        public SentryId CaptureCheckIn(string monitorSlug, CheckInStatus status, SentryId? sentryId = null, TimeSpan? duration = null, Scope scope = null) => SentryId.Empty;
    }
}
