using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample.Xamarin.Core.Resources
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BugSvg : ContentView
    {
        public BugSvg()
        {
            InitializeComponent();
        }
    }
}