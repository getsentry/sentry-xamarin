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
    internal partial class NativeIntegration : ISdkIntegration
    {
        private IHub _hub;
        private Application _application;
        internal bool Implemented => true;

        public void Register(IHub hub, SentryOptions options) 
        {
            _hub = hub;

            options.DiagnosticLogger = new DebugDiagnosticLogger(options.DiagnosticsLevel);

            _application = Application.Current;
            _application.UnhandledException += NativeHandle;
            _application.EnteredBackground += OnSleep;
            _application.LeavingBackground += OnResume;
        }
        internal void Unregister() 
        {
            _application.UnhandledException -= NativeHandle;
            _application.EnteredBackground -= OnSleep;
            _application.LeavingBackground -= OnResume;

            _hub = null;
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
                var st = exception.StackTrace.ToString();
                exception.Data[Mechanism.HandledKey] = false;
                exception.Data[Mechanism.MechanismKey] = "Application.UnhandledException";
                var @eventId = SentrySdk.CaptureException(exception);
                SentrySdk.FlushAsync(TimeSpan.FromSeconds(10)).Wait();
                (_hub as IDisposable)?.Dispose();
            }
        }


        private void OnResume(object sender, LeavingBackgroundEventArgs e)
            => SentrySdk.AddBreadcrumb("OnResume", "app.lifecycle", "event");

        private void OnSleep(object sender, EnteredBackgroundEventArgs e)
            => SentrySdk.AddBreadcrumb("OnSleep", "app.lifecycle", "event");
    }
}