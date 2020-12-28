namespace Sentry
{
    /// <summary>
    /// Sentry Xamarin SDK options.
    /// </summary>
    public class SentryXamarinOptions : SentryOptions
    {
        internal bool XamarinLoggerEnabled { get; set; } = true;
        internal bool NativeIntegrationEnabled { get; set; } = true;
        internal bool InternalCacheEnabled { get; set; } = true;
    }
}
