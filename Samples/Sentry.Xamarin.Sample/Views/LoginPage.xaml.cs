using ContribSentry.Sample;
using Sentry.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sentry.Xamarin.Sample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if(e.Value is true)
            {
                Application.Current.UserAppTheme = OSAppTheme.Dark;
            }
            else
            {
                Application.Current.UserAppTheme = OSAppTheme.Unspecified;
            }
        }

        private void Login_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine($"Lucas {LoginEntry.Text}");
            Console.WriteLine($"Lucas {PasswordEntry.Text}");
            SentrySdk.AddBreadcrumb("Login", "ui.click", level: BreadcrumbLevel.Info);
            if(LoginEntry.Text == "1234" && PasswordEntry.Text == "1234")
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                SentrySdk.AddBreadcrumb("invalid login", "console", level: BreadcrumbLevel.Warning);
                DisplayAlert("warning", "Wrong login or password.", "OK");
            }
        }
    }
}