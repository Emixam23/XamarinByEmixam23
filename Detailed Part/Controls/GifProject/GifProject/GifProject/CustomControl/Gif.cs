using Foundation;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace GifProject.CustomControl
{
    public class Gif : WebView
    {
        public string GifSource
        {
            set
            {
                var html = new HtmlWebViewSource();

                string GifAssetURL;
                switch (Device.OS)
                {
                    case TargetPlatform.Android:
                        GifAssetURL = "file:///android_asset/";
                        break;
                    case TargetPlatform.iOS:
                        GifAssetURL = NSBundle.MainBundle.BundlePath;
                        break;
                    case TargetPlatform.Windows:
                        GifAssetURL = "ms-appx-web:///Assets/";
                        break;
                    default:
                        GifAssetURL = "";
                        break;
                }

                html.Html = String.Format(@"<html><body><img src='{0}' style='width:100%;height:100%;'/></body></html>", GifAssetURL + value);
                SetValue(SourceProperty, html);
                this.Margin = -10;
            }
        }
    }
}