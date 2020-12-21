using Sentry.Extensibility;

namespace Sentry.Xamarin.Forms.Internals
{
    internal interface INativeEventProcessor : ISentryEventProcessor
    {
        /// <summary>
        /// Informs if the Native Event Processor does any processing or not.
        /// </summary>
        bool Implemented { get; }

        /// <summary>
        /// The target name, Android, iOS, ...
        /// </summary>
        string TargetName { get; }
    }
}
