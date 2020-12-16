using Rg.Plugins.Popup.Services;
using Sample.Xamarin.Core.Views;
using Sample.Xamarin.Core.Views.Popups;
using System;
using Xamarin.Forms;

namespace Sample.Xamarin.Core.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Disco_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DiscoPage());
        }

        private void Popup_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AboutPopupPage());
        }
        private void Xaml_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new XamlPageWithIssue());
        }
    }
}
