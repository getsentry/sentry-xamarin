using Sentry.Xamarin.Forms;
using Sentry.Xamarin.Forms.Extensions;

namespace Sentry
{
    public static class SentryOptionsExtensions
    {
        /// <summary>
        /// Disables the automatic Xamarin warning crumbs.
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableXamarinWarningsBreadcrumbs(this SentryOptions options)
        {
            SentryXamarinFormsIntegration.Options.Value.XamarinLoggerEnabled = false;
        }

        /// <summary>
        /// Disables the Sentry Xamarin Forms Native integration.
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableNativeIntegration(this SentryOptions options)
        {
            SentryXamarinFormsIntegration.Instance.UnregisterNativeIntegration();
        }
    }
}
