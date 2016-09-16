using MapPinsProject.CustomControl;
using MapPinsProject.Models;
using MapPinsProject.UWP.CustomRenderer;
using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
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

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.CustomPinsProperty.PropertyName)
                UpdatePins();
        }

        private void UpdatePins()
        {
            if (customMap != null && nativeMap != null)
            {
                if (customMap.CustomPins != null && customMap.CustomPins.Count > 0)
                {
                    List<MapIcon> pins = new List<MapIcon>();

                    foreach (CustomPin pin in customMap.CustomPins)
                    {
                        nativeMap.MapElements.Add(new MapIcon()
                        {
                            Title = pin.Name,
                            Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Pin/customicon.png")),
                            Location = new Geopoint(new BasicGeoposition() { Latitude = pin.Position.Latitude, Longitude = pin.Position.Longitude }),
                            NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0)
                        });
                    }
                }
            }
        }
    }
}
