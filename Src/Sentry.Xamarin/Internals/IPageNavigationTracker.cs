using Sentry.Integrations;

namespace Sentry.Xamarin.Internals
{
    interface IPageNavigationTracker : ISdkIntegration
    {
        public string CurrentPage { get; }

        public void RegisterXamarinOptions(SentryXamarinOptions options);
    }
}
