using MapPinsProject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.CustomControl
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty CustomPinsProperty =
            BindableProperty.Create(nameof(CustomPins), typeof(ObservableCollection<CustomPin>), typeof(CustomMap), null,
                propertyChanged: OnCustomPinsPropertyChanged);
        public ObservableCollection<CustomPin> CustomPins
        {
            get { return (ObservableCollection<CustomPin>)GetValue(CustomPinsProperty); }
            set { SetValue(CustomPinsProperty, value); }
        }

        public static readonly BindableProperty CameraFocusProperty =
            BindableProperty.Create(nameof(CameraFocus), typeof(CameraFocusData), typeof(CustomMap), null);
        public CameraFocusData CameraFocus
        {
            get { return (CameraFocusData)GetValue(CameraFocusProperty); }
            set { SetValue(CameraFocusProperty, value); }
        }

        public enum CameraFocusReference
        {
            None,
            OnPins
        }
        public static readonly BindableProperty CameraFocusParameterProperty =
            BindableProperty.Create(nameof(CameraFocusParameter), typeof(CameraFocusReference), typeof(CustomMap), CameraFocusReference.None);
        public CameraFocusReference CameraFocusParameter
        {
            get { return (CameraFocusReference)GetValue(CameraFocusParameterProperty); }
            set { SetValue(CameraFocusParameterProperty, value); }
        }

        public static readonly BindableProperty PinSizeProperty =
            BindableProperty.Create(nameof(PinSize), typeof(uint), typeof(CustomMap), Convert.ToUInt32(50));
        public uint PinSize
        {
            get { return (uint)GetValue(PinSizeProperty); }
            set { SetValue(PinSizeProperty, value); }
        }

        public enum PinSizeSourceName
        {
            Map,
            Pin
        }
        public static readonly BindableProperty PinSizeSourceProperty =
            BindableProperty.Create(nameof(PinSizeSource), typeof(PinSizeSourceName), typeof(CustomMap), PinSizeSourceName.Map);
        public PinSizeSourceName PinSizeSource
        {
            get { return (PinSizeSourceName)GetValue(PinSizeSourceProperty); }
            set { SetValue(PinSizeSourceProperty, value); }
        }

        public static readonly BindableProperty PinImagePathProperty =
            BindableProperty.Create(nameof(PinImagePath), typeof(string), typeof(CustomMap), "");
        public string PinImagePath
        {
            get { return (string)GetValue(PinImagePathProperty); }
            set { SetValue(PinImagePathProperty, value); }
        }

        public enum ImagePathSourceType
        {
            FromMap,
            FromPin
        }
        public static readonly BindableProperty PinImagePathSourceProperty =
            BindableProperty.Create(nameof(PinImagePathSource), typeof(ImagePathSourceType), typeof(CustomMap), ImagePathSourceType.FromMap);
        public ImagePathSourceType PinImagePathSource
        {
            get { return (ImagePathSourceType)GetValue(PinImagePathSourceProperty); }
            set { SetValue(PinImagePathSourceProperty, value); }
        }

        public static readonly BindableProperty ZoomLevelProperty =
            BindableProperty.Create(nameof(ZoomLevel), typeof(Distance), typeof(CustomMap), new Distance());
        public Distance ZoomLevel
        {
            get { return (Distance)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        public static readonly BindableProperty PinZoomVisibilityMaximumLimitProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityMaximumLimit), typeof(uint), typeof(CustomMap), UInt32.MaxValue);
        public uint PinZoomVisibilityMaximumLimit
        {
            get { return (uint)GetValue(PinZoomVisibilityMaximumLimitProperty); }
            set { SetValue(PinZoomVisibilityMaximumLimitProperty, value); }
        }

        public static readonly BindableProperty PinZoomVisibilityMinimumLimitProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityMinimumLimit), typeof(uint), typeof(CustomMap), UInt32.MinValue);
        public uint PinZoomVisibilityMinimumLimit
        {
            get { return (uint)GetValue(PinZoomVisibilityMinimumLimitProperty); }
            set { SetValue(PinZoomVisibilityMinimumLimitProperty, value); }
        }

        public enum PinZoomVisibilityLimitUnityName
        {
            Kilometers,
            Meters,
            Miles
        }
        public static readonly BindableProperty PinZoomVisibilityLimitUnityProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityLimitUnity), typeof(PinZoomVisibilityLimitUnityName), typeof(CustomMap), PinZoomVisibilityLimitUnityName.Kilometers);
        public PinZoomVisibilityLimitUnityName PinZoomVisibilityLimitUnity
        {
            get { return (PinZoomVisibilityLimitUnityName)GetValue(PinZoomVisibilityLimitUnityProperty); }
            set { SetValue(PinZoomVisibilityLimitUnityProperty, value); }
        }

        public enum PinZoomVisibilityLimitSourceEnum
        {
            Map,
            None,
            Pin
        }
        public static readonly BindableProperty PinZoomVisibilityLimitSourceProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityLimitSource), typeof(PinZoomVisibilityLimitSourceEnum), typeof(CustomMap), PinZoomVisibilityLimitSourceEnum.None);
        public PinZoomVisibilityLimitSourceEnum PinZoomVisibilityLimitSource
        {
            get { return (PinZoomVisibilityLimitSourceEnum)GetValue(PinZoomVisibilityLimitSourceProperty); }
            set { SetValue(PinZoomVisibilityLimitSourceProperty, value); }
        }

        public static readonly BindableProperty PinClickedCallbackProperty =
            BindableProperty.Create(nameof(PinClickedCallback), typeof(Action<CustomPin>), typeof(CustomMap), null);
        public Action<CustomPin> PinClickedCallback
        {
            get { return (Action<CustomPin>)GetValue(PinClickedCallbackProperty); }
            set { SetValue(PinClickedCallbackProperty, value); }
        }

        public enum PinClickedCallbackSourceEnum
        {
            Map,
            Pins
        }
        public static readonly BindableProperty PinClickedCallbackSourceProperty =
            BindableProperty.Create(nameof(PinClickedCallbackSource), typeof(PinClickedCallbackSourceEnum), typeof(CustomMap), PinClickedCallbackSourceEnum.Map);
        public PinClickedCallbackSourceEnum PinClickedCallbackSource
        {
            get { return (PinClickedCallbackSourceEnum)GetValue(PinClickedCallbackSourceProperty); }
            set { SetValue(PinClickedCallbackSourceProperty, value); }
        }

        #region Constructor
        public CustomMap()
        {
            this.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                CustomMap map = sender as CustomMap;
                if (map.VisibleRegion != null)
                {
                    this.ZoomLevel = (map.VisibleRegion.Radius);
                }
            };
        }
        #endregion

        #region
        public async static Task<string> GetAddressName(Position position)
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/json";
            string additionnal_URL = "?latlng=" + position.Latitude + "," + position.Longitude
            + "&key=" + App.GOOGLE_MAP_API_KEY;

            JObject obj = await CustomMap.GoogleAPIHttpRequest(url, additionnal_URL);

            string address_name;
            try
            {
                address_name = (obj["results"][0]["formatted_address"]).ToString();
            }
            catch (Exception)
            {
                return ("");
            }
            return (address_name);
        }
        public async static Task<Position> GetAddressPosition(string name)
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/json";
            string additionnal_URL = "?address=" + name
            + "&key=" + App.GOOGLE_MAP_API_KEY;

            JObject obj = await CustomMap.GoogleAPIHttpRequest(url, additionnal_URL);

            Position position;
            try
            {
                position = new Position(Double.Parse((obj["results"][0]["geometry"]["location"]["lat"]).ToString()),
                                        Double.Parse((obj["results"][0]["geometry"]["location"]["lng"]).ToString()));
            }
            catch (Exception)
            {
                position = new Position();
            }
            return (position);
        }

        private static async Task<JObject> GoogleAPIHttpRequest(string url, string additionnal_URL)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(url);

                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(additionnal_URL, content);
                }
                catch (Exception)
                {
                    return (null);
                }
                string result = await response.Content.ReadAsStringAsync();
                if (result != null)
                {
                    try
                    {
                        return JObject.Parse(result);
                    }
                    catch (Exception)
                    {
                        return (null);
                    }
                }
                else
                {
                    return (null);
                }
            }
            catch (Exception)
            {
                return (null);
            }
        }
        #endregion

        #region Camera focus definition
        public class CameraFocusData
        {
            public Position Position { get; set; }
            public Distance Distance { get; set; }
        }
        private static void OnCustomPinsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap customMap = ((CustomMap)bindable);

            Debug.WriteLine("Pins collection has changed");
            if (customMap.CameraFocusParameter == CameraFocusReference.OnPins)
            {
                List<double> latitudes = new List<double>();
                List<double> longitudes = new List<double>();

                foreach (CustomPin pin in (newValue as ObservableCollection<CustomPin>))
                {
                    latitudes.Add(pin.Location.Latitude);
                    longitudes.Add(pin.Location.Longitude);
                }

                double lowestLat = latitudes.Min();
                double highestLat = latitudes.Max();
                double lowestLong = longitudes.Min();
                double highestLong = longitudes.Max();
                double finalLat = (lowestLat + highestLat) / 2;
                double finalLong = (lowestLong + highestLong) / 2;

                double distance = DistanceCalculation.GeoCodeCalc.CalcDistance(lowestLat, lowestLong, highestLat, highestLong, DistanceCalculation.GeoCodeCalcMeasurement.Kilometers);

                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(finalLat, finalLong), Distance.FromKilometers(distance * 0.7)));
            }
            Debug.WriteLine("Map pins collection setted !");
        }
        private class DistanceCalculation
        {
            public static class GeoCodeCalc
            {
                public const double EarthRadiusInMiles = 3956.0;
                public const double EarthRadiusInKilometers = 6367.0;

                public static double ToRadian(double val) { return val * (Math.PI / 180); }
                public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

                public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
                {
                    return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Miles);
                }

                public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
                {
                    double radius = GeoCodeCalc.EarthRadiusInMiles;

                    if (m == GeoCodeCalcMeasurement.Kilometers) { radius = GeoCodeCalc.EarthRadiusInKilometers; }
                    return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
                }
            }

            public enum GeoCodeCalcMeasurement : int
            {
                Miles = 0,
                Kilometers = 1
            }
        }
        #endregion
    }
}