using Sentry.Protocol;
using Sample.Xamarin.Core.Interfaces;
using Sample.Xamarin.Core.Services;
using Sample.Xamarin.Core.Views;
using System;
using Xamarin.Forms;
using Sentry;

namespace Sample.Xamarin.Core.ViewModels
{
    public class LoginPageViewModel : NavigationService
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
                ReplacePage(new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromRgb(46, 14, 51)
                });
            }
            else
            {
                SentrySdk.AddBreadcrumb(authenticate.Item2, "console", level: BreadcrumbLevel.Warning);
                await DisplayAlert("warning", authenticate.Item2, "OK");
            }
        };
    }
}
