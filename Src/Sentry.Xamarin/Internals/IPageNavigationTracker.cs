using Sentry.Integrations;

namespace Sentry.Xamarin.Internals
{
    /// <summary>
    /// Interface that should be implemented to track the page navigation on an app.
    /// </summary>
    interface IPageNavigationTracker : ISdkIntegration
    {
        /// <summary>
        /// The name of the current page on the application.
        /// </summary>
        public string CurrentPage { get; }

        /// <summary>
        /// Disable the navigation code.
        /// </summary>
        public void Unregister();
    }
}
