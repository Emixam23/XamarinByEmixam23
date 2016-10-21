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
    /// <summary>
    /// CustomRenderer for the CustomMap created in the PCL part.
    /// This Renderer gives us the possibility to add/override some functionalities.
    /// </summary>
    public class CustomMapRenderer : MapRenderer
    {
        /// <summary>
        /// Instance of the native map for this plateform.
        /// </summary>
        MapControl nativeMap;

        /// <summary>
        /// Instance of our Custom control declared in the PCL part.
        /// </summary>
        CustomMap customMap;

        /// <summary>
        /// We override the OnElementChanged() event handler to get the desired instance. We also use it for updates.
        /// </summary>
        /// <param name="e">It contains either the NewElement or the OldElement</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customMap = (CustomMap)e.NewElement;
                nativeMap = Control as MapControl;
                UpdatePolyLine();
            }
        }

        /// <summary>
        /// The on element property changed callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/>Instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.PolylineCoordinatesProperty.PropertyName
                || e.PropertyName == CustomMap.PolylineColorProperty.PropertyName
                || e.PropertyName == CustomMap.PolylineThicknessProperty.PropertyName)
                UpdatePolyLine();
        }

        /// <summary>
        /// Draw the polyline from the PolylineCoordinates.
        /// </summary>
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
                }
            }
        }
    }
}
