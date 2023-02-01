using Sentry.Internals.Session;
using Sentry.Xamarin.Internals;

#if __ANDROID__
using System;
using Sentry.Android.AssemblyReader;
#endif

namespace Sentry
{
    /// <summary>
    /// Sentry Xamarin SDK options.
    /// </summary>
    public class SentryXamarinOptions : SentryOptions
    {
        /// <summary>
        /// Automatically attaches a screenshot of the app at the time of the event capture.
        /// </summary>
        /// <remarks>
        /// Be aware PII might be included by activating this feature.
        /// </remarks>
        public bool AttachScreenshots { get; set; }

        /// <summary>
        /// Define the range of time that duplicated internal breadcrumbs will be ignored.
        /// </summary>
        public int InternalBreadcrumbDuplicationTimeSpan { get; set; } = 2;

        internal bool XamarinLoggerEnabled { get; set; } = true;
        internal bool NativeIntegrationEnabled { get; set; } = true;
        internal bool InternalCacheEnabled { get; set; } = true;
        internal IPageNavigationTracker PageTracker { get; set; }
        internal string ProtocolPackageName { get; set; }
        internal string ProjectName { get; set; }
        internal int GetCurrentApplicationDelay { get; set; } = 500;
        internal int GetCurrentApplicationMaxRetries { get; set; } = 15;

        /// <summary>
        /// Redirects session callbacks to SentrySdk based on
        /// device's app status.
        /// </summary>
        internal IDeviceActiveLogger SessionLogger { get; set; }

        internal Breadcrumb LastInternalBreadcrumb {get;set;}

        /// <summary>
        /// The Sentry Xamarin Options.
        /// </summary>
        public SentryXamarinOptions()
        {
            IsEnvironmentUser = false;
            AutoSessionTracking = true;
            IsGlobalModeEnabled = true;

#if __ANDROID__
            var reader = new Lazy<IAndroidAssemblyReader?>(() =>
                SentryAndroidHelpers.GetAndroidAssemblyReader(DiagnosticLogger));
            AssemblyReader = name => reader.Value?.TryReadAssembly(name);
#endif
        }
    }
}
