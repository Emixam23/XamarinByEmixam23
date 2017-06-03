using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Map3DProject.CustomControl;
using Map3DProject.Droid.CustomRenderer;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Map3DProject.Droid.CustomRenderer
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
        /// We override the OnElementChanged() event handler to get the desired instance. We also use it for updates.
        /// </summary>
        /// <param name="e">It contains either the NewElement or the OldElement</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customMap = e.NewElement as CustomMap;
                ((MapView)Control).GetMapAsync(this);
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
        }

        private void UpdateCameraView()
        {
            // Create the camera
            CameraPosition cameraPosition = new CameraPosition.Builder()
                                                              .Target(new LatLng(customMap.Location.Latitude, customMap.Location.Longitude))
                                                              .Tilt(45)
                                                              .Zoom(10)
                                                              .Bearing(0)
                                                              .Build();
            // Convert to an update object
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            // Attach the camera
            map.MoveCamera(cameraUpdate); // map is of type GoogleMap
        }

        /// <summary>
        /// Callback of end load of the Google Map.
        /// </summary>
        /// <param name="googleMap">Instance of the native control.</param>
        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            UpdateCameraView();
        }
    }
}