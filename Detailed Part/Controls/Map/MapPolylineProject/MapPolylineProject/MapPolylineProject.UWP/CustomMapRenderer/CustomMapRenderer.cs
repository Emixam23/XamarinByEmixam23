using MapPolylineProject.CustomControl;
using NightLine.UWP.CustomRenderers;
using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace NightLine.UWP.CustomRenderers
{
    public class CustomMapRenderer : MapRenderer
    {
        MapControl nativeMap;
        CustomMap formsMap;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                formsMap = (CustomMap)e.NewElement;
                nativeMap = Control as MapControl;
                UpdatePolyLine();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.PolylineCoordinatesProperty.PropertyName)
            {
                UpdatePolyLine();
            }
        }

        private void UpdatePolyLine()
        {
            if (formsMap != null && formsMap.PolylineCoordinates.Count > 0)
            {
                List<BasicGeoposition> coordinates = new List<BasicGeoposition>();

                foreach (var position in formsMap.PolylineCoordinates)
                {
                    coordinates.Add(new BasicGeoposition() { Latitude = position.Latitude, Longitude = position.Longitude });
                }

                Geopath path = new Geopath(coordinates);
                MapPolyline polyline = new MapPolyline();
                polyline.StrokeColor = Color.FromArgb(
                    Convert.ToByte(formsMap.PolylineColor.A),
                    Convert.ToByte(formsMap.PolylineColor.R),
                    Convert.ToByte(formsMap.PolylineColor.G),
                    Convert.ToByte(formsMap.PolylineColor.B));
                polyline.StrokeThickness = 5;
                polyline.Path = path;
                nativeMap.MapElements.Add(polyline);
            }
        }
    }
}
