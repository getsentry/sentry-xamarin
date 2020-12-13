using Xamarin.Forms.Xaml;

namespace Sentry.Xamarin.Sample.Views.Popups
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