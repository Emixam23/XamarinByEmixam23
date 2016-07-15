using Xamarin;

namespace MapTileProject.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            FormsMaps.Init("dlMPnp3xnHQ1xvE5jO38~c7R01Zwoke-xSqETrtbR-w~Ah4hD3y9eAWegX41IAN1nzNeMrtcLy5Rr9blNr24ka_4W88iBGB25t2EPdDSF_CF");
            LoadApplication(new MapTileProject.App());
        }
    }
}
