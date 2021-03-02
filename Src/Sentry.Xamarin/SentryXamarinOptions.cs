using Sentry.Xamarin.Internals;

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
        internal IPageNavigationTracker PageTracker { get; set; }
        internal string ProtocolPackageName { get; set; }
        internal string ProjectName { get; set; }
        internal int GetCurrentApplicationDelay { get; set; } = 500;
        internal int GetCurrentApplicationMaxRetries { get; set; } = 15;
    }
}
