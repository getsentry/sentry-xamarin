using AppKit;
using CoreGraphics;
using Foundation;
using Sentry;

namespace Sample.Xamarin.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private SampleWindowController _windowController;

        public override void DidFinishLaunching(NSNotification notification)
        {
            SentryXamarin.Init(options =>
            {
                options.Dsn = "https://5a193123a9b841bc8d8e42531e7242a1@o447951.ingest.sentry.io/5560112";
                options.Debug = true;
            });
        
            _windowController = new SampleWindowController();

            var screenRect = NSScreen.MainScreen.VisibleFrame;
            var offsetFromLeft = 10;
            var offsetFromTop = 10;
            var offsetFromBottom = screenRect.GetMaxY() - _windowController.Window.Frame.Height - offsetFromTop;
        
            _windowController.Window.SetFrameOrigin(new CGPoint(offsetFromLeft, offsetFromBottom));
            _windowController.Window.MakeKeyAndOrderFront(this);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}