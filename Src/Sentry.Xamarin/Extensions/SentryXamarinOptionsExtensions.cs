using System;
using Sentry.Xamarin.Internals;

#if NETSTANDARD2_0
using System.Text.RegularExpressions;
#endif

#if NATIVE_PROCESSOR
using Sentry.Internals.Session;
using System.Text.RegularExpressions;
#endif

#if NATIVE_PROCESSOR && !UAP10_0_16299
using Sentry.Internals.Device.Screenshot;
#endif

namespace Sentry
{
    /// <summary>
    /// Extend SentryXamarinOptions by allowing it to manipulate options from Sentry Xamarin.
    /// </summary>
    public  static partial class SentryXamarinOptionsExtensions
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
        /// Disables the Sentry Xamarin Native integration.
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableNativeIntegration(this SentryXamarinOptions options)
        {
            options.NativeIntegrationEnabled = false;
        }

        /// <summary>
        /// Disables the cache, must be called before Sentry gets initialized
        /// </summary>
        /// <param name="options">The Sentry options.</param>
        public static void DisableOfflineCaching(this SentryXamarinOptions options)
        {
            options.InternalCacheEnabled = false;
        }

        internal static string DefaultCacheDirectoryPath(this SentryXamarinOptions _)
            => _internalCacheDefaultPath.Value;

        internal static void ConfigureSentryXamarinOptions(this SentryXamarinOptions options)
        {
            if (options.InternalCacheEnabled)
            {
                options.CacheDirectoryPath ??= options.DefaultCacheDirectoryPath();
            }

#if __ANDROID__
            options.AdjustSaasDsn();
#endif
        }

        // Internal for testing.
        internal static void AdjustSaasDsn(this SentryXamarinOptions options)
            // Mono for Android uses an outdated certificate that doesn't work with the default SaaS DSN.
            // Instead of users altering their DSN on their code to use an alternative DSN
            // We'll alter their DSN to the alternative one that works with Xamarin Android.
            // More information: https://github.com/xamarin/xamarin-android/issues/6351
            => options.Dsn = options.Dsn is not null ? Regex.Replace(options.Dsn, "@o\\d+\\.ingest\\.sentry", "@sentry") : null;

        internal static void RegisterXamarinEventProcessors(this SentryXamarinOptions options)
        {
            options.AddEventProcessor(new XamarinEventProcessor(options));

#if NATIVE_PROCESSOR
            options.AddEventProcessor(new NativeEventProcessor(options));
#else
            options.DiagnosticLogger?.Log(SentryLevel.Debug, "No NativeEventProcessor found for the given target.");
#endif
        }

        internal static void RegisterScreenshotEventProcessor(this SentryXamarinOptions options)
        {
#if NATIVE_PROCESSOR && !UAP10_0_16299
            if (options.AttachScreenshots)
            {
                SentrySdk.ConfigureScope(s => 
                    s.AddAttachment(new ScreenshotAttachment(new ScreenshotAttachmentContent())));
            }
#endif
        }

        internal static bool RegisterNativeIntegrations(this SentryXamarinOptions options)
        {
            if (options.NativeIntegrationEnabled)
            {
#if NATIVE_PROCESSOR
                var nativeintegration = new NativeIntegration(options);
                options.AddIntegration(nativeintegration);
                return true;
#else
                options.DiagnosticLogger?.Log(SentryLevel.Debug, "No NativeIntegration found for the given target.");
#endif
            }
            return false;
        }

        internal static bool RegisterNativeActivityStatus(this SentryXamarinOptions options)
        {
#if LIFECYCLE_PROCESSOR
            if (options.AutoSessionTracking)
            {
                options.SessionLogger = new DeviceActiveLogger();
                return true;
            }
#endif
            return false;
        }

        /// <summary>
        /// Unregister the previous navigation tracker and adds the registers the new Navigation Tracker to SentryXamarinOptions.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="tracker">The IPageNavigationTracker.</param>
        internal static void AddPageNavigationTrackerIntegration(this SentryXamarinOptions options, IPageNavigationTracker tracker)
        {
            options.AddIntegration(tracker);
            options.PageTracker?.Unregister();
            options.PageTracker = tracker;
        }

        internal static void RegisterXamarinInAppExclude(this SentryXamarinOptions options)
        {
            options.AddInAppExclude("Java.");
            options.AddInAppExclude("ZXing");
            options.AddInAppExclude("TouchView");
            options.AddInAppExclude("Rg.Plugins");
            options.AddInAppExclude("Prism");
            options.AddInAppExclude("CarouselView.FormsPlugin");
            options.AddInAppExclude("CardsView");
            options.AddInAppExclude("MvvmCross.Forms");
            options.AddInAppExclude("Com.Airbnb.Xamarin.Forms.Lottie");
            options.AddInAppExclude("Com.OneSignal");
            options.AddInAppExclude("SQLite-net");
        }
    }
}
