using Sentry.Xamarin.Forms;

namespace Sentry
{
    public static class SentryOptionsExtensions
    {
        public static void DisableBreadcrumbForXamlWarnings(SentryOptions option)
        {
            SentryFormsIntegration.LogXamlErrors = false;
        }
    }
}
