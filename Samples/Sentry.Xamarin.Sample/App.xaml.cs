using Sentry;
using Sample.Xamarin.Core.Views;
using Xamarin.Forms;

namespace Sample.Xamarin.Core
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
