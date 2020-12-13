using ContribSentry.Sample;
using ContribSentry.Sample.Views;
using Sentry.Protocol;
using Sentry.Xamarin.Sample.Interfaces;
using Sentry.Xamarin.Sample.Services;
using System;
using Xamarin.Forms;

namespace Sentry.Xamarin.Sample.ViewModels
{
    public class LoginPageViewModel : ApplicationBridge
    {
        private readonly AuthService _authService;
        public string Login { get; set; }
        public string Password { get; set; }
        public Command LoginCmd { get; }

        public LoginPageViewModel()
        {
            LoginCmd = new Command(LoginAction);
            _authService = new AuthService();
        }

        internal Action LoginAction => async () => 
        {
            SentrySdk.AddBreadcrumb("Login", "ui.click", level: BreadcrumbLevel.Info);
            var authenticate = _authService.Authenticate(Login, Password);
            if (authenticate.Item1)
                
            {
                ReplacePage(new NavigationPage(new MainPage()));
            }
            else
            {
                SentrySdk.AddBreadcrumb(authenticate.Item2, "console", level: BreadcrumbLevel.Warning);
                await DisplayAlert("warning", authenticate.Item2, "OK");
            }
        };
    }
}
