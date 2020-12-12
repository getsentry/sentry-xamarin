using Sentry.Xamarin.Forms;
using Sentry.Xamarin.Forms.Extensions;

namespace Sentry
{
    public static class SentryOptionsExtensions
    {
        public static void DisableBreadcrumbForXamlWarnings(SentryOptions option)
        {
            SentryXamarinFormsIntegration.Instance.UnregisterNativeIntegration();
        }
    }
}
