using AppKit;

namespace Sample.Xamarin.Mac;

public class SampleViewController : NSViewController
{
    private SampleView _view;
    
    public override void LoadView()
    {
        _view = new SampleView();
        base.View = _view;
    }
}