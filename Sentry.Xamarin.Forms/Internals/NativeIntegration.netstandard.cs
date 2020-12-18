using Sentry.Integrations;

namespace Sentry.Xamarin.Forms.Internals
{
    internal partial class NativeIntegration : ISdkIntegration
    {
        internal bool Implemented => true;

        internal NativeIntegration(SentryXamarinOptions options) {
            options.NativeIntegrationEnabled = false;
        }
        /// <summary>
        /// Initialize a disabled native Integration.
        /// </summary>
        /// <param name="hub">The hub.</param>
        /// <param name="options">The Sentry options.</param>
        public void Register(IHub hub, SentryOptions options) { }
        internal void Unregister() { }
    }
}