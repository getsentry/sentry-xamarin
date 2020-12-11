using Xamarin.Forms.Xaml;

namespace Sentry.Xamarin.Sample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopupPage()
        {
            InitializeComponent();
        }
    }
}