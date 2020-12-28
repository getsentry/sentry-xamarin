namespace Sentry.Xamarin.Forms.Internals
{
    internal class SentryXamarinOptions
    {
        internal bool XamarinLoggerEnabled { get; set; } = true;
        internal bool NativeIntegrationEnabled { get; set; } = true;
        internal bool InternalCacheEnabled { get; set; } = true;
    }
}
