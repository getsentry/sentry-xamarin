using ContribSentry.Forms;

namespace Sentry
{
    public static class ContribSentryOptionsExtensions
    {
        public static void DisableBreadcrumbForXamlWarnings(SentryOptions option)
        {
            ContribSentryFormsIntegration.LogXamlErrors = false;
        }
    }
}
