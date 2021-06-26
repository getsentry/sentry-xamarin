using Foundation;
using Sentry.Extensions;
using Sentry.Integrations;
using System;
using System.Collections.Generic;
using System.Reflection;
using UIKit;

namespace Sentry.Xamarin.Internals
{
    internal class NativeIntegration : ISdkIntegration
    {
        private List<NSObject> _observerTokens;
        private SentryXamarinOptions _xamarinOptions;
        private IHub _hub;

        internal NativeIntegration(SentryXamarinOptions options)
        {
            _xamarinOptions = options;
            _xamarinOptions.ProjectName = Assembly.GetEntryAssembly().GetName().Name;
        }
        /// <summary>
        /// Initialize the iOS specific code.
        /// </summary>
        /// <param name="hub">The hub.</param>
        /// <param name="options">The Sentry options.</param>
        public void Register(IHub hub, SentryOptions options)
        {
            _hub = hub;
            _observerTokens = new List<NSObject>();
            _observerTokens.Add(NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidEnterBackgroundNotification, AppEnteredBackground));
            _observerTokens.Add(NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillEnterForegroundNotification, AppEnteredForeground));
            _observerTokens.Add(NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidReceiveMemoryWarningNotification, MemoryWarning));
            options.AddExceptionProcessor(new NativeExceptionProcessor(_xamarinOptions));
            RegisterInAppExclude();
        }

        private void RegisterInAppExclude()
        {
            _xamarinOptions.AddInAppExclude("UIKit.");
            _xamarinOptions.AddInAppExclude("Foundation.NS");
            _xamarinOptions.AddInAppExclude("ObjCRuntime");
        }
        internal void Unregister()
        {
            NSNotificationCenter.DefaultCenter.RemoveObservers(_observerTokens);
            _xamarinOptions.NativeIntegrationEnabled = false;
        }

        internal Action<NSNotification> AppEnteredBackground => (_) =>
        {
            _hub.AddInternalBreadcrumb(_xamarinOptions, 
                null,
                "ui.lifecycle",
                "navigation", data: new Dictionary<string, string>
                {
                    ["screen"] = _xamarinOptions.PageTracker?.CurrentPage,
                    ["state"] = "background"
                }, level: BreadcrumbLevel.Info);
            _xamarinOptions.SessionLogger?.StatePaused();
        };

        internal Action<NSNotification> AppEnteredForeground => (_) =>
        {
            _hub.AddInternalBreadcrumb(_xamarinOptions, 
                null,
                "ui.lifecycle",
                "navigation", data: new Dictionary<string, string>
                {
                    ["screen"] = _xamarinOptions.PageTracker?.CurrentPage,
                    ["state"] = "foreground"
                }, level: BreadcrumbLevel.Info);
            _xamarinOptions.SessionLogger?.StateResumed();
        };

        internal Action<NSNotification> MemoryWarning => (_) =>
        {
            _hub.AddInternalBreadcrumb(_xamarinOptions, 
                "low memory",
                "xamarin",
                "info",
                level: BreadcrumbLevel.Warning);
        };
    }
}