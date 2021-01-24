

using Foundation;
using Sample.Xamarin.Core.Interfaces;
using Sample.Xamarin.iOS.Dependencies;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeCrash))]
namespace Sample.Xamarin.iOS.Dependencies
{
    public class NativeCrash : INativeCrash
    {
        public void BrokenNativeCallback()
        {
            var dict = new NSMutableDictionary();
            dict.LowlevelSetObject(System.IntPtr.Zero, System.IntPtr.Zero);
        }
    }
}