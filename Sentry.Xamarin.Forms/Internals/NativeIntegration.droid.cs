using Sentry.Integrations;
using Sentry.Protocol;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace Sentry.Xamarin.Forms.Internals
{
    internal partial class NativeIntegration : ISdkIntegration
    {
        internal bool Implemented { get; private set; } = true;
        public void Register(IHub hub, SentryOptions options)
        {
            try
            {
                Platform.ActivityStateChanged += Platform_ActivityStateChanged;
            }
            catch (Exception ex)
            {
                options.DiagnosticLogger.Log(SentryLevel.Error, "Sentry.Xamarin.Forms failed to attach AtivityStateChanged", ex);
                Implemented = false;
            }
        }

        internal void Unregister()
        {
            if (Implemented)
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
                }, level: BreadcrumbLevel.Info);
        }
    }
}
