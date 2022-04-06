using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sentry.Xamarin.Forms.Internals
{
    internal class FormsApplicationListener
    {
        private readonly List<Action<Application>> _listeners = new List<Action<Application>>();

        private readonly SentryXamarinOptions _options;

        /// <summary>
        /// Class that returns the current application to any listener once available.
        /// </summary>
        /// <param name="options"> The sentry Xamarin options.</param>
        public FormsApplicationListener(SentryXamarinOptions options) => _options = options;

        /// <summary>
        ///  Adds a callback to a function that requires the Application.
        /// </summary>
        /// <param name="listener"> A function that requires the application in order to work properly.</param>
        public void AddListener(Action<Application> listener) => _listeners.Add(listener);

        /// <summary>
        /// Inokes a task that will wait for the initialization of Application.Current.
        /// On Success, it'll return the current application to all registered listeners. 
        /// </summary>
        public void Invoke() =>
            //Don't lock the main Thread while waiting for the current application to be created.
            Task.Run(async () =>
            {
                var application = await GetCurrentApplication().ConfigureAwait(false);
                if (application is null)
                {
                    _options.DiagnosticLogger?.Log(SentryLevel.Warning, "Sentry.Xamarin.Forms timeout for tracking Application.Current. Navigation tracking is going to be disabled");
                    return;
                }

                foreach(var hook in _listeners)
                {
                    hook.Invoke(application);
                }
            });

        /// <summary>
        /// Get the current Application.
        /// If SentrySDK was initialized from the Native project (Android/IOS) the Application might not have been created in time.
        /// So we wait for max 7 seconds to see check if it was created or not
        /// </summary>
        /// <returns>Current application.</returns>
        private async Task<Application> GetCurrentApplication()
        {
            for (int i = 0; i < _options.GetCurrentApplicationMaxRetries && Application.Current is null; i++)
            {
                await Task.Delay(_options.GetCurrentApplicationDelay).ConfigureAwait(false);
            }
            return Application.Current;
        }
    }
}
