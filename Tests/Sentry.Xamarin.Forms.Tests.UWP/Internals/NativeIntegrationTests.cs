using Sentry.Protocol;
using Sentry.Xamarin.Internals;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sentry.Xamarin.Forms.Tests.UWP.Internals
{
    public class NativeIntegrationTests
    {
        /// <summary>
        /// Mock is not supported by .NET Native so we have to manually Mock the Hub.
        /// </summary>
        private class MockHub : IHub
        {
            public SentryId LastEventId => throw new NotImplementedException();

            public int CaptureEventCount = 0;
            public int FlushAsyncCount = 0;

            public bool IsEnabled => true;

            public void BindClient(ISentryClient client) { }

            public SentryId CaptureEvent(SentryEvent evt, Scope scope = null)
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

            public ITransaction StartTransaction(string name, string operation)
                => null;

            public ITransaction StartTransaction(ITransactionContext context, IReadOnlyDictionary<string, object> customSamplingContext)
                => null;

            public SentryTraceHeader GetTraceHeader() => null;

            public void CaptureTransaction(ITransaction transaction) { }
        }

        [Fact]
        public void Unregister_DoesntCrashifNotRegistered()
        {
            //Arrange
            var integration = new NativeIntegration(new SentryXamarinOptions());

            //Act
            integration.Unregister();
        }

        [Fact]
        public void Handle_RegisterUnhandleException()
        {
            //Arrange
            var integration = new NativeIntegration(new SentryXamarinOptions());
            var hub = new MockHub();

            var exception = new Exception();
            integration.Register(hub, new SentryOptions());

            //Act
            try
            {
                integration.Handle(new Exception());
            }
            finally
            {
                integration.Unregister();
            }

            //Assert
            Assert.Equal(1, hub.CaptureEventCount);
            Assert.Equal(1, hub.FlushAsyncCount);
        }
    }
}
