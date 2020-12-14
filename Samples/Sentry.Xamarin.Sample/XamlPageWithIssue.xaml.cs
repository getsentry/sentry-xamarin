using Sentry;
using System;
using Xamarin.Forms;

namespace Sample.Xamarin.Core
{
    public partial class XamlPageWithIssue : ContentPage
    {
        public XamlPageWithIssue()
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
    }
}