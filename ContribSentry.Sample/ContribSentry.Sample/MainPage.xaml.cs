using Sentry;
using System;
using Xamarin.Forms;

namespace ContribSentry.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Disco_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Disco());
        }

    }
}
