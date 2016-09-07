using MapPolylineProject.CustomControl;
using NightLine.UWP.CustomRenderers;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace NightLine.UWP.CustomRenderers
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
                UpdatePolyLine();
                SetUIButtonVisibility();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.PolylineCoordinatesProperty.PropertyName)
                UpdatePolyLine();
            else if (e.PropertyName == CustomMap.IsUIButtonVisibleProperty.PropertyName)
                SetUIButtonVisibility();
        }

        private void UpdatePolyLine()
        {
            if (customMap != null && nativeMap != null)
            {
                if (customMap.PolylineCoordinates != null && customMap.PolylineCoordinates.Count > 0)
                {
                    List<BasicGeoposition> coordinates = new List<BasicGeoposition>();

                    foreach (var position in customMap.PolylineCoordinates)
                    {
                        coordinates.Add(new BasicGeoposition() { Latitude = position.Latitude, Longitude = position.Longitude });
                    }

                    Geopath path = new Geopath(coordinates);
                    MapPolyline polyline = new MapPolyline();
                    polyline.StrokeColor = new Color()
                    {
                        A = (byte)(customMap.PolylineColor.A * 255),
                        R = (byte)(customMap.PolylineColor.R * 255),
                        G = (byte)(customMap.PolylineColor.G * 255),
                        B = (byte)(customMap.PolylineColor.B * 255)
                    };
                    polyline.StrokeThickness = customMap.PolylineThickness;
                    polyline.Path = path;
                    nativeMap.MapElements.Add(polyline);
                    nativeMap.UpdateLayout();
                    customMap.MoveToRegion();
                }
            }

        }

        private void SetUIButtonVisibility()
        {
            Control.ZoomInteractionMode = MapInteractionMode.GestureOnly;
        }
    }
}
