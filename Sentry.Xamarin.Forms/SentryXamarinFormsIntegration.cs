using Sentry.Integrations;
using Sentry.Protocol;
using Xamarin.Forms.Internals;
using Sentry.Xamarin.Forms.Internals;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Sentry.Xamarin.Forms
{
    public class SentryXamarinFormsIntegration : ISdkIntegration
    {
        internal static Lazy<SentryXamarinOptions> Options = new Lazy<SentryXamarinOptions>();
        internal static SentryXamarinFormsIntegration Instance;
        internal static DelegateLogListener XamarinLogger;

        /// <summary>
        /// current page name.
        /// </summary>
        internal static string CurrentPage;

        internal NativeIntegration Nativeintegration;

        public void Register(IHub hub, SentryOptions options)
        {
            //Only one integration can be active
            if (Instance != null) 
            { 
                return; 
            }
            Instance = this;
            options.AddEventProcessor(new XamarinFormsEventProcessor(options));

#if !NETSTANDARD
            options.AddEventProcessor(new NativeEventProcessor(options));
#endif
            XamarinLogger = new DelegateLogListener((arg1, arg2) =>
            {
                if (Options.Value.XamarinLoggerEnabled)
                {
                    SentrySdk.AddBreadcrumb(null,
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

            if (Options.Value.NativeIntegrationEnabled)
            {
                if (new NativeIntegration(Options.Value) is NativeIntegration nativeIntegration &&
                    nativeIntegration.Implemented)
                {
                    nativeIntegration.Register(hub, options);
                    Nativeintegration = nativeIntegration;
                }
            }

            //Don't lock the main Thread while you wait for the current application to be created.
            Task.Run(async () =>
            {
                var application = await GetCurrentApplication();
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

        /// <summary>
        /// Gets the current Application.
        /// If SentrySDK was initialized from the Native project (Android/IOS) the Application might not have been created in time.
        /// So we wait for max 5 seconds to see check if it was created or not
        /// </summary>
        /// <returns></returns>
        private async Task<Application> GetCurrentApplication()
        {
            for (int i = 0; i < 10 && Application.Current is null; i++)
            {
                await Task.Delay(300);
            }
            return Application.Current;
        }

        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            SentrySdk.AddBreadcrumb(e.RequestedTheme.ToString(), "AppTheme.Change", level: BreadcrumbLevel.Info);
        }

        private void Current_PageDisappearing(object sender, Page e)
        {
            var type = e.GetType();
            if (type.BaseType.Name.StartsWith("PopupPage"))
            {
                SentrySdk.AddBreadcrumb(null,
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
            var pageType = e.GetType();
            if (CurrentPage != null && CurrentPage != pageType.Name)
            {
                if (pageType.Name is "NavigationPage")
                {
                    return;
                }
                if (pageType.BaseType.Name is "PopupPage")
                {
                    SentrySdk.AddBreadcrumb(null,
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
                    SentrySdk.AddBreadcrumb(null,
                        "navigation",
                        "navigation",
                        new Dictionary<string, string>() { { "from", $"/{CurrentPage}" }, { "to", $"/{pageType.Name}" } });
                }
            }
            CurrentPage = pageType.Name;
        }
    }
}
