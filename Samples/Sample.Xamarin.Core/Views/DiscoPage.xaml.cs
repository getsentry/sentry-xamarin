using Sentry;
using Sentry.Protocol;
using System;
using System.Threading;
using Xamarin.Forms;

namespace Sample.Xamarin.Core.Views
{
    public partial class DiscoPage : ContentPage
    {
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private CancellationToken _token;


        private Color _originalBackgroundColor { get; set; }
        public DiscoPage()
        {
            try
            {
                InitializeComponent();
                PropertyChanged += DiscoPage_PropertyChanged;
                _token = _tokenSource.Token;

                var navigationPage = (Application.Current.MainPage as NavigationPage);

                _originalBackgroundColor = navigationPage.BarBackgroundColor;

                var background = navigationPage.BarBackgroundColor;
                var red = Color.Red;
                var green = Color.Green;
                var blue = Color.Blue;
                var gold = Color.Gold;
                var white = Color.White;

                var animation = new Animation();
                //Label
                animation.Add(0, 0.15, new Animation(c => navigationPage.BarBackgroundColor = Color.FromRgba(background.R + c * (red.R - background.R),
                                                                                                background.G + c * (red.G - background.G),
                                                                                                background.B + c * (red.B - background.B),
                                                                                                background.A + c * (red.A - background.A)), 0, 1.0));
                animation.Add(0.15, 0.30, new Animation(c => navigationPage.BarBackgroundColor = Color.FromRgba(red.R + c * (green.R - red.R),
                                                                                                red.G + c * (green.G - red.G),
                                                                                                red.B + c * (green.B - red.B),
                                                                                                red.A + c * (green.A - red.A)), 0, 1.0));
                animation.Add(0.30, 0.45, new Animation(c => navigationPage.BarBackgroundColor = Color.FromRgba(green.R + c * (blue.R - green.R),
                                                                                                green.G + c * (blue.G - green.G),
                                                                                                green.B + c * (blue.B - green.B),
                                                                                                green.A + c * (blue.A - green.A)), 0, 1.0));
                animation.Add(0.45, 0.60, new Animation(c => navigationPage.BarBackgroundColor = Color.FromRgba(blue.R + c * (gold.R - blue.R),
                                                                                                blue.G + c * (gold.G - blue.G),
                                                                                                blue.B + c * (gold.B - blue.B),
                                                                                                blue.A + c * (gold.A - blue.A)), 0, 1.0));
                animation.Add(0.60, 0.75, new Animation(c => navigationPage.BarBackgroundColor = Color.FromRgba(gold.R + c * (white.R - gold.R),
                                                                                                gold.G + c * (white.G - gold.G),
                                                                                                gold.B + c * (white.B - gold.B),
                                                                                                gold.A + c * (white.A - gold.A)), 0, 1.0));
                animation.Add(0.75, 1.0, new Animation(c => navigationPage.BarBackgroundColor = Color.FromRgba(white.R + c * (background.R - white.R),
                                                                                                white.G + c * (background.G - white.G),
                                                                                                white.B + c * (background.B - white.B),
                                                                                                white.A + c * (background.A - white.A)), 0, 1.0));
                animation.Commit(this,
                    "DanceColor",
                    160,
                    1000,
                    finished: (a, b) =>
                    {
                        navigationPage.BarBackgroundColor = _originalBackgroundColor;
                    },
                    repeat: () => !_token.IsCancellationRequested);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private bool _isGoingBack = false;
        private void DiscoPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Parent")
            {
                if (!_isGoingBack)
                {
                    _isGoingBack = true;
                    return;
                }
                _tokenSource.Cancel();
                ViewExtensions.CancelAnimations(this);
            }
        }
        private void StartStop_Click(object sender, EventArgs e)
        {
            SentrySdk.AddBreadcrumb("Start/Stop", "ui.click", level: BreadcrumbLevel.Info);
            DiscoGif.IsAnimationPlaying = !DiscoGif.IsAnimationPlaying;
        }

        private void HelloWorld_Click(object sender, EventArgs e)
        {
            SentrySdk.AddBreadcrumb("Hello World", "ui.click", level: BreadcrumbLevel.Info);
            var message = "Hello World Xamarin Forms!";
            SentrySdk.CaptureMessage(message);
            DisplayAlert("Hey", message, "OK");
        }        
    }
}