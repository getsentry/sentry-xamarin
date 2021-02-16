using System;
using Foundation;
using Sentry.Xamarin.tvOS.Services;
using UIKit;

namespace Sentry.Xamarin.tvOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void Message_Click(UIButton sender)
        {
            SentrySdk.CaptureMessage("Hello tvOS");
            AlertViewController.PresentOKAlert("Hi", "Hello tvOS", this);
        }

        partial void Handled_Exception_Click(UIButton sender)
        {
            try
            {
                var authService = new AuthService();
                authService.DoLogin("admin", "1234");
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                AlertViewController.PresentOKAlert("Error", "A handled exception happened", this);
            }
        }

        partial void Crash_Click(UIButton sender)
        {
            var authService = new AuthService();
            authService.DoLogin("admin", "1234");
        }

        partial void Native_Crash_Click(UIButton sender)
        {
            DoSomeObjectiveCCode();
        }

        private void DoSomeObjectiveCCode()
        {
            var dict = new NSMutableDictionary();
            dict.LowlevelSetObject(System.IntPtr.Zero, System.IntPtr.Zero);
        }
    }
}

