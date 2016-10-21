namespace MapPinsProject.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            Xamarin.FormsMaps.Init("YOUR_BING_KEY");
            LoadApplication(new MapPinsProject.App());
        }
    }
}
