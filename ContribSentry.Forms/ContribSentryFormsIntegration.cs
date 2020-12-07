using Sentry;
using Sentry.Integrations;
using Sentry.Protocol;
using Xamarin.Forms.Internals;
using ContribSentry.Forms.Internals;

namespace ContribSentry.Forms
{
    public class ContribSentryFormsIntegration : ISdkIntegration
    {
        internal static bool LogXamlErrors { get; set; } = true;
        internal static DelegateLogListener XamlLogger;

        public void Register(IHub hub, SentryOptions options)
        {
            options.AddEventProcessor(new FormsEventProcessor(options));

#if !NETSTANDARD
            options.AddEventProcessor(new NativeEventProcessor(options));
#endif
            XamlLogger = new DelegateLogListener((arg1, arg2) =>
            {
                SentrySdk.AddBreadcrumb(arg2, $"XamlLogger.{arg1}", level: BreadcrumbLevel.Warning);
            });

            if (LogXamlErrors)
            {
                Log.Listeners.Add(XamlLogger);
            }
        }
    }
}
