using Sentry;
using Sentry.Integrations;
using Sentry.Protocol;
using Xamarin.Forms.Internals;
using ContribSentry.Forms.Internals;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContribSentry.Forms
{
    public partial class ContribSentryFormsIntegration : ISdkIntegration
    {
        private NativeExceptionHandler _nativeHandler;
        private string _previousPageName;
        internal static bool LogXamlErrors { get; set; } = true;
        internal static DelegateLogListener XamlLogger;

        public void Register(IHub hub, SentryOptions options)
        {
            options.AddEventProcessor(new FormsEventProcessor(options));

            _nativeHandler = new NativeExceptionHandler();

#if !NETSTANDARD
            options.AddEventProcessor(new NativeEventProcessor(options));
#endif
            XamlLogger = new DelegateLogListener((arg1, arg2) =>
            {
                if (LogXamlErrors)
                {
                    SentrySdk.AddBreadcrumb(arg2, $"XamlLogger.{arg1}", level: BreadcrumbLevel.Warning);
                }
            });

            if (LogXamlErrors)
            {
                Log.Listeners.Add(XamlLogger);
            }

            //If initialized from the Android/IOS project, the current application is not going to be set in time, so wait a bit...
            Task.Run(async () =>
            {
                for(int i=0; i < 5 && Application.Current is null; i++) 
                {
                    await Task.Delay(1000);
                }
                if (Application.Current is null)
                {
                    options.DiagnosticLogger.Log(SentryLevel.Warning, "ContribSentry.Forms timeout for tracking Application.Current. Navigation tracking is going to be disabled");
                }
                else 
                {
                    Application.Current.PageAppearing += Current_PageAppearing;
                    Application.Current.PageDisappearing += Current_PageDisappearing;
                    Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
                }
            });
        }

        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            SentrySdk.AddBreadcrumb(e.RequestedTheme.ToString(), "AppTheme.Change", level: BreadcrumbLevel.Info);
        }

        private void Current_PageDisappearing(object sender, Page e)
        {
            _previousPageName = e?.Title ?? e?.GetType().ToString();
        }

        private void Current_PageAppearing(object sender, Page e)
        {
            if (_previousPageName != null)
            {
                var name = e.Title ?? e.GetType().ToString();
                SentrySdk.AddBreadcrumb(null,
                    "navigation",
                    "navigation",
                    new Dictionary<string, string>() { { "from", $"/{_previousPageName}" }, { "to", $"/{name}" } });
            }
        }
    }
}
