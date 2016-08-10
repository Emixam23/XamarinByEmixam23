using MapPolylineProject.Droid.CustomRenderer;
using MapPolylineProject.CustomControl;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapPolylineProject.Droid.CustomRenderer
{
    public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        GoogleMap map;
        CustomMap customMap;
        Polyline polyline;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                customMap = e.NewElement as CustomMap;
                ((MapView)Control).GetMapAsync(this);
            }

            UpdatePolyLine();
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
            if (map != null)
            {
                if (polyline != null)
                {
                    polyline.Remove();
                    polyline.Dispose();
                }
                var polylineOptions = new PolylineOptions();
                //polylineOptions.InvokeColor(int.Parse(((CustomMap)this.Element).PolylineColor));
                polylineOptions.InvokeWidth(((CustomMap)this.Element).PolylineWidth);

                foreach (var position in ((CustomMap)this.Element).PolylineCoordinates)
                {
                    polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
                }

                polyline = map.AddPolyline(polylineOptions);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            map.UiSettings.ZoomControlsEnabled = false;

            UpdatePolyLine();
        }
    }
}