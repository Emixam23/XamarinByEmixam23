using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Map3DProject.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            Xamarin.FormsMaps.Init("dlMPnp3xnHQ1xvE5jO38~c7R01Zwoke-xSqETrtbR-w~Ah4hD3y9eAWegX41IAN1nzNeMrtcLy5Rr9blNr24ka_4W88iBGB25t2EPdDSF_CF");
            LoadApplication(new Map3DProject.App());
        }
    }
}
