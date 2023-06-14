using System;
using AppKit;

namespace Sample.Xamarin.Mac;

public class SampleWindowController : NSWindowController
{
    private readonly SampleWindow _window;

    public SampleWindowController(IntPtr handle) : base(handle)
    {
        base.Window = _window = new SampleWindow();
    }

    public SampleWindowController()
    {
        base.Window = _window = new SampleWindow();
    }

    public new SampleWindow Window => _window;
}