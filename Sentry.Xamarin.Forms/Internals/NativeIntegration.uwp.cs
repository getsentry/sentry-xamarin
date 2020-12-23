using Sentry.Infrastructure;
using Sentry.Integrations;
using Sentry.Protocol;
using System;
using System.Runtime.ExceptionServices;
using System.Security;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Sentry.Xamarin.Forms.Internals
{
    internal class NativeIntegration : ISdkIntegration
    {
        private IHub _hub;
        private Application _application;

        internal NativeIntegration(SentryXamarinOptions options) { }

        /// <summary>
        /// Initialize the UWP specific code.
        /// </summary>
        /// <param name="hub">The hub.</param>
        /// <param name="options">The Sentry options.</param>
        public void Register(IHub hub, SentryOptions options) 
        {
            _hub = hub;

            options.DiagnosticLogger = new DebugDiagnosticLogger(options.DiagnosticsLevel);

            _application = Application.Current;
            if (_application != null)
            {
                _application.UnhandledException += NativeHandle;
                _application.EnteredBackground += OnSleep;
                _application.LeavingBackground += OnResume;
            }
        }
        internal void Unregister() 
        {
            if (_application != null)
            {
                _application.UnhandledException -= NativeHandle;
                _application.EnteredBackground -= OnSleep;
                _application.LeavingBackground -= OnResume;
            }
        }

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        internal void NativeHandle(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            //We need to backup the reference, because the Exception reference last for one access.
            //After that, a new  Exception reference is going to be set into e.Exception.
            var exception = e.Exception;

            Unregister();
            Handle(exception);
        }

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        internal void Handle(Exception exception)
        {
            if (exception != null)
            {
                exception.Data[Mechanism.HandledKey] = false;
                exception.Data[Mechanism.MechanismKey] = "Application.UnhandledException";
                _hub.CaptureException(exception);
                _hub.FlushAsync(TimeSpan.FromSeconds(10)).Wait();
                (_hub as IDisposable)?.Dispose();
            }
        }

        private void OnResume(object sender, LeavingBackgroundEventArgs e)
            => _hub.AddBreadcrumb("OnResume", "app.lifecycle", "event");

        private void OnSleep(object sender, EnteredBackgroundEventArgs e)
            => _hub.AddBreadcrumb("OnSleep", "app.lifecycle", "event");
    }
}