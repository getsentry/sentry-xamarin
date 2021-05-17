namespace Sentry.Extensions
{
    internal static class SentryOptionsExtensions
    {
        public static IO.Sentry.SentryOptions ToSentryJavaOptions(this SentryOptions options)
        {
            var nativeOptions = new IO.Sentry.SentryOptions()
            {
                Environment = options.Environment,
                Release = options.Release,
                Dsn = options.Dsn,
                CacheDirPath = options.CacheDirectoryPath,
                SendDefaultPii = options.SendDefaultPii
            };
            nativeOptions.SetDebug(Java.Lang.Boolean.ValueOf(options.Debug));
            return nativeOptions;
        }

        public static IO.Sentry.Android.Core.SentryAndroidOptions ToSentryAndroidOptions(this SentryOptions options)
        {
            var androidOptions = new IO.Sentry.Android.Core.SentryAndroidOptions()
            {
                Environment = options.Environment,
                Release = options.Release,
                Dsn = options.Dsn,
                CacheDirPath = options.CacheDirectoryPath,
                AnrReportInDebug = options.Debug
            };
            androidOptions.SetDebug(Java.Lang.Boolean.ValueOf(options.Debug));
            return androidOptions;
        }
    }
}
