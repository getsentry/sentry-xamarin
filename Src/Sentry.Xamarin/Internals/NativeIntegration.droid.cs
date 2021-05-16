using Android.Runtime;
using Sentry.Integrations;
using Sentry.Protocol;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using IO.Sentry;

namespace Sentry.Xamarin.Internals
{
    internal class NativeIntegration : ISdkIntegration
    {

        private SentryXamarinOptions _xamarinOptions;
        private IHub _hub;

        internal NativeIntegration(SentryXamarinOptions options) => _xamarinOptions = options;

        /// <summary>
        /// Initialize the Android specific code.
        /// </summary>
        /// <param name="hub">The hub.</param>
        /// <param name="options">The Sentry options.</param>
        public void Register(IHub hub, SentryOptions options)
        {
            try
            {
                _hub = hub;
                Platform.ActivityStateChanged += Platform_ActivityStateChanged;
                AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
                var nativeOptions = new IO.Sentry.SentryOptions()
                {
                    Dsn = options.Dsn,
                    CacheDirPath = options.CacheDirectoryPath
                };
                IO.Sentry.Sentry.Init(nativeOptions);
            }
            catch (Exception ex)
            {
                options.DiagnosticLogger?.Log(SentryLevel.Error, "Sentry.Xamarin failed to attach AtivityStateChanged", ex);
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
            _hub.AddBreadcrumb(null,
                "ui.lifecycle",
                "navigation", data: new Dictionary<string, string>
                {
                    ["screen"] = _xamarinOptions.PageTracker?.CurrentPage,
                    ["state"] = e.State.ToString()
                }, level: BreadcrumbLevel.Info);
        }
        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            e.Exception.Data[Mechanism.HandledKey] = e.Handled;
            e.Exception.Data[Mechanism.MechanismKey] = "UnhandledExceptionRaiser";
            SentrySdk.CaptureException(e.Exception);
            if (!e.Handled)
            {
                SentrySdk.Close();
            }

        }
    }
}
