using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Sample.Xamarin.Core.Rules;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.Xamarin.Core.Interfaces
{
    /// <summary>
    /// Simple navigation service that hides the direct access to the MainPage for navigation, but also, 
    /// simplifies passing argument to viewmodels.
    /// </summary>
    public class NavigationService : ExtendedBindableObject
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

        public static async Task ShowPopup(PopupPage popup, NavigationService viewModel, Dictionary<string, object> parameters)
        {
            await PopupNavigation.Instance.PushAsync(popup);
            popup.BindingContext = viewModel;
            viewModel.Initialize(parameters);
        }

        public static Task ClosePopup()
            => PopupNavigation.Instance.PopAsync();

        public virtual void Initialize(Dictionary<string, object> parameters) { }
    }
}