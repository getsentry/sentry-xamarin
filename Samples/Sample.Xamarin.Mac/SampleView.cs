using System;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using Sentry;

namespace Sample.Xamarin.Mac;

public class SampleView : NSView
{
    private readonly List<NSView> _views = new List<NSView>();

    public SampleView()
    {
        InitializeButtons();
    }

    public override bool IsFlipped => true;

    public override void Layout()
    {
        var bounds = Bounds;

        if (bounds.IsEmpty) return;

        var y = 40;
        foreach (var view in _views)
        {
            view.Frame = new CGRect(10, y, 300, 28);
            y += 38;
        }
    }

    private void InitializeButtons()
    {
        AddButton("Throw managed exception (uncaught)", ThrowManagedExceptionUncaught);
        AddButton("Throw managed exception (caught)", ThrowManagedExceptionCaught);
        Layout();
    }

    private void ThrowManagedExceptionUncaught()
    {
        throw new Exception("Managed exception");
    }
    
    private void ThrowManagedExceptionCaught()
    {
        try
        {
            throw new Exception("Managed exception (caught)");
        }
        catch (Exception e)
        {
            SentrySdk.CaptureException(e);
        }
    }
  
    private void AddButton(string title, Action action)
    {
        var button = new NSButton();
        button.BezelStyle = NSBezelStyle.Rounded;
        button.SetButtonType(NSButtonType.MomentaryPushIn);
        button.Title = title;
        button.Activated += (_, _) =>
        {
            action.Invoke();
        };
      
        _views.Add(button);
        AddSubview(button);
    }
}