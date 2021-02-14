using Sentry.Xamarin.Forms.Extensions;
using Sentry.Xamarin.Internals;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Sentry.Xamarin.Forms.Internals
{
    internal class SentryXamarinFormsIntegration : IPageNavigationTracker
    {
        internal static SentryXamarinFormsIntegration Instance;
        private SentryXamarinOptions _options;
        private DelegateLogListener _xamarinLogger;
        private IHub _hub;

        public string CurrentPage { get; private set; }
        public void RegisterXamarinOptions(SentryXamarinOptions options)
        {
            _options = options;
        }

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

            RegisterXamarinFormsLogListener(hub);

            //Don't lock the main Thread while waiting for the current application to be created.
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

        internal void RegisterXamarinFormsLogListener(IHub hub)
        {
            _xamarinLogger = new DelegateLogListener((arg1, arg2) =>
            {
                if (_options.XamarinLoggerEnabled)
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

            if (_options.XamarinLoggerEnabled)
            {
                Log.Listeners.Add(_xamarinLogger);
            }
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
                        new Dictionary<string, string> { { "from", $"/{CurrentPage}" }, { "to", $"/{pageType.Name}" } });
                }
            }
            CurrentPage = pageType.Name;
        }
    }
}
