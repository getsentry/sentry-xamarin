using Sentry;
using System;
using Xamarin.Forms;

namespace ContribSentry.Sample
{
    public partial class Disco : ContentPage
    {
        public Disco()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void CaptureMessageClick(object sender, EventArgs e)
        {
            var sentryId = SentrySdk.CaptureMessage("Hello World Xamarin Forms");
            DisplayAlert("Hello World", $"EventId {sentryId}", "OK");
        }

        private void CaptureHandledClick(object sender, EventArgs e)
        {
            try
            {
                DoLogin("admin", "1234");
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        private void CaptureUnhandledClick(object sender, EventArgs e)
        {
            DoLogin("admin", "1234");
        }

        private void DoLogin(string user, string password)
        {
            ValidatePassword(user, password);
        }

        private bool ValidatePassword(string user, string password)
        {
            var validation = int.Parse(password) / user.Length;
            string notExpected = user;
            return validation != int.Parse(notExpected);
        }
    }
}