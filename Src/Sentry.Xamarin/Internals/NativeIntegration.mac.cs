using Sentry.Integrations;
using System.Reflection;

namespace Sentry.Xamarin.Internals
{
    internal class NativeIntegration : ISdkIntegration
    {
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
            options.AddExceptionProcessor(new NativeExceptionProcessor(_xamarinOptions));
            RegisterInAppExclude();
        }

        private void RegisterInAppExclude()
        {
            _xamarinOptions.AddInAppExclude("AppKit.");
            _xamarinOptions.AddInAppExclude("Foundation.NS");
            _xamarinOptions.AddInAppExclude("ObjCRuntime");
        }
        internal void Unregister()
        {
            _xamarinOptions.NativeIntegrationEnabled = false;
        }
    }
}