using Sentry.Xamarin.Forms;
using Sentry.Xamarin.Forms.Extensions;

namespace Sentry
{
    public static class SentryOptionsExtensions
    {
        public static void DisableBreadcrumbForXamlWarnings(this SentryOptions option)
        {
            SentryXamarinFormsIntegration.LogXamlErrors = false;
        }

        public static void DisableNativeIntegration(this SentryOptions options)
        {
            SentryXamarinFormsIntegration.Instance.UnregisterNativeIntegration();
        }
    }
}
