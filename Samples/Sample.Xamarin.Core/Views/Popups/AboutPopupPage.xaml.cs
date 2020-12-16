using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace Sample.Xamarin.Core.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public AboutPopupPage()
        {
            InitializeComponent();
        }

        private void Popup_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PaymentPopupPage());
        }
    }
}