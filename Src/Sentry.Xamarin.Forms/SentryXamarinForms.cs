using Sentry.Xamarin.Forms.Internals;
using System;

namespace Sentry
{
    public static class SentryXamarinForms
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
            if (options is null)
            {
                options = new SentryXamarinOptions();
            }

            options.AddPageNavigationTrackerIntegration(new SentryXamarinFormsIntegration());
            SentryXamarin.Init(options);
        }
    }
}
