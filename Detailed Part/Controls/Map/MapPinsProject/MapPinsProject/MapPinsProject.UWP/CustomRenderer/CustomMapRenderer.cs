using MapPinsProject.CustomControl;
using MapPinsProject.Models;
using MapPinsProject.UWP.CustomRenderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapPinsProject.UWP.CustomRenderer
{
    public class CustomMapRenderer : MapRenderer
    {
        MapControl nativeMap;
        CustomMap customMap;

        private Dictionary<MapIcon, CustomPin> MapIconPinLinkDictionary;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customMap = (CustomMap)e.NewElement;
                nativeMap = Control as MapControl;
                MapIconPinLinkDictionary = null;
                nativeMap.MapElementClick += OnPinClicked;
                UpdatePins();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.CustomPinsProperty.PropertyName || e.PropertyName == CustomMap.ZoomLevelProperty.PropertyName)
                UpdatePins();
        }

        private void OnPinClicked(MapControl sender, MapElementClickEventArgs ea)
        {
            MapIcon mapIconClicked = ea.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            customMap.PinClickedCallback(MapIconPinLinkDictionary[mapIconClicked] as CustomPin);

            /*Debug.WriteLine("Element clicked !");
            Debug.WriteLine("{0}, {1}", MapIconPinLinkDictionary[mapIconClicked].Name, MapIconPinLinkDictionary[mapIconClicked].Details);*/
        }

        private void DisplayCustomPinInfo(CustomPin pin)
        {
            Debug.WriteLine("AnchorPoint", pin.AnchorPoint);
        }

        private void UpdatePins()
        {
            if (customMap != null && nativeMap != null)
            {
                if (customMap.CustomPins != null && customMap.CustomPins.Count > 0)
                {
                    nativeMap.MapElements.Clear();
                    if (MapIconPinLinkDictionary == null)
                        MapIconPinLinkDictionary = new Dictionary<MapIcon, CustomPin>();
                    else
                        MapIconPinLinkDictionary.Clear();

                    Debug.WriteLine("Current zoom is => {0}", customMap.ZoomLevel.Kilometers);

                    Debug.WriteLine("---- UPDATE PINS ----");
                    if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.None)
                    {
                        Debug.WriteLine("pin limit source none");
                        addPins();
                    }
                    else
                    {
                        if (customMap.PinZoomVisibilityLimitUnity == CustomMap.PinZoomVisibilityLimitUnityName.Kilometers)
                        {
                            Debug.WriteLine("pin limit unity kilometers");
                            if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Map && customMap.ZoomLevel.Kilometers < customMap.PinZoomVisibilityLimit)
                            {
                                Debug.WriteLine("pin limit source map");
                                addPins();
                            }
                            else if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Pin)
                            {
                                Debug.WriteLine("pin limit source pin");
                                addPins(customMap.ZoomLevel.Kilometers);
                            }
                        }
                        else if (customMap.PinZoomVisibilityLimitUnity == CustomMap.PinZoomVisibilityLimitUnityName.Meters)
                        {
                            Debug.WriteLine("pin limit unity meters");
                            if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Map && customMap.ZoomLevel.Meters < customMap.PinZoomVisibilityLimit)
                            {
                                Debug.WriteLine("pin limit source map");
                                addPins();
                            }
                            else if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Pin)
                            {
                                Debug.WriteLine("pin limit source pin");
                                addPins(customMap.ZoomLevel.Meters);
                            }
                        }
                        else if (customMap.PinZoomVisibilityLimitUnity == CustomMap.PinZoomVisibilityLimitUnityName.Miles)
                        {
                            Debug.WriteLine("pin limit unity miles");
                            if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Map && customMap.ZoomLevel.Miles < customMap.PinZoomVisibilityLimit)
                            {
                                Debug.WriteLine("pin limit source map");
                                addPins();
                            }
                            else if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Pin)
                            {
                                Debug.WriteLine("pin limit source pin");
                                addPins(customMap.ZoomLevel.Miles);
                            }
                        }
                    }
                    Debug.WriteLine("---- UPDATE PINS ----");
                }
            }
        }

        private async void addPins()
        {
            MapIcon mapIcon;

            foreach (CustomPin pin in customMap.CustomPins)
            {
                mapIcon = new MapIcon()
                {
                    Title = pin.Name,
                    Image = await ResizeImage(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Pin/" + pin.ImageSource)), pin.PinSize),
                    Location = new Geopoint(new BasicGeoposition() { Latitude = pin.Position.Latitude, Longitude = pin.Position.Longitude }),
                    NormalizedAnchorPoint = new Point(pin.AnchorPoint.X, pin.AnchorPoint.Y)
                };
                nativeMap.MapElements.Add(mapIcon);
                MapIconPinLinkDictionary.Add(mapIcon, pin);
            }
        }

        private async void addPins(double currentZoom)
        {
            MapIcon mapIcon;

            foreach (CustomPin pin in customMap.CustomPins)
            {
                if (pin.PinZoomVisibilityLimit < currentZoom)
                {
                    mapIcon = new MapIcon()
                    {
                        Title = pin.Name,
                        Image = await ResizeImage(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Pin/" + pin.ImageSource)), pin.PinSize),
                        Location = new Geopoint(new BasicGeoposition() { Latitude = pin.Position.Latitude, Longitude = pin.Position.Longitude }),
                        NormalizedAnchorPoint = new Point(pin.AnchorPoint.X, pin.AnchorPoint.Y)
                    };
                    nativeMap.MapElements.Add(mapIcon);
                    MapIconPinLinkDictionary.Add(mapIcon, pin);
                }
            }
        }

        #region Additional functions
        private async Task<RandomAccessStreamReference> ResizeImage(StorageFile imageFile, uint scale)
        {
            using (IRandomAccessStream fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(fileStream);

                //create a RandomAccessStream as output stream
                var memStream = new InMemoryRandomAccessStream();

                //creates a new BitmapEncoder and initializes it using data from an existing BitmapDecoder
                BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(memStream, decoder);

                //resize the image
                encoder.BitmapTransform.ScaledWidth = scale;
                encoder.BitmapTransform.ScaledHeight = scale;

                //commits and flushes all of the image data
                await encoder.FlushAsync();

                //return the output stream as RandomAccessStreamReference
                return RandomAccessStreamReference.CreateFromStream(memStream);
            }
        }
        #endregion
    }
}
