using Foundation;
using MapKit;
using MapTileProject.CustomControls;
using MapTileProject.iOS.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapTileProject.iOS.CustomRenderer
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
        MKMapView nativeMap;

        /// <summary>
        /// We override the OnElementChanged() event handler to get the desired instance. We also use it for updates.
        /// </summary>
        /// <param name="e">It contains either the NewElement or the OldElement</param>
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {

            }

            if (e.NewElement != null)
            {
                customMap = e.NewElement as CustomMap;
                nativeMap = Control as MKMapView;
                nativeMap.OverlayRenderer = null;

                UpdateTiles();
            }
        }

        /// <summary>
        /// This function update the tiles of the Map for this plateform.
        /// </summary>
        private void UpdateTiles()
        {
            var tileOverlay = new MKTileOverlay(customMap.MapTileTemplate);

            if (nativeMap != null)
            {
                nativeMap.OverlayRenderer = (MKMapView mapView, IMKOverlay overlay) =>
                {
                    var _tileOverlay = overlay as MKTileOverlay;

                    if (_tileOverlay != null)
                    {
                        return new MKTileOverlayRenderer(_tileOverlay);
                    }

                    return new MKOverlayRenderer(overlay);
                };
            }
            nativeMap.AddOverlay(tileOverlay);
        }

        /// <summary>
        /// This class converts the basic url template value (x, y, z) into real values.
        /// </summary>
        public class CustomTileOverlay : MKTileOverlay
        {
            private string urlTemplate;

            public override void LoadTileAtPath(MKTileOverlayPath path, MKTileOverlayLoadTileCompletionHandler result)
            {
                base.LoadTileAtPath(path, result);
            }

            public override NSUrl URLForTilePath(MKTileOverlayPath path)
            {
                //Here we write the code for creating the url.
                var url = urlTemplate.Replace("{z}", path.Z.ToString()).Replace("{x}", path.X.ToString()).Replace("{y}", path.Y.ToString());

                return new NSUrl(url);
            }
        }
    }
}