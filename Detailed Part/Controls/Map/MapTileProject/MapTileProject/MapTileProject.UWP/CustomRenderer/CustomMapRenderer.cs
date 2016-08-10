using MapTileProject.CustomControl;
using MapTileProject.UWP.CustomRenderer;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapTileProject.UWP.CustomRenderer
{
    /// <summary>
    /// CustomRenderer for the CustomMap created in the PCL part.
    /// This Renderer gives us the possibility to add/override some functionalities.
    /// </summary>
    public class CustomMapRenderer : MapRenderer
    {
        /// <summary>
        /// Instance of our Custom control declared in the PCL part.
        /// </summary>
        CustomMap customMap;
        /// <summary>
        /// Instance of the native map for this plateform.
        /// </summary>
        MapControl nativeMap;

        /// <summary>
        /// We override the OnElementChanged() event handler to get the desired instance. We also use it for updates.
        /// </summary>
        /// <param name="e">It contains either the NewElement or the OldElement</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customMap = e.NewElement as CustomMap;
                nativeMap = Control as MapControl;

                UpdateTiles();
            }
        }

        /// <summary>
        /// The on element property changed callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/>Instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomMap.MapTileTemplateProperty.PropertyName)
                UpdateTiles();
        }

        /// <summary>
        /// This function update the tiles of the Map for this plateform.
        /// </summary>
        private void UpdateTiles()
        {
            if (nativeMap != null)
            {
                if (this.customMap.MapTileTemplate != null)
                {
                    if (nativeMap.TileSources.Count > 0)
                    {
                        nativeMap.TileSources.Clear();
                        this.nativeMap.Style = MapStyle.Road;
                    }

                    this.nativeMap.Style = MapStyle.None;
                    HttpMapTileDataSource dataSource = new HttpMapTileDataSource();
                    dataSource.UriRequested += DataSource_UriRequested;
                    nativeMap.TileSources.Add(new MapTileSource(dataSource));
                }
            }
        }

        /// <summary>
        /// This function converts the basic url template value (x, y, z) into real values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataSource_UriRequested(HttpMapTileDataSource sender, MapTileUriRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            string urlTemplate = customMap.MapTileTemplate;

            //Here we write the code for creating the url.
            var url = urlTemplate.Replace("{z}", args.ZoomLevel.ToString()).Replace("{x}", args.X.ToString()).Replace("{y}", args.Y.ToString());
            args.Request.Uri = new Uri(url);

            deferral.Complete();
        }
    }
}
