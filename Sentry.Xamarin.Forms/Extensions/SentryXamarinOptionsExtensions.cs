using Sentry.Xamarin.Forms;
using Sentry.Xamarin.Forms.Extensions;
using Sentry.Xamarin.Forms.Internals;
using System;
using Xamarin.Essentials;

namespace Sentry
{
    /// <summary>
    /// Extend SentryXamarinOptions by allowing it to manipulate options from Sentry Xamarin Forms.
    /// </summary>
    public static class SentryXamarinOptionsExtensions
    {
        private static Lazy<string> _internalCacheDefaultPath = new Lazy<string>(()=> Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

        /// <summary>
        /// Disables the automatic Xamarin warning crumbs.
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableXamarinWarningsBreadcrumbs(this SentryXamarinOptions options)
        {
            options.XamarinLoggerEnabled = false;
        }

        /// <summary>
        /// Disables the Sentry Xamarin Forms Native integration.
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableNativeIntegration(this SentryXamarinOptions options)
        {
            SentryXamarinFormsIntegration.Instance.UnregisterNativeIntegration(options);
        }

        /// <summary>
        /// Disables the cache, must be called before Sentry gets initialized
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableXamarinFormsCache(this SentryXamarinOptions options)
        {
            options.InternalCacheEnabled = false;
        }

        internal static string DefaultCacheDirectoyPath(this SentryXamarinOptions options)
            => _internalCacheDefaultPath.Value;

        internal static void ConfigureSentryXamarinOptions(this SentryXamarinOptions options)
        {
            options.Release ??= $"{AppInfo.PackageName}@{AppInfo.VersionString}";
            if (options.InternalCacheEnabled)
            {
                options.CacheDirectoryPath ??= options.DefaultCacheDirectoyPath();
            }
        }

        internal static void RegisterXamarinEventProcessors(this SentryXamarinOptions options)
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
