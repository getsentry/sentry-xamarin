using Sentry.Xamarin.Forms.Internals;

namespace Sentry.Xamarin.Forms.Extensions
{
    internal static class SentryXamarinFormsIntegrationExtensions
    {
        internal static void UnregisterNativeIntegration(this SentryXamarinFormsIntegration integration, SentryXamarinOptions options)
        {
#if NATIVE_PROCESSOR
            integration?.Nativeintegration?.Unregister();
#endif
            options.NativeIntegrationEnabled = false;
        }
    }
}
