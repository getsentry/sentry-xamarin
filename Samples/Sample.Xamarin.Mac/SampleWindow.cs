using System;
using AppKit;
using CoreGraphics;

namespace Sample.Xamarin.Mac
{
    public class SampleWindow : NSWindow
    {
        private SampleViewController _sampleViewController;

        public SampleWindow(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public SampleWindow()
        {
            Initialize();
        }

        void Initialize()
        {
            SetFrame(new CGRect(0, 0, 1024, 768), true, true);
            StyleMask = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled | NSWindowStyle.FullSizeContentView;
            Title = "Sentry Xamarin.Mac Sample";

            _sampleViewController = new SampleViewController();
        
            ContentView.AutoresizesSubviews = true;
            ContentView.AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
            _sampleViewController.View.Frame = ContentView.Bounds;
            ContentView.AddSubview(_sampleViewController.View);
        }
    }
}