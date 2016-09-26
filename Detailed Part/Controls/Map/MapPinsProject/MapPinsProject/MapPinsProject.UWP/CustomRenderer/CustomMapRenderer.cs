using MapPinsProject.CustomControl;
using MapPinsProject.Models;
using MapPinsProject.UWP.CustomRenderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customMap = (CustomMap)e.NewElement;
                nativeMap = Control as MapControl;
                UpdatePins();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.CustomPinsProperty.PropertyName)
                UpdatePins();
            else if (e.PropertyName == CustomMap.ZoomLevelProperty.PropertyName)
                UpdatePins();
        }

        private async void UpdatePins()
        {
            if (customMap != null && nativeMap != null)
            {
                if (customMap.CustomPins != null && customMap.CustomPins.Count > 0)// && (customMap.VisibleRegion != null))
                {
                    List<MapIcon> pins = new List<MapIcon>();

                    var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Pin/CustomIconImage.png"));
                    RandomAccessStreamReference icon = await ResizeImage(file, Convert.ToUInt32(customMap.ZoomLevel.Kilometers / 2));

                    nativeMap.MapElements.Clear();
                    foreach (CustomPin pin in customMap.CustomPins)
                    {
                        nativeMap.MapElements.Add(new MapIcon()
                        {
                            Title = pin.Name,
                            Image = icon,
                            Location = new Geopoint(new BasicGeoposition() { Latitude = pin.Position.Latitude, Longitude = pin.Position.Longitude }),
                            NormalizedAnchorPoint = new Point(0.5, 1.0)
                        });
                    }
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
