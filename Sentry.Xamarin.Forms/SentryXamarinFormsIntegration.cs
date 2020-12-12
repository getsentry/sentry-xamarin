using Sentry.Integrations;
using Sentry.Protocol;
using Xamarin.Forms.Internals;
using Sentry.Xamarin.Forms.Internals;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sentry.Xamarin.Forms
{
    public class SentryXamarinFormsIntegration : ISdkIntegration
    {
        #region Internal Options
        internal static bool LogXamlErrors { get; set; } = true;
        #endregion

        internal static SentryXamarinFormsIntegration Instance;
        internal NativeIntegration Nativeintegration;
        internal static string CurrentPage;
        internal static DelegateLogListener XamlLogger;

        public void Register(IHub hub, SentryOptions options)
        {
            Instance = this;
            options.AddEventProcessor(new XamarinFormsEventProcessor(options));

#if !NETSTANDARD
            options.AddEventProcessor(new NativeEventProcessor(options));
#endif
            XamlLogger = new DelegateLogListener((arg1, arg2) =>
            {
                if (LogXamlErrors)
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

            if (LogXamlErrors)
            {
                Log.Listeners.Add(XamlLogger);
            }

            //If initialized from the Android/IOS project, the current application is not going to be set in time, so wait a bit...
            Task.Run(async () =>
            {
                for (int i = 0; i < 5 && Application.Current is null; i++)
                {
                    await Task.Delay(1000);
                }
                if (Application.Current is null)
                {
                    options.DiagnosticLogger.Log(SentryLevel.Warning, "Sentry.Xamarin.Forms timeout for tracking Application.Current. Navigation tracking is going to be disabled");
                }
                else
                {
                    Application.Current.PageAppearing += Current_PageAppearing;
                    Application.Current.PageDisappearing += Current_PageDisappearing;
                    Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
                }

                Nativeintegration = new NativeIntegration();
                if (Nativeintegration.Implemented)
                {
                    Nativeintegration.Register(hub, options);
                }
                else
                {
                    Nativeintegration = null;
                }
            });
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
