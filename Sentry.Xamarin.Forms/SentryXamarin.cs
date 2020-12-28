using Sentry.Xamarin.Forms;
using Sentry.Xamarin.Forms.Internals;
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
        internal static SentryXamarinOptions Options { get; set; }

        /// <summary>
        /// Initializes the SDK with an optional configuration options callback.
        /// </summary>
        /// <param name="configureOptions">The configure options.</param>
        public static void Init(Action<SentryOptions> configureOptions)
        {
            var options = new SentryOptions();
            configureOptions?.Invoke(options);
            Init(options);
        }

        /// <summary>
        /// Initializes the SDK with the specified options instance.
        /// </summary>
        /// <param name="options">The options instance</param>
        public static void Init(SentryOptions options)
        {
            Options = new SentryXamarinOptions();           
            options ??= new SentryOptions();

            options.ConfigureSentryXamarinOptions(Options);
            options.RegisterXamarinEventProcessors();
            options.AddIntegration(new SentryXamarinFormsIntegration(Options));

            SentrySdk.Init(options);
        }
    }
}
