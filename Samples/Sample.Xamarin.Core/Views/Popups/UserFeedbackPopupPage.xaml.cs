using System.Collections.Generic;
using Xamarin.Forms.Xaml;

namespace Sample.Xamarin.Core.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserFeedbackPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public UserFeedbackPopupPage()
        {
            InitializeComponent();
        }
    }
}