using Sentry.Integrations;
using Sentry.Protocol;
using Xamarin.Forms.Internals;
using Sentry.Xamarin.Forms.Internals;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sentry.Xamarin.Forms.Extensions;
using Xamarin.Essentials;

namespace Sentry.Xamarin.Forms
{
    /// <summary>
    /// Integration class for Sentry .NET SDK that you must call when initialzing SentrySDK.
    /// </summary>
    public class SentryXamarinFormsIntegration : ISdkIntegration
    {
        internal static Lazy<SentryXamarinOptions> Options = new Lazy<SentryXamarinOptions>();
        internal static SentryXamarinFormsIntegration Instance;
        internal static DelegateLogListener XamarinLogger;
        private IHub _hub;

        /// <summary>
        /// current page name.
        /// </summary>
        internal static string CurrentPage;

#if NATIVE_PROCESSOR
        internal NativeIntegration Nativeintegration { get; private set; }
#endif

        /// <summary>
        /// Register the Sentry Xamarin Forms SDK on Sentry.NET SDK
        /// </summary>
        /// <param name="hub">the Hub.</param>
        /// <param name="options">the Sentry options.</param>
        public void Register(IHub hub, SentryOptions options)
        {
            //Only one integration can be active
            if (Instance != null)
            {
                return;
            }
            Instance = this;
            _hub = hub;
            ConfigureSentryOptions(options);
            RegisterEventProcessors(options);
            RegisterNativeIntegrations(hub, options, Options.Value);
            RegisterXamarinLogListener(hub);

            //Don't lock the main Thread while you wait for the current application to be created.
            Task.Run(async () =>
                {
                    var application = await GetCurrentApplication().ConfigureAwait(false);
                    if (application is null)
                    {
                        options.DiagnosticLogger.Log(SentryLevel.Warning, "Sentry.Xamarin.Forms timeout for tracking Application.Current. Navigation tracking is going to be disabled");
                    }
                    else
                    {
                        application.PageAppearing += Current_PageAppearing;
                        application.PageDisappearing += Current_PageDisappearing;
                        application.RequestedThemeChanged += Current_RequestedThemeChanged;
                    }
                });
        }

        internal void RegisterEventProcessors(SentryOptions options)
        {
            options.AddEventProcessor(new XamarinFormsEventProcessor(options));

#if NATIVE_PROCESSOR
            options.AddEventProcessor(new NativeEventProcessor(options));
#else
            options.DiagnosticLogger?.Log(SentryLevel.Debug, "No NativeEventProcessor found for the given target.");
#endif
        }

        internal void RegisterNativeIntegrations(IHub hub, SentryOptions options, SentryXamarinOptions xamarinOptions)
        {
            if (xamarinOptions.NativeIntegrationEnabled)
            {
#if NATIVE_PROCESSOR
                Nativeintegration = new NativeIntegration(xamarinOptions);
                Nativeintegration.Register(hub, options);
#else
                options.DiagnosticLogger?.Log(SentryLevel.Debug, "No NativeIntegration found for the given target.");
#endif
            }
        }

        internal void RegisterXamarinLogListener(IHub hub)
        {
            XamarinLogger = new DelegateLogListener((arg1, arg2) =>
            {
                if (Options.Value.XamarinLoggerEnabled)
                {
                    hub.AddBreadcrumb(null,
                        "xamarin",
                        "info",
                        new Dictionary<string, string>
                        {
                            ["logger"] = arg1,
                            ["issue"] = arg2
                        }, level: BreadcrumbLevel.Warning);
                }
            });

            if (Options.Value.XamarinLoggerEnabled)
            {
                Log.Listeners.Add(XamarinLogger);
            }
        }

        internal void ConfigureSentryOptions(SentryOptions options)
        {
            options.Release ??= $"{AppInfo.PackageName}@{AppInfo.VersionString}";
        }

        /// <summary>
        /// Gets the current Application.
        /// If SentrySDK was initialized from the Native project (Android/IOS) the Application might not have been created in time.
        /// So we wait for max 5 seconds to see check if it was created or not
        /// </summary>
        /// <returns>Current application.</returns>
        private async Task<Application> GetCurrentApplication()
        {
            for (int i = 0; i < 10 && Application.Current is null; i++)
            {
                await Task.Delay(300).ConfigureAwait(false);
            }
            return Application.Current;
        }

        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            _hub.AddBreadcrumb(e.RequestedTheme.ToString(), "AppTheme.Change", level: BreadcrumbLevel.Info);
        }

        private void Current_PageDisappearing(object sender, Page e)
        {
            var type = e.GetPageType();
            if (type.BaseType.StartsWith("PopupPage"))
            {
                _hub.AddBreadcrumb(null,
                    "ui.lifecycle",
                    "navigation",
                    new Dictionary<string, string>
                    {
                        ["popup"] = type.Name,
                        ["state"] = "disappearing"
                    }, level: BreadcrumbLevel.Info);
            }
        }

        private void Current_PageAppearing(object sender, Page e)
        {
            var pageType = e.GetPageType();
            if (CurrentPage != null && CurrentPage != pageType.Name)
            {
                if (pageType.Name is "NavigationPage")
                {
                    return;
                }
                if (pageType.BaseType is "PopupPage")
                {
                    _hub.AddBreadcrumb(null,
                        "ui.lifecycle",
                        "navigation",
                        new Dictionary<string, string>
                        {
                            ["popup"] = pageType.Name,
                            ["state"] = "appearing"
                        }, level: BreadcrumbLevel.Info);
                    return;
                }
                else
                {
                    _hub.AddBreadcrumb(null,
                        "navigation",
                        "navigation",
                        new Dictionary<string, string>() { { "from", $"/{CurrentPage}" }, { "to", $"/{pageType.Name}" } });
                }
            }
            CurrentPage = pageType.Name;
        }
    }
}
