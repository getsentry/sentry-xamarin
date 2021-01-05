using Sentry.Xamarin;
using System;

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

        /// <summary>
        /// Initializes the SDK with an optional configuration options callback.
        /// </summary>
        /// <param name="configureOptions">The configure options.</param>
        public static void Init(Action<SentryXamarinOptions> configureOptions)
        {
            var options = new SentryXamarinOptions();
            configureOptions?.Invoke(options);
            Init(options);
        }

        /// <summary>
        /// Initializes the SDK with the specified options instance.
        /// </summary>
        /// <param name="options">The options instance</param>
        public static void Init(SentryXamarinOptions options)
        {
            if(options is null)
            {
                options = new SentryXamarinOptions();
            }

            options.ConfigureSentryXamarinOptions();
            options.RegisterNativeIntegrations();
            options.RegisterXamarinEventProcessors();
            options.RegisterXamarinInAppExclude();

            SentrySdk.Init(options);
        }
    }
}
