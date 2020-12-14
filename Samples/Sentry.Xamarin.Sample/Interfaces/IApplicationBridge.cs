using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Sentry.Xamarin.Sample.Rules;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sentry.Xamarin.Sample.Interfaces
{
    /// <summary>
    /// Hides the UI Tasks
    /// </summary>
    public class ApplicationBridge : ExtendedBindableObject
    {
        public static Task DisplayAlert(string title, string message, string cancel)
            => Application.Current.MainPage.DisplayAlert(title, message, cancel);

        public static void ReplacePage(Page page)
        {
            Application.Current.MainPage = page;
        }

        public static void NavigateTo(Page page)
        {
            Application.Current.MainPage.Navigation.PushAsync(page);                
        }

        public static Task ShowPopup(PopupPage popup)
            => PopupNavigation.Instance.PushAsync(popup);
    }
}