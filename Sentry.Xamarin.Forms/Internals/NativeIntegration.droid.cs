using Sentry.Integrations;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace Sentry.Xamarin.Forms.Internals
{
    internal partial class NativeIntegration : ISdkIntegration
    {
        internal bool Implemented { get; private set; } = true;

        private SentryXamarinOptions _xamarinOptions;

        internal NativeIntegration(SentryXamarinOptions options) => _xamarinOptions = options;

        public void Register(IHub hub, SentryOptions options)
        {
            try
            {
                Platform.ActivityStateChanged += Platform_ActivityStateChanged;
            }
            catch (Exception ex)
            {
                options.DiagnosticLogger.Log(SentryLevel.Error, "Sentry.Xamarin.Forms failed to attach AtivityStateChanged", ex);
                _xamarinOptions.NativeIntegrationEnabled = false;
            }
        }

        internal void Unregister()
        {
            if (_xamarinOptions.NativeIntegrationEnabled)
            {
                Platform.ActivityStateChanged -= Platform_ActivityStateChanged;
            }
        }

        private void Platform_ActivityStateChanged(object sender, ActivityStateChangedEventArgs e)
        {
            SentrySdk.AddBreadcrumb(null,
                "ui.lifecycle",
                "navigation", data: new Dictionary<string, string>
                {
                    ["screen"] = SentryXamarinFormsIntegration.CurrentPage,
                    ["state"] = e.State.ToString()
                }, level: Protocol.BreadcrumbLevel.Info);
        }
    }
}
