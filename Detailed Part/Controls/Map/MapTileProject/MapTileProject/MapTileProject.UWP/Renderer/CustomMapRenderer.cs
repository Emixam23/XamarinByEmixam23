using MapTileProject.CustomControls;
using MapTileProject.UWP.Renderer;
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapTileProject.UWP.Renderer
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
        /// This function update the tiles of the Map for this plateform.
        /// </summary>
        private void UpdateTiles()
        {
            Debug.WriteLine("BEGINING !");
            HttpMapTileDataSource dataSource = new HttpMapTileDataSource();
            dataSource.UriRequested += DataSource_UriRequested;
            MapTileSource tileSource = new MapTileSource(dataSource);
            nativeMap.TileSources.Add(tileSource);
            Debug.WriteLine("END !");
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
