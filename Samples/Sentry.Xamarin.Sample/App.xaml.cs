using Sentry;
using Xamarin.Forms;

namespace ContribSentry.Sample
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            SentrySdk.AddBreadcrumb("OnStart", "app.lifecycle", "event");
            base.OnStart();
        }
        protected override void OnSleep()
        {
            SentrySdk.AddBreadcrumb("OnSleep", "app.lifecycle", "event");
            base.OnSleep();
        }
        protected override void OnResume()
        {
            SentrySdk.AddBreadcrumb("OnResume", "app.lifecycle", "event");
            base.OnResume();
        }
    }
}
