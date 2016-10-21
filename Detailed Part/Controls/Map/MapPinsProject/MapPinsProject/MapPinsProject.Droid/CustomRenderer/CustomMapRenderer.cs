using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using MapPinsProject.CustomControl;
using MapPinsProject.Droid.CustomRenderer;
using MapPinsProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

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
                    /*if (MapIconPinLinkDictionary == null)
                        MapIconPinLinkDictionary = new Dictionary<MapIcon, CustomPin>();
                    else
                        MapIconPinLinkDictionary.Clear();*/

                    if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.None)
                        addPins();
                    else
                    {
                        if (customMap.PinZoomVisibilityLimitUnity == CustomMap.PinZoomVisibilityLimitUnityName.Kilometers)
                        {
                            if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Map && customMap.ZoomLevel.Kilometers < customMap.PinZoomVisibilityLimit)
                                addPins();
                            else if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Pin)
                                addPins_AboutPinLimitOfZoom(customMap.ZoomLevel.Kilometers);
                        }
                        else if (customMap.PinZoomVisibilityLimitUnity == CustomMap.PinZoomVisibilityLimitUnityName.Meters)
                        {
                            if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Map && customMap.ZoomLevel.Meters < customMap.PinZoomVisibilityLimit)
                                addPins();
                            else if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Pin)
                                addPins_AboutPinLimitOfZoom(customMap.ZoomLevel.Meters);
                        }
                        else if (customMap.PinZoomVisibilityLimitUnity == CustomMap.PinZoomVisibilityLimitUnityName.Miles)
                        {
                            if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Map && customMap.ZoomLevel.Miles < customMap.PinZoomVisibilityLimit)
                                addPins();
                            else if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Pin)
                                addPins_AboutPinLimitOfZoom(customMap.ZoomLevel.Miles);
                        }
                    }
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
                if (currentZoom < pin.PinZoomVisibilityLimit)
                {
                    addMarker(pin);
                }
            }
        }

        private async void addMarker(CustomPin pin)
        {
            MarkerOptions marker = new MarkerOptions();

            marker.SetTitle(pin.Name);
            marker.SetIcon(BitmapDescriptorFactory.fromBitmap(ResizeImage("file:///android_asset/" + pin.ImageSource, ((customMap.PinSizeSource == CustomMap.PinSizeSourceName.Pin) ? (pin.PinSize) : (customMap.PinSize)))));
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.Anchor((float)pin.AnchorPoint.X, (float)pin.AnchorPoint.Y);

            MarkerOptionsPinLinkDictionary.Add(nativeMap.AddMarker(marker), pin);
        }

        /// <summary>
        /// This function only takes place on Android plateform. This function is the native callback called when the map is loaded.
        /// </summary>
        /// <param name="googleMap"></param>
        public void OnMapReady(GoogleMap googleMap)
        {
            this.nativeMap = googleMap;
            googleMap.UiSettings.ZoomControlsEnabled = false;

            UpdatePins();
        }

        #region Additional functions
        private Bitmap ResizeImage(string imageSource, uint scale)
        {
            Uri uri = new Uri(imageSource);

            Bitmap source = new Bitmap();
            source.BeginInit();
            source.UriSource = uri;
            source.DecodePixelHeight = scale;
            source.DecodePixelWidth = scale;
            source.EndInit();

            return source;
        }
        #endregion
    }
}