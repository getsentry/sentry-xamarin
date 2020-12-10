using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sentry.Xamarin.Sample
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