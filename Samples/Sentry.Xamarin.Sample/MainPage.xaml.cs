using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Sentry;
using System;
using Xamarin.Forms;

namespace ContribSentry.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Disco_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Disco());
        }

        private void Popup_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PopupPage());
        }
    }
}
