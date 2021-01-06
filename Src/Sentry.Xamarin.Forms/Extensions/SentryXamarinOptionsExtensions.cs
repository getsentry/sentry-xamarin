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
            options.AddPageNavigationTrackerIntegration(new SentryXamarinFormsIntegration());
            options.ProtocolPackageName = ProtocolPackageName;
        }
    }
}
