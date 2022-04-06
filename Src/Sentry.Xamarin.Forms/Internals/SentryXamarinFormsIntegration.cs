using Sentry.Extensions;
using Sentry.Integrations;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Sentry.Xamarin.Forms.Internals
{
    internal class SentryXamarinFormsIntegration : ISdkIntegration
    {
        internal static SentryXamarinFormsIntegration Instance { get; set; }

        private readonly SentryXamarinOptions _options;

        private DelegateLogListener _xamarinLogger;
        private IHub _hub;

        public SentryXamarinFormsIntegration(SentryXamarinOptions options) => _options = options;

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
        }

        /// <summary>
        /// creates breadcrumbs from events received from RequestedThemeChanged on the given application.
        /// </summary>
        /// <param name="application">The Xamarin Application.</param>
        internal void RegisterRequestThemeChange(Application application)
            => application.RequestedThemeChanged += Current_RequestedThemeChanged;

        internal void RegisterXamarinFormsLogListener(IHub hub)
        {
            _xamarinLogger = new DelegateLogListener((logger, issue) =>
            {
                if (_options.XamarinLoggerEnabled)
                {
                    hub?.AddInternalBreadcrumb(_options,
                        null,
                        "xamarin",
                        "info",
                        new Dictionary<string, string>
                        {
                            ["logger"] = logger,
                            ["issue"] = issue
                        }, level: BreadcrumbLevel.Warning);
                }
            });

            if (_options.XamarinLoggerEnabled)
            {
                Log.Listeners.Add(_xamarinLogger);
            }
        }

        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs themeEvent)
            => _hub?.AddBreadcrumb(themeEvent.RequestedTheme.ToString(), "AppTheme.Change", level: BreadcrumbLevel.Info);
    }
}
