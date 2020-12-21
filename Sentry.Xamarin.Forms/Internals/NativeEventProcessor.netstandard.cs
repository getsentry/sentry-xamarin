namespace Sentry.Xamarin.Forms.Internals
{
    internal class NativeEventProcessor : INativeEventProcessor
    {
        public bool Implemented => false;

        public string TargetName => "NET Standard";

        internal NativeEventProcessor(SentryOptions options) { }

        public SentryEvent? Process(SentryEvent @event)
            => @event;
    }
}