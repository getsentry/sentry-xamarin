using Sentry;
using Sentry.Xamarin.Sample.Views;
using Xamarin.Forms;

namespace ContribSentry.Sample
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void OnSleep()
        {
            base.OnSleep();
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}
