using UIKit;

namespace Sentry.Xamarin.Sample.iOS
{
#pragma warning disable RCS1102 // Make class static.
    public class Application
#pragma warning restore RCS1102 // Make class static.
    {
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
