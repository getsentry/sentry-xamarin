using Sentry.Xamarin.Forms.Extensibility;
using Sentry.Xamarin.Forms.Internals;

namespace Sentry
{
    /// <summary>
    /// Extend SentryXamarinOptions by allowing it to manipulate options from Sentry Xamarin Forms.
    /// </summary>
    public static partial class SentryXamarinOptionsExtensions
    {
        internal static readonly string ProtocolPackageName = "sentry.dotnet.xamarin-forms";

        /// <summary>
        /// Add the Sentry Xamarin Forms integration to Sentry.Xamarin SDK.
        /// </summary>
        /// <param name="options">The Sentry Xamarion Options.</param>
        public static void AddXamarinFormsIntegration(this SentryXamarinOptions options)
        {
            var applicationListener = new FormsApplicationListener(options);

            var formsIntegration = new SentryXamarinFormsIntegration(options);
            options.AddIntegration(formsIntegration);
            applicationListener.AddListener(formsIntegration.RegisterRequestThemeChange);

            if (options.PageTracker is null)
            {
                var navigationIntegration = new FormsNavigationIntegration();
                options.AddPageNavigationTrackerIntegration(navigationIntegration);
                applicationListener.AddListener(navigationIntegration.ApplySentryNavigationEvents);
            }

            options.ProtocolPackageName = ProtocolPackageName;

            applicationListener.Invoke();
        }

        /// <summary>
        /// Disables the default integration that registers the page and popup navigation.
        /// </summary>
        /// <param name="options">The Sentry Xamarion Options.</param>
        public static void RemoveNavigationPageIntegration(this SentryXamarinOptions options)
        {
            options.PageTracker?.Unregister();
            options.PageTracker = DisabledNavigationPage.Instance;

        }
    }
}
