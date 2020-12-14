using Sentry;
using Sentry.Xamarin.Forms;
using System;
using System.IO;

namespace Sample.Xamarin.Core.Helpers
{
    public static class SentryInitializer
    {
        public static void Init()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sentry");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            SentrySdk.Init(o =>
            {
                o.CacheDirectoryPath = path;
                o.Dsn = "https://80aed643f81249d4bed3e30687b310ab@o447951.ingest.sentry.io/5428537";
                o.AddIntegration(new SentryXamarinFormsIntegration());
            });
        }
    }
}
