using Sentry.Xamarin.Internals;

namespace Sentry.Xamarin.Forms.Extensibility
{
    internal class DisabledNavigationPage : IPageNavigationTracker
    {
        public static DisabledNavigationPage Instance = new();

        public string CurrentPage => null;

        public void Register(IHub hub, SentryOptions options) { }

        public void Unregister() { }
    }
}
