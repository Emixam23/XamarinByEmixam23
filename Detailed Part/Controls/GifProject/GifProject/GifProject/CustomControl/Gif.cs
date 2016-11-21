using GifProject.Interface;
using System;
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
                html.Html = String.Format(@"<html><body style='background: #000000;'><img src='{0}' style='width:100%;height:100%;'/></body></html>", DependencyService.Get<IBaseUrl>().Get() + value);
                SetValue(SourceProperty, html);
                this.Margin = -10;
            }
        }
    }
}