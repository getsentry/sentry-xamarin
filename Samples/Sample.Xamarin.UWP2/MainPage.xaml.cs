namespace Sample.Xamarin.uwp
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadApplication(new Core.App());
        }
    }
}
