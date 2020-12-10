using Foundation;
using Sentry;
using Sentry.Xamarin.Forms;
using UIKit;

namespace ContribSentry.Sample.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            SentrySdk.Init(o =>
            {
                o.Dsn = new Dsn("https://80aed643f81249d4bed3e30687b310ab@o447951.ingest.sentry.io/5428537");
                o.AddIntegration(new ContribSentryFormsIntegration());
            });
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
