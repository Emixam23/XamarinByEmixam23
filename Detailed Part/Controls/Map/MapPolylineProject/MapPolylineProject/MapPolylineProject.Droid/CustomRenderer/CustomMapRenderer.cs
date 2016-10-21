using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using MapPolylineProject.CustomControl;
using MapPolylineProject.Droid.CustomRenderer;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapPolylineProject.Droid.CustomRenderer
{
    /// <summary>
    /// CustomRenderer for the CustomMap created in the PCL part.
    /// This Renderer gives us the possibility to add/override some functionalities.
    /// </summary>
    public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        /// <summary>
        /// Instance of native control.
        /// </summary>
        GoogleMap map;

        /// <summary>
        /// Instance of our Custom control declared in the PCL part.
        /// </summary>
        CustomMap customMap;

        /// <summary>
        /// Polyline Renderer.
        /// </summary>
        Polyline polyline;

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
                ((MapView)Control).GetMapAsync(this);
            }

            UpdatePolyLine();
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
            if (map != null && ((CustomMap)this.Element).PolylineCoordinates != null)
            {
                if (polyline != null)
                {
                    polyline.Remove();
                    polyline.Dispose();
                }
                var polylineOptions = new PolylineOptions();

                polylineOptions.InvokeColor(((CustomMap)this.Element).PolylineColor.ToAndroid());
                polylineOptions.InvokeWidth((float)((CustomMap)this.Element).PolylineThickness);

                foreach (var position in ((CustomMap)this.Element).PolylineCoordinates)
                {
                    polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
                }

                polyline = map.AddPolyline(polylineOptions);
            }
        }

        /// <summary>
        /// Callback of end load of the Google Map.
        /// </summary>
        /// <param name="googleMap">Instance of the native control.</param>
        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            UpdatePolyLine();
        }
    }
}
