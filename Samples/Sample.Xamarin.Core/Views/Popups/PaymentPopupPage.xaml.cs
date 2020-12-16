using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace Sample.Xamarin.Core.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PaymentPopupPage()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                if (IsVisible)
                {
                    IsVisible = false;
                }
            });
        }
    }
}