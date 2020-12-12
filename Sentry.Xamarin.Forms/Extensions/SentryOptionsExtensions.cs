using Sentry.Xamarin.Forms;
using Sentry.Xamarin.Forms.Extensions;

namespace Sentry
{
    public static class SentryOptionsExtensions
    {
        public static void DisableBreadcrumbForXamlWarnings(this SentryOptions _)
        {
            SentryXamarinFormsIntegration.Options.Value.XamarinLoggerEnabled = false;
        }

        public static void DisableNativeIntegration(this SentryOptions _)
        {
            SentryXamarinFormsIntegration.Instance.UnregisterNativeIntegration();
        }
    }
}
