namespace Sentry.Internals.Session
{
    internal class DeviceActiveLogger : IDeviceActiveLogger
    {
        /// <inheritdoc cref="IHub.PauseSession"/>
        public void StatePaused() => SentrySdk.PauseSession();

        /// <inheritdoc cref="IHub.ResumeSession"/>
        public void StateResumed() => SentrySdk.ResumeSession();
    }
}
