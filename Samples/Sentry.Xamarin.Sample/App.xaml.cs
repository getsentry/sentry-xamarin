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
