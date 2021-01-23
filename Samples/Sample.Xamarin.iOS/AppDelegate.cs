using Foundation;
using Sample.Xamarin.Core;
using UIKit;

namespace Sentry.Xamarin.Sample.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            SentryXamarin.Init(options =>
            {
                options.Dsn = "https://5a193123a9b841bc8d8e42531e7242a1@o447951.ingest.sentry.io/5560112";
                options.AddXamarinFormsIntegration();
#if DEBUG
                options.Debug = true;
#endif
            });
            //SentryInitializer.Init();
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental");
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            var dict = new NSMutableDictionary();
            dict.LowlevelSetObject(System.IntPtr.Zero, System.IntPtr.Zero);

            return base.FinishedLaunching(app, options);
        }
    }
}
