using CoreLocation;
using MapKit;
using MapPolylineProject.CustomControl;
using MapPolylineProject.iOS.CustomRenderer;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapPolylineProject.iOS.CustomRenderer
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
        /// Polyline Renderer.
        /// </summary>
        MKPolylineRenderer polylineRenderer;

        /// <summary>
        /// We override the OnElementChanged() event handler to get the desired instance. We also use it for updates.
        /// </summary>
        /// <param name="e">It contains either the NewElement or the OldElement</param>
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customMap = e.NewElement as CustomMap;
                nativeMap = Control as MKMapView;
                nativeMap.OverlayRenderer = null;
                
                UpdatePolyLine();
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
            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.PolylineCoordinatesProperty.PropertyName
                || e.PropertyName == CustomMap.PolylineColorProperty.PropertyName
                || e.PropertyName == CustomMap.PolylineThicknessProperty.PropertyName)
                UpdatePolyLine();
        }

        /// <summary>
        /// Draw the polyline from the PolylineCoordinates.
        /// </summary>
        private void UpdatePolyLine()
        {
            if (nativeMap != null)
            {
                var formsMap = ((CustomMap)this.Element);
                nativeMap = Control as MKMapView;

                nativeMap.OverlayRenderer = GetOverlayRenderer;

                CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[formsMap.PolylineCoordinates.Count];

                int index = 0;
                foreach (var position in formsMap.PolylineCoordinates)
                {
                    coords[index] = new CLLocationCoordinate2D(position.Latitude, position.Longitude);
                    index++;
                }

                var routeOverlay = MKPolyline.FromCoordinates(coords);
                nativeMap.AddOverlay(routeOverlay);
            }
        }

        #region Additional methods
        /// <summary>
        /// Return MKOverlay (Line of the Polyline) designed basing on the CustomMap instance properties.
        /// </summary>
        /// <param name="mapView">Instance of the MapView.</param>
        /// <param name="overlay">Interface of MKOverlay.</param>
        /// <returns></returns>
        private MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlay)
        {
            if (polylineRenderer == null)
            {
                polylineRenderer = new MKPolylineRenderer(overlay as MKPolyline);
                polylineRenderer.FillColor = customMap.PolylineColor.ToUIColor();
                polylineRenderer.StrokeColor = customMap.PolylineColor.ToUIColor();
                polylineRenderer.LineWidth = (float)customMap.PolylineThickness;
            }
            return polylineRenderer;
        }
        #endregion
    }
}
