using MapPinsProject.CustomControl;
using MapPinsProject.Models;
using MapPinsProject.UWP.CustomRenderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
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
                customMap = e.NewElement as CustomMap;
                nativeMap = Control as MapControl;
                MapIconPinLinkDictionary = null;
                nativeMap.MapElementClick += OnPinClicked;
                //UpdatePins();
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
            CustomPin pin = (MapIconPinLinkDictionary[mapIconClicked] as CustomPin);

            if (customMap.PinClickedCallbackSource == CustomMap.PinClickedCallbackSourceEnum.Map)
                customMap.PinClickedCallback(pin);
            else
                pin.PinClickedCallback(pin);
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

                    CustomMap.AddPinsToRendererMap(customMap, addPins, addPins_AboutPinLimitOfZoom);
                }
            }
        }

        private void addPins()
        {
            foreach (CustomPin pin in customMap.CustomPins)
            {
                addMapIcon(pin);
            }
        }

        private void addPins_AboutPinLimitOfZoom(double currentZoom)
        {
            foreach (CustomPin pin in customMap.CustomPins)
            {
                if (currentZoom >= pin.PinZoomVisibilityMinimumLimit && currentZoom <= pin.PinZoomVisibilityMaximumLimit)
                {
                    addMapIcon(pin);
                }
            }
        }

        private async void addMapIcon(CustomPin pin)
        {
            // Image path null/empty throws an exception ;)
            if ((customMap.PinImagePathSource == CustomMap.ImagePathSourceType.FromMap && customMap.PinImagePath == "")
                || (customMap.PinImagePathSource == CustomMap.ImagePathSourceType.FromPin && pin.ImagePath == "")
                || (pin.Location.Latitude == Double.MaxValue && pin.Location.Longitude == Double.MaxValue))
                return;

            MapIcon mapIcon = new MapIcon()
            {
                Title = pin.Name,
                Image = await ResizeImage(await StorageFile.GetFileFromApplicationUriAsync(
                    new Uri("ms-appx:///Assets/Pin/" + ((customMap.PinImagePathSource == CustomMap.ImagePathSourceType.FromMap) ? (customMap.PinImagePath) : (pin.ImagePath)))),
                    (customMap.PinSizeSource == CustomMap.PinSizeSourceName.Pin) ? (pin.PinSize) : (customMap.PinSize)),
                Location = new Geopoint(new BasicGeoposition() { Latitude = pin.Location.Latitude, Longitude = pin.Location.Longitude }),
                NormalizedAnchorPoint = new Windows.Foundation.Point(pin.AnchorPoint.X, pin.AnchorPoint.Y)
            };
            nativeMap.MapElements.Add(mapIcon);
            MapIconPinLinkDictionary.Add(mapIcon, pin);
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
