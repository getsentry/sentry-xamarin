namespace Sentry.Internals.Session
{
    internal class DeviceActiveLogger
    {
        public void StatePaused() => SentrySdk.PauseSession();

        public void StateResumed() => SentrySdk.ResumeSession();
    }
}
