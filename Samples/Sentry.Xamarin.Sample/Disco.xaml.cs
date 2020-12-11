using Sentry;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContribSentry.Sample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Disco : ContentPage
    {
        public Disco()
        {
            InitializeComponent();
            SentrySdk.CaptureMessage("Hello World");
        }
    }
}