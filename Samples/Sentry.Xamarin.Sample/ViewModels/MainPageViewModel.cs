using ContribSentry.Sample;
using ContribSentry.Sample.Views;
using Sentry.Xamarin.Sample.Interfaces;
using Sentry.Xamarin.Sample.Views;
using Sentry.Xamarin.Sample.Views.Popups;
using System;
using Xamarin.Forms;

namespace Sentry.Xamarin.Sample.ViewModels
{
    public class MainPageViewModel : ApplicationBridge
    {
        public Command FeedbackCmd {get;}
        public Command HandledCmd { get; }
        public Command UnhandledCmd { get; }
        public Command DiscoCmd { get; }
        public Command PopupCmd { get; }
        public Command BrokenViewCmd { get; }

        public MainPageViewModel()
        {

            DiscoCmd = new Command(GotoDisco);
            PopupCmd = new Command(ShowAboutPopup);
            BrokenViewCmd = new Command(GotoBrokenView);
            FeedbackCmd = new Command(ShowFeedback);
        }

        private Action GotoDisco => () =>
        {
            NavigateTo(new DiscoPage());
        };

        private Action GotoBrokenView => () =>
        {
            NavigateTo(new XamlPageWithIssue());
        };

        private Action ShowAboutPopup => async () =>
        {
            await ShowPopup(new AboutPopupPage());
        };

        private Action ShowFeedback => async () =>
        {
            await ShowPopup(new UserFeedbackPopupPage());
        };

    }
}
