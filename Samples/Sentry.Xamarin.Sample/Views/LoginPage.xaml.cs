using Sample.Xamarin.Core;
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

namespace Sample.Xamarin.Core.Views
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
    }
}