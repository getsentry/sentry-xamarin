using Sample.Xamarin.Core.Interfaces;
using Sample.Xamarin.Core.Services;
using Sample.Xamarin.Core.ViewModels.Popups;
using Sample.Xamarin.Core.Views;
using Sample.Xamarin.Core.Views.Popups;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Sentry;

namespace Sample.Xamarin.Core.ViewModels
{
    public class MainPageViewModel : NavigationService
    {
        private readonly INativeCrash _nativeCrashService;
        public Command FeedbackCmd {get;}
        public Command HandledCmd { get; }
        public Command UnhandledCmd { get; }
        public Command DiscoCmd { get; }
        public Command PopupCmd { get; }
        public Command BrokenViewCmd { get; }
        public Command NativeCrashCmd { get; }
        public Command ImageSelectorCmd { get; }
        public MainPageViewModel()
        {
            DiscoCmd = new Command(GotoDisco);
            PopupCmd = new Command(ShowAboutPopup);
            HandledCmd = new Command(DoLogin);
            UnhandledCmd = new Command(Unhandle);
            BrokenViewCmd = new Command(GotoBrokenView);
            FeedbackCmd = new Command(ShowFeedback);
            NativeCrashCmd = new Command(NativeCrash);
            ImageSelectorCmd = new Command(ImageSelector);

            _nativeCrashService = DependencyService.Get<INativeCrash>();
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
            var lastEventId = SentrySdk.LastEventId;
            if (!lastEventId.Equals(SentryId.Empty))
            {
                await ShowPopup(new UserFeedbackPopupPage(), new UserFeedbackPopupPageViewModel(), new Dictionary<string, object> { { "SentryId", lastEventId } });
            }
            else
            {
                _ = DisplayAlert("Well", "Nothing broke so there's no need to give feedbacks...", "OK");
            }
        };

        private Action DoLogin => () =>
        {
            try
            {
                var authService = new AuthService();
                authService.DoLogin("admin", "1234");
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                DisplayAlert("Whoops", "A handled exception happened", "OK");
            }
        };

        private Action Unhandle => () =>
        {
            var authService = new AuthService();
            authService.DoLogin("admin", "1234");
        };

        private Action NativeCrash => () =>
        {
            _nativeCrashService?.BrokenNativeCallback();
            DisplayAlert("Whoops", "A native crash sample wasn't implemented for this platform, mind opening a pull request? :)", "Yes");
        };

        private Action ImageSelector => () =>
        {
            NavigateTo(new ImageSelectorPage());
        };
    }
}
