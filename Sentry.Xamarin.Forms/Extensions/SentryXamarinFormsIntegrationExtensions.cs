namespace Sentry.Xamarin.Forms.Extensions
{
    internal static class SentryXamarinFormsIntegrationExtensions
    {
        internal static void UnregisterNativeIntegration(this SentryXamarinFormsIntegration integration)
        {
            integration.Nativeintegration?.Unregister();
        }
    }
}
