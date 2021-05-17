namespace Sentry.Extensions
{
    internal static class SentryOptionsExtensions
    {
        public static void ApplyToSentryAndroidOptions(this SentryOptions options, IO.Sentry.Android.Core.SentryAndroidOptions javaOptions)
        {
            javaOptions.Environment = options.Environment;
            javaOptions.Release = options.Release;
            javaOptions.Dsn = options.Dsn;
            javaOptions.DiagnosticLevel = IO.Sentry.SentryLevel.Debug;
            javaOptions.CacheDirPath = options.CacheDirectoryPath;
            javaOptions.SendDefaultPii = options.SendDefaultPii;
            javaOptions.AnrReportInDebug = options.Debug;
            javaOptions.SetDebug(Java.Lang.Boolean.ValueOf(options.Debug));
        }
        
        public static IO.Sentry.Android.Core.SentryAndroidOptions ToSentryAndroidOptions(this SentryOptions options)
        {
            var androidOptions = new IO.Sentry.Android.Core.SentryAndroidOptions()
            {
                Environment = options.Environment,
                Release = options.Release,
                Dsn = options.Dsn,
                DiagnosticLevel = IO.Sentry.SentryLevel.Debug,
                CacheDirPath = options.CacheDirectoryPath,
                AnrReportInDebug = options.Debug
            };
            androidOptions.SetDebug(Java.Lang.Boolean.ValueOf(options.Debug));
            return androidOptions;
        }
    }
}
