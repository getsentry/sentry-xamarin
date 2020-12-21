namespace Sentry.Xamarin.Forms.Extensions
{
    internal static class SentryXamarinFormsIntegrationExtensions
    {
        internal static void UnregisterNativeIntegration(this SentryXamarinFormsIntegration integration)
        {
#if NATIVE_INTEGRATION
            integration?.Nativeintegration?.Unregister();
#endif
            SentryXamarinFormsIntegration.Options.Value.NativeIntegrationEnabled = false;
        }
    }
}
