using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using MapUIOptionsProject.CustomControl;
using MapUIOptionsProject.Droid.CustomRenderer;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapUIOptionsProject.Droid.CustomRenderer
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
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customMap = e.NewElement as CustomMap;
                ((MapView)Control).GetMapAsync(this);
            }

            UpdateUIOptions();
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

            if (e.PropertyName == CustomMap.IsUIOptionsEnableProperty.PropertyName)
                UpdateUIOptions();
        }

        /// <summary>
        /// Show or Hide the UI options of the Map.
        /// </summary>
        private void UpdateUIOptions()
        {
            if (customMap.IsUIOptionsEnable)
                map.UiSettings.ZoomControlsEnabled = true;
            else
                map.UiSettings.ZoomControlsEnabled = false;
        }

        /// <summary>
        /// Callback of end load of the Google Map.
        /// </summary>
        /// <param name="googleMap">Instance of the native control.</param>
        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            UpdateUIOptions();
        }
    }
}