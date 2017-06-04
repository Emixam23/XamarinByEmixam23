using Android.Content.Res;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using MapPinsProject.CustomControl;
using MapPinsProject.Droid.CustomRenderer;
using MapPinsProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using static Android.Gms.Maps.GoogleMap;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapPinsProject.Droid.CustomRenderer
{
    /// <summary>
    /// CustomRenderer for the CustomMap created in the PCL part.
    /// This Renderer gives us the possibility to add/override some functionalities.
    /// </summary>
    public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        /// <summary>
        /// Instance of our Custom control declared in the PCL part.
        /// </summary>
        CustomMap customMap;
        /// <summary>
        /// Instance of the native map for this plateform.
        /// </summary>
        GoogleMap nativeMap;

        /// <summary>
        /// Link between custom pins and their renderers.
        /// </summary>
        private Dictionary<Marker, CustomPin> MarkerOptionsPinLinkDictionary;

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

            if (e.PropertyName == CustomMap.CustomPinsProperty.PropertyName || e.PropertyName == CustomMap.ZoomLevelProperty.PropertyName)
                UpdatePins();
        }

        /// <summary>
        /// This function update the pins of the Map for this plateform.
        /// </summary>
        private void UpdatePins()
        {
            if (customMap != null && nativeMap != null)
            {
                if (customMap.CustomPins != null && customMap.CustomPins.Count > 0)
                {
                    nativeMap.Clear();
                    if (MarkerOptionsPinLinkDictionary == null)
                        MarkerOptionsPinLinkDictionary = new Dictionary<Marker, CustomPin>();
                    else
                        MarkerOptionsPinLinkDictionary.Clear();

                    CustomMap.AddPinsToRendererMap(customMap, addPins, addPins_AboutPinLimitOfZoom);
                }
            }
        }

        /// <summary>
        /// Basic function to add a pin.
        /// </summary>
        private void addPins()
        {
            foreach (CustomPin pin in customMap.CustomPins)
            {
                addMarker(pin);
            }
        }

        /// <summary>
        /// Basic function to add a pin but where each pin is add based on the zoom limit.
        /// </summary>
        /// <param name="currentZoom">The current zoom to determine if the pin can be add</param>
        private void addPins_AboutPinLimitOfZoom(double currentZoom)
        {
            foreach (CustomPin pin in customMap.CustomPins)
            {
                if (currentZoom >= pin.PinZoomVisibilityMinimumLimit && currentZoom <= pin.PinZoomVisibilityMaximumLimit)
                {
                    addMarker(pin);
                }
            }
        }

        /// <summary>
        /// This function customize the native pin based on the custom pin instance/object and then add it to the native map.
        /// </summary>
        /// <param name="pin">Custom pin object instance to add to the map.</param>
        private async void addMarker(CustomPin pin)
        {
            MarkerOptions marker = new MarkerOptions();

            marker.SetTitle(pin.Id);
            marker.SetIcon(BitmapDescriptorFactory.FromBitmap(ResizeImage(pin.ImagePath, ((customMap.PinSizeSource == CustomMap.PinSizeSourceName.Pin) ? (pin.PinSize) : (customMap.PinSize)))));
            marker.SetPosition(new LatLng(pin.Location.Latitude, pin.Location.Longitude));
            marker.Anchor((float)pin.AnchorPoint.X, (float)pin.AnchorPoint.Y);

            MarkerOptionsPinLinkDictionary.Add(nativeMap.AddMarker(marker), pin);
        }

        /// <summary>
        /// Function called when a pin get clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The element.</param>
        private void OnPinClicked(object sender, MarkerClickEventArgs e)
        {
            var item = this.MarkerOptionsPinLinkDictionary.FirstOrDefault(i => i.Value.Id.Equals(e.Marker.Title));
            CustomPin pin = item.Value;

            if (customMap.PinClickedCallbackSource == CustomMap.PinClickedCallbackSourceEnum.Map)
                customMap.PinClickedCallback(pin);
            else
                pin.PinClickedCallback(pin);
        }

        #region Additional functions
        /// <summary>
        /// This function only takes place on Android plateform.
        /// This function is the native callback called when the map is loaded.
        /// </summary>
        /// <param name="googleMap">The native map instance.</param>
        public void OnMapReady(GoogleMap googleMap)
        {
            this.nativeMap = googleMap;
            googleMap.UiSettings.ZoomControlsEnabled = false;
            nativeMap.MarkerClick += OnPinClicked;
            nativeMap.CameraChange += OnCameraChanged;
            customMap.ZoomLevel = new Distance(googleMap.CameraPosition.Zoom);
            customMap.MapLoaded();
            UpdatePins();
            //customMap.UpdateCamera();
        }
        /// <summary>
        /// Function called each time the user moves the camera.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The camera.</param>
        private void OnCameraChanged(object sender, CameraChangeEventArgs e)
        {
            customMap.ZoomLevel = new Distance(e.Position.Zoom);
        }
        /// <summary>
        /// Create a bitmap image from a given source and a given scale.
        /// </summary>
        /// <param name="imageSource">The image source string.</param>
        /// <param name="scale">The image size uint.</param>
        /// <returns>Return a Bitmap image created from a source string and scaled about the parameter scale.</returns>
        private Bitmap ResizeImage(string imageSource, uint scale)
        {
            AssetManager assetManager = MainActivity.MyAssets;
            Stream str;
            Bitmap bitmap = null;

            int markerSize = Convert.ToInt32(scale) * 2;

            str = assetManager.Open("Pin/" + imageSource);

            bitmap = BitmapFactory.DecodeStream(str);
            bitmap = Bitmap.CreateScaledBitmap(bitmap, markerSize, markerSize, false);

            return bitmap;
        }
        #endregion
    }
}