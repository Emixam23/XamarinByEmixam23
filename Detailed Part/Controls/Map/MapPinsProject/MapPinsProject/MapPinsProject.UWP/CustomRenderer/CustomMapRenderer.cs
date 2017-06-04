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
        /// Link between custom pins and their renderers.
        /// </summary>
        private Dictionary<MapIcon, CustomPin> MapIconPinLinkDictionary;

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
                MapIconPinLinkDictionary = null;
                nativeMap.MapElementClick += OnPinClicked;
                nativeMap.Loaded += ((sender, re) =>
                {
                    customMap.MapLoaded();
                    //customMap.UpdateCamera();
                    UpdatePins();
                });
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
            {
                Debug.WriteLine("Zoom = {0}km", customMap.ZoomLevel.Kilometers);
                UpdatePins();
            }
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
                    nativeMap.MapElements.Clear();
                    if (MapIconPinLinkDictionary == null)
                        MapIconPinLinkDictionary = new Dictionary<MapIcon, CustomPin>();
                    else
                        MapIconPinLinkDictionary.Clear();

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
                addMapIcon(pin);
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
                    addMapIcon(pin);
                }
            }
        }

        /// <summary>
        /// This function customize the native pin based on the custom pin instance/object and then add it to the native map.
        /// </summary>
        /// <param name="pin">Custom pin object instance to add to the map.</param>
        private async void addMapIcon(CustomPin pin)
        {
            // Image path null/empty throws an exception ;)
            if ((customMap.PinImagePathSource == CustomMap.ImagePathSourceType.FromMap && customMap.PinImagePath == "")
                || (customMap.PinImagePathSource == CustomMap.ImagePathSourceType.FromPin && pin.ImagePath == "")
                || (pin.Location.Latitude == Double.MaxValue && pin.Location.Longitude == Double.MaxValue))
                return;

            MapIcon mapIcon = new MapIcon()
            {
                Title = pin.Id,
                Image = await ResizeImage(await StorageFile.GetFileFromApplicationUriAsync(
                    new Uri("ms-appx:///Assets/Pin/" + ((customMap.PinImagePathSource == CustomMap.ImagePathSourceType.FromMap) ? (customMap.PinImagePath) : (pin.ImagePath)))),
                    (customMap.PinSizeSource == CustomMap.PinSizeSourceName.Pin) ? (pin.PinSize) : (customMap.PinSize)),
                Location = new Geopoint(new BasicGeoposition() { Latitude = pin.Location.Latitude, Longitude = pin.Location.Longitude }),
                NormalizedAnchorPoint = new Windows.Foundation.Point(pin.AnchorPoint.X, pin.AnchorPoint.Y)
            };
            nativeMap.MapElements.Add(mapIcon);
            MapIconPinLinkDictionary.Add(mapIcon, pin);
        }

        /// <summary>
        /// Function called when a pin get clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="ea">The element.</param>
        private void OnPinClicked(MapControl sender, MapElementClickEventArgs ea)
        {
            MapIcon mapIconClicked = ea.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            CustomPin pin = (MapIconPinLinkDictionary[mapIconClicked] as CustomPin);

            if (customMap.PinClickedCallbackSource == CustomMap.PinClickedCallbackSourceEnum.Map)
                customMap.PinClickedCallback(pin);
            else
                pin.PinClickedCallback(pin);
        }

        #region Additional functions
        /// <summary>
        /// Resize a StorageFile to a RandomAccessStreamReference based on a scale parameter asynchronously.
        /// </summary>
        /// <param name="imageFile">The image under StorageFile type.</param>
        /// <param name="scale">The desired scale for the final output.</param>
        /// <returns>Return a RandomAccessStreamReference of the image scaled on the paramter scale.</returns>
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
