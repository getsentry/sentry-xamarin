using Sentry.Xamarin.Forms;
using Sentry.Xamarin.Forms.Extensions;
using Sentry.Xamarin.Forms.Internals;
using System;
using Xamarin.Essentials;

namespace Sentry
{
    /// <summary>
    /// Extend SentryOptions by allowing it to manipulate options from Sentry Xamarin Forms.
    /// </summary>
    public static class SentryOptionsExtensions
    {
        private static Lazy<string> _internalCacheDefaultPath = new Lazy<string>(()=> Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

        /// <summary>
        /// Disables the automatic Xamarin warning crumbs.
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableXamarinWarningsBreadcrumbs(this SentryOptions options)
        {
            options.GetSentryXamarinOptions().XamarinLoggerEnabled = false;
        }

        /// <summary>
        /// Disables the Sentry Xamarin Forms Native integration.
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableNativeIntegration(this SentryOptions options)
        {
            SentryXamarinFormsIntegration.Instance.UnregisterNativeIntegration(options.GetSentryXamarinOptions());
        }

        /// <summary>
        /// Disables the cache, must be called before Sentry gets initialized
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableXamarinFormsCache(this SentryOptions options)
        {
            options.GetSentryXamarinOptions().InternalCacheEnabled = false;
        }

        internal static SentryXamarinOptions GetSentryXamarinOptions(this SentryOptions options)
            => SentryXamarin.Options;

        internal static string DefaultCacheDirectoyPath(this SentryOptions options)
            => _internalCacheDefaultPath.Value;

        internal static void ConfigureSentryXamarinOptions(this SentryOptions options, SentryXamarinOptions xamarinOptions)
        {
            options.Release ??= $"{AppInfo.PackageName}@{AppInfo.VersionString}";
            if (xamarinOptions.InternalCacheEnabled)
            {
                options.CacheDirectoryPath ??= options.DefaultCacheDirectoyPath();
            }
        }

        internal static void RegisterXamarinEventProcessors(this SentryOptions options)
        {
            options.AddEventProcessor(new XamarinFormsEventProcessor(options));

#if NATIVE_PROCESSOR
            options.AddEventProcessor(new NativeEventProcessor(options));
#else
            options.DiagnosticLogger?.Log(SentryLevel.Debug, "No NativeEventProcessor found for the given target.");
#endif
        }

    }
}
