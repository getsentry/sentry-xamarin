using Sentry;
using Sentry.Xamarin.Forms;
using System;
using System.IO;

namespace Sample.Xamarin.Core.Helpers
{
    public static class SentryInitializer
    {
        public static void Init(bool enableCache = true)
        {
            var path = enableCache ? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) : null;
            SentrySdk.Init(o =>
            {
                o.CacheDirectoryPath = path;
                o.Debug = true;
                o.Dsn = "https://5a193123a9b841bc8d8e42531e7242a1@o447951.ingest.sentry.io/5560112";
                o.AddIntegration(new SentryXamarinFormsIntegration());
            });
        }
    }
}
