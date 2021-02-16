using UIKit;

namespace Sentry.Xamarin.tvOS
{
    public static class AlertViewController
    {
        public static UIAlertController PresentOKAlert(string title, string description, UIViewController controller)
        {
            UIAlertController alert = UIAlertController.Create(title, description, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            controller.PresentViewController(alert, true, null);
            return alert;
        }

    }
}