using Sentry;

namespace Sample.Xamarin.Core.Helpers
{
    public static class SentryInitializer
    {
        public static void Init()
        {
            SentrySdk.Init(options =>
            {
                options.Dsn = "https://5a193123a9b841bc8d8e42531e7242a1@o447951.ingest.sentry.io/5560112";
#if DEBUG
                options.Debug = true;
#endif
            });
        }
    }
}
