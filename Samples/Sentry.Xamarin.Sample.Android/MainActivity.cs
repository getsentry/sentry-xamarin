using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Sentry;
using Sentry.Xamarin.Forms;
using System;
using System.IO;
using Environment = System.Environment;

namespace ContribSentry.Sample.Droid
{
    [Activity(Label = "Sentry.Xamarin.Forms.Sample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sentry"))){
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sentry"));
            }
            SentrySdk.Init(o =>
            {
                o.Debug = true;
                o.CacheDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sentry");
                o.Dsn = "https://80aed643f81249d4bed3e30687b310ab@o447951.ingest.sentry.io/5428537";
                o.AddIntegration(new SentryXamarinFormsIntegration());
            });

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (!Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                base.OnBackPressed();
            }
        }
    }
}