using Sentry.Integrations;

namespace Sentry.Xamarin.Forms.Internals
{
    internal partial class NativeIntegration : ISdkIntegration
    {
        internal bool Implemented => true;

        internal NativeIntegration(SentryXamarinOptions options) {
            options.NativeIntegrationEnabled = false;
        }

        public void Register(IHub hub, SentryOptions options) { }
        internal void Unregister() { }
    }
}