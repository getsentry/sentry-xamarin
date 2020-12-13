using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sentry.Xamarin.Sample.Resources
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrokenSvg : ContentView
    {
        public BrokenSvg()
        {
            InitializeComponent();
        }
    }
}