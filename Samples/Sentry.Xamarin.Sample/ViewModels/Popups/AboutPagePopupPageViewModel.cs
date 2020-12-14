using Sentry.Protocol;
using Sample.Xamarin.Core.Interfaces;
using Sample.Xamarin.Core.Views.Popups;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Browser = Xamarin.Essentials.Browser;
using Sentry;

namespace Sample.Xamarin.Core.ViewModels.Popups
{
    public class AboutPagePopupPageViewModel : ApplicationBridge
    {
        public Command PluginGithubCmd { get; }
        public Command BuyCmd { get; }

        public AboutPagePopupPageViewModel()
        {
            PluginGithubCmd = new Command(PluginGithub);
            BuyCmd = new Command(Buy);
        }

        private Action PluginGithub => async () =>
        {
            SentrySdk.AddBreadcrumb("plugin github", "ui.click", level: BreadcrumbLevel.Info);
            try
            {
                await Browser.OpenAsync(new Uri("https://github.com/rotorgames/Rg.Plugins.Popup"), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        };

        private Action Buy => () =>
        {
            SentrySdk.AddBreadcrumb("buy", "ui.click", level: BreadcrumbLevel.Info);
            ShowPopup(new PaymentPopupPage());
        };
    }
}
