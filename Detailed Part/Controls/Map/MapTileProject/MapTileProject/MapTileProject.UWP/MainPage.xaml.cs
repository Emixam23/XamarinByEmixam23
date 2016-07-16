﻿using Xamarin;

namespace MapTileProject.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            FormsMaps.Init("YOUR_BING_MAP_KEY_UWP");
            LoadApplication(new MapTileProject.App());
        }
    }
}
