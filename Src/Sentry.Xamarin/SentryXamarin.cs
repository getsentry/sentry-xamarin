using System;
using Xamarin.Essentials;

namespace Sentry
{
    /// <summary>
    /// Sentry Xamarin SDK entrypoint.
    /// </summary>
    /// <remarks>
    /// This is a facade to the SDK instance that also Initializes Sentry .NET SDK.
    /// use SentrySdk for additional operations (like capturing exceptions, messages,...).
    /// </remarks>
    public static class SentryXamarin
    {

        internal static readonly string ProtocolPackageName = "sentry.dotnet.xamarin";

        /// <summary>
        /// Initializes the SDK with an optional configuration options callback.
        /// </summary>
        /// <param name="configureOptions">The configure options.</param>
        public static void Init(Action<SentryXamarinOptions> configureOptions)
        {
            if (SentrySdk.IsEnabled)
            {
                Console.Error.WriteLine("SentryXamarin.Init was called again. It should only be called once. Any change to options will be ignored.");
                return;
            }

            var options = new SentryXamarinOptions();
            // Set the release now but give the user a chance to reset it (i.e: to null to rely on the built-in format)
            options.Release = $"{AppInfo.PackageName}@{AppInfo.VersionString}+{AppInfo.BuildString}";

            configureOptions?.Invoke(options);

            options.ConfigureSentryXamarinOptions();
            options.RegisterNativeActivityStatus();
            options.RegisterNativeIntegrations();
            options.RegisterXamarinEventProcessors();
            options.RegisterXamarinInAppExclude();
            options.ProtocolPackageName ??= ProtocolPackageName;

            Init(options);
        }

        /// <summary>
        /// Initializes the SDK with the specified options instance.
        /// </summary>
        /// <param name="options">The options instance</param>
        public static void Init(SentryXamarinOptions options)
        {
            if (SentrySdk.IsEnabled)
            {
                Console.Error.WriteLine("SentryXamarin.Init was called again. It should only be called once. Any change to options will be ignored.");
                return;
            }

            SentrySdk.Init(options);

            options.RegisterScreenshotEventProcessor();
        }
    }
}
