using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample.Xamarin.Core.Resources
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