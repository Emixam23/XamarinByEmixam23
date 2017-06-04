using MapPinsProject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.CustomControl
{
    public class CustomMap : Map, INotifyPropertyChanged
    {
        /// <summary>
        /// Handler for event of updating or changing the 
        /// </summary>
        new public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Pins collection property.
        /// </summary>
        public static readonly BindableProperty CustomPinsProperty =
            BindableProperty.Create(nameof(CustomPins), typeof(ObservableCollection<CustomPin>), typeof(CustomMap), null, BindingMode.TwoWay,
                propertyChanged: OnCustomPinsPropertyChanged);

        /// <summary>
        /// Assessor for CustomPins property.
        /// </summary>
        public ObservableCollection<CustomPin> CustomPins
        {
            get { return (ObservableCollection<CustomPin>)GetValue(CustomPinsProperty); }
            set { SetValue(CustomPinsProperty, value); }
        }

        /// <summary>
        /// Camera focus reference enumerator.
        /// </summary>
        public enum CameraFocusReference
        {
            None,
            OnPins
        }

        /// <summary>
        /// Camera focus parameter property.
        /// </summary>
        public static readonly BindableProperty CameraFocusParameterProperty =
            BindableProperty.Create(nameof(CameraFocusParameter), typeof(CameraFocusReference), typeof(CustomMap), CameraFocusReference.OnPins);

        /// <summary>
        /// Assessor for CameraFocusParameter property.
        /// </summary>
        public CameraFocusReference CameraFocusParameter
        {
            get { return (CameraFocusReference)GetValue(CameraFocusParameterProperty); }
            set { SetValue(CameraFocusParameterProperty, value); }
        }

        /// <summary>
        /// Global pin size property.
        /// </summary>
        public static readonly BindableProperty PinSizeProperty =
            BindableProperty.Create(nameof(PinSize), typeof(uint), typeof(CustomMap), Convert.ToUInt32(50));

        /// <summary>
        /// Assessor for PinSize property.
        /// </summary>
        public uint PinSize
        {
            get { return (uint)GetValue(PinSizeProperty); }
            set { SetValue(PinSizeProperty, value); }
        }

        /// <summary>
        /// Pin's size source name enumerator.
        /// </summary>
        public enum PinSizeSourceName
        {
            Map,
            Pin
        }

        /// <summary>
        /// Pin's size source property.
        /// </summary>
        public static readonly BindableProperty PinSizeSourceProperty =
            BindableProperty.Create(nameof(PinSizeSource), typeof(PinSizeSourceName), typeof(CustomMap), PinSizeSourceName.Map);

        /// <summary>
        /// Assessor for PinSizeSource property.
        /// </summary>
        public PinSizeSourceName PinSizeSource
        {
            get { return (PinSizeSourceName)GetValue(PinSizeSourceProperty); }
            set { SetValue(PinSizeSourceProperty, value); }
        }

        /// <summary>
        /// Pin's image path property.
        /// </summary>
        public static readonly BindableProperty PinImagePathProperty =
            BindableProperty.Create(nameof(PinImagePath), typeof(string), typeof(CustomMap), "");

        /// <summary>
        /// Assessor for PinImagePath property.
        /// </summary>
        public string PinImagePath
        {
            get { return (string)GetValue(PinImagePathProperty); }
            set { SetValue(PinImagePathProperty, value); }
        }

        /// <summary>
        /// Image path source enumerator.
        /// </summary>
        public enum ImagePathSourceType
        {
            FromMap,
            FromPin
        }
        /// <summary>
        /// Pin image path source property.
        /// </summary>
        public static readonly BindableProperty PinImagePathSourceProperty =
            BindableProperty.Create(nameof(PinImagePathSource), typeof(ImagePathSourceType), typeof(CustomMap), ImagePathSourceType.FromMap);

        /// <summary>
        /// Assessor for PinImagePathSource property.
        /// </summary>
        public ImagePathSourceType PinImagePathSource
        {
            get { return (ImagePathSourceType)GetValue(PinImagePathSourceProperty); }
            set { SetValue(PinImagePathSourceProperty, value); }
        }

        /// <summary>
        /// Zoom level property.
        /// </summary>
        public static readonly BindableProperty ZoomLevelProperty =
            BindableProperty.Create(nameof(ZoomLevel), typeof(Distance), typeof(CustomMap), new Distance());

        /// <summary>
        /// Assessor for ZoomLevel property.
        /// </summary>
        public Distance ZoomLevel
        {
            get { return (Distance)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        /// <summary>
        /// Pin's zoom visibility Maximum limit property.
        /// </summary>
        public static readonly BindableProperty PinZoomVisibilityMaximumLimitProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityMaximumLimit), typeof(uint), typeof(CustomMap), UInt32.MaxValue);

        /// <summary>
        /// Assessor for PinZoomVisibilityMaximumLimit property.
        /// </summary>
        public uint PinZoomVisibilityMaximumLimit
        {
            get { return (uint)GetValue(PinZoomVisibilityMaximumLimitProperty); }
            set { SetValue(PinZoomVisibilityMaximumLimitProperty, value); }
        }

        /// <summary>
        /// Pin's zoom visibility Minimum limit property.
        /// </summary>
        public static readonly BindableProperty PinZoomVisibilityMinimumLimitProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityMinimumLimit), typeof(uint), typeof(CustomMap), UInt32.MinValue);

        /// <summary>
        /// Assessor for PinZoomVisibilityMinimumLimit property.
        /// </summary>
        public uint PinZoomVisibilityMinimumLimit
        {
            get { return (uint)GetValue(PinZoomVisibilityMinimumLimitProperty); }
            set { SetValue(PinZoomVisibilityMinimumLimitProperty, value); }
        }

        /// <summary>
        /// Pin's zoom visibility limit unity enumerator. 
        /// </summary>
        public enum PinZoomVisibilityLimitUnityEnum
        {
            Kilometers,
            Meters,
            Miles
        }

        /// <summary>
        /// Pin's zoom visibility limit unity property.
        /// </summary>
        public static readonly BindableProperty PinZoomVisibilityLimitUnityProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityLimitUnity), typeof(PinZoomVisibilityLimitUnityEnum), typeof(CustomMap), PinZoomVisibilityLimitUnityEnum.Kilometers);

        /// <summary>
        /// Assessor for PinZoomVisibilityLimitUnity property.
        /// </summary>
        public PinZoomVisibilityLimitUnityEnum PinZoomVisibilityLimitUnity
        {
            get { return (PinZoomVisibilityLimitUnityEnum)GetValue(PinZoomVisibilityLimitUnityProperty); }
            set { SetValue(PinZoomVisibilityLimitUnityProperty, value); }
        }

        /// <summary>
        /// Pin's zoom visibility limit source enumerator.
        /// </summary>
        public enum PinZoomVisibilityLimitSourceEnum
        {
            Map,
            None,
            Pin
        }

        /// <summary>
        /// Pin's zoom visibility limit source property.
        /// </summary>
        public static readonly BindableProperty PinZoomVisibilityLimitSourceProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityLimitSource), typeof(PinZoomVisibilityLimitSourceEnum), typeof(CustomMap), PinZoomVisibilityLimitSourceEnum.None);

        /// <summary>
        /// Assessor for PinZoomVisibilityLimitSource property.
        /// </summary>
        public PinZoomVisibilityLimitSourceEnum PinZoomVisibilityLimitSource
        {
            get { return (PinZoomVisibilityLimitSourceEnum)GetValue(PinZoomVisibilityLimitSourceProperty); }
            set { SetValue(PinZoomVisibilityLimitSourceProperty, value); }
        }

        /// <summary>
        /// Pin's clicked callback property.
        /// </summary>
        public static readonly BindableProperty PinClickedCallbackProperty =
            BindableProperty.Create(nameof(PinClickedCallback), typeof(Action<CustomPin>), typeof(CustomMap), null);

        /// <summary>
        /// Assessor for PinClickedCallback property.
        /// </summary>
        public Action<CustomPin> PinClickedCallback
        {
            get { return (Action<CustomPin>)GetValue(PinClickedCallbackProperty); }
            set { SetValue(PinClickedCallbackProperty, value); }
        }

        /// <summary>
        /// Pin's clicked callback source enumerator.
        /// </summary>
        public enum PinClickedCallbackSourceEnum
        {
            Map,
            Pins
        }

        /// <summary>
        /// Pin's clicked callback source property.
        /// </summary>
        public static readonly BindableProperty PinClickedCallbackSourceProperty =
            BindableProperty.Create(nameof(PinClickedCallbackSource), typeof(PinClickedCallbackSourceEnum), typeof(CustomMap), PinClickedCallbackSourceEnum.Map);

        /// <summary>
        /// Assessor for PinClickedCallbackSource property.
        /// </summary>
        public PinClickedCallbackSourceEnum PinClickedCallbackSource
        {
            get { return (PinClickedCallbackSourceEnum)GetValue(PinClickedCallbackSourceProperty); }
            set { SetValue(PinClickedCallbackSourceProperty, value); }
        }

        #region Constructor
        /// <summary>
        /// Contructor
        /// </summary>
        public CustomMap()
        {
            isMapLoaded = false;
            this.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                CustomMap map = sender as CustomMap;
                if (map.VisibleRegion != null)
                {
                    this.ZoomLevel = map.VisibleRegion.Radius;
                }
            };
        }
        #endregion

        #region Additionnal static methods for Custom Renderer (avoid duplicated code)
        /// <summary>
        /// This method has been made to avoid the duplication of code for each renderer.
        /// It allows us, based on some internal/given parameters, to add the pins by "normal adding" function or a "special adding" one.
        /// </summary>
        /// <param name="customMap">The instance of the Custom Map object.</param>
        /// <param name="addPins">The normal function to add a pin.</param>
        /// <param name="addPins_AboutPinLimitOfZoom">The special function to add a pin.</param>
        public static void AddPinsToRendererMap(CustomMap customMap, Action addPins, Action<double> addPins_AboutPinLimitOfZoom)
        {
            if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.None)
                addPins();
            else
            {
                if (customMap.PinZoomVisibilityLimitUnity == CustomMap.PinZoomVisibilityLimitUnityEnum.Kilometers)
                {
                    if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Map
                        && customMap.ZoomLevel.Kilometers >= customMap.PinZoomVisibilityMinimumLimit && customMap.ZoomLevel.Kilometers <= customMap.PinZoomVisibilityMaximumLimit)
                        addPins();
                    else if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Pin)
                        addPins_AboutPinLimitOfZoom(customMap.ZoomLevel.Kilometers);
                }
                else if (customMap.PinZoomVisibilityLimitUnity == CustomMap.PinZoomVisibilityLimitUnityEnum.Meters)
                {
                    if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Map
                        && customMap.ZoomLevel.Kilometers >= customMap.PinZoomVisibilityMinimumLimit && customMap.ZoomLevel.Meters <= customMap.PinZoomVisibilityMaximumLimit)
                        addPins();
                    else if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Pin)
                        addPins_AboutPinLimitOfZoom(customMap.ZoomLevel.Meters);
                }
                else if (customMap.PinZoomVisibilityLimitUnity == CustomMap.PinZoomVisibilityLimitUnityEnum.Miles)
                {
                    if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Map
                        && customMap.ZoomLevel.Kilometers >= customMap.PinZoomVisibilityMinimumLimit && customMap.ZoomLevel.Miles <= customMap.PinZoomVisibilityMaximumLimit)
                        addPins();
                    else if (customMap.PinZoomVisibilityLimitSource == CustomMap.PinZoomVisibilityLimitSourceEnum.Pin)
                        addPins_AboutPinLimitOfZoom(customMap.ZoomLevel.Miles);
                }
            }
        }
        /// <summary>
        /// Boolean which defines if the map is full load or not.
        /// </summary>
        private bool isMapLoaded;
        /// <summary>
        /// Set the isMapLoaded boolean to true and set some values. This function is called from the renderer when the map is full loaded.
        /// </summary>
        public void MapLoaded()
        {
            isMapLoaded = true;
            if (this.VisibleRegion != null)
            {
                this.ZoomLevel = this.VisibleRegion.Radius;
            }
            UpdateCamera();
        }
        /// <summary>
        /// Update the camera focus based on the CameraFocus property.
        /// </summary>
        public void UpdateCamera()
        {
            if (this.isMapLoaded)
            {
                if (this.CameraFocusParameter == CameraFocusReference.OnPins && CustomPins != null)
                {
                    List<double> latitudes = new List<double>();
                    List<double> longitudes = new List<double>();

                    foreach (CustomPin pin in CustomPins)
                    {
                        if (pin.Location.Latitude != Double.MaxValue)
                        {
                            latitudes.Add(pin.Location.Latitude);
                            longitudes.Add(pin.Location.Longitude);
                        }
                    }

                    double lowestLat = latitudes.Min();
                    double highestLat = latitudes.Max();
                    double lowestLong = longitudes.Min();
                    double highestLong = longitudes.Max();
                    double finalLat = (lowestLat + highestLat) / 2;
                    double finalLong = (lowestLong + highestLong) / 2;

                    double distance = DistanceCalculation.GeoCodeCalc.CalcDistance(lowestLat, lowestLong, highestLat, highestLong, DistanceCalculation.GeoCodeCalcMeasurement.Kilometers);

                    //If the value is too high, then on UWP it throws an uncatchable exception...
                    if ((finalLat < Double.MaxValue && finalLat > Double.MinValue)
                        && (finalLong < Double.MaxValue && finalLong > Double.MinValue)
                        && (distance < Double.MaxValue && distance > Double.MinValue))
                    {
                        try
                        {
                            this.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(finalLat, finalLong), Distance.FromKilometers(distance * 0.7)));
                            return;
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
        #endregion

        #region Additionnal static methods for Google API functionnalities
        /// <summary>
        /// Get the address of the Lat/Long position by using Google API.
        /// </summary>
        /// <param name="position">Lat/Long reference.</param>
        /// <returns>The address of the Lat/Long position parameter.</returns>
        public static async Task<string> GetAddressName(Position position)
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
        /// <summary>
        /// Get the Lat/Long of the address string by using Google API.
        /// </summary>
        /// <param name="name">Address reference.</param>
        /// <returns>The Lat/Long of the address string parameter.</returns>
        public static async Task<Position> GetAddressPosition(string name)
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
                position = new Position(Double.MaxValue, Double.MaxValue);
            }
            return (position);
        }
        /// <summary>
        /// Making an http request to the Google API.
        /// </summary>
        /// <param name="url">The final url for the request.</param>
        /// <param name="additionnal_URL">The parameter of the "url parameter" for the request.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Class to store data of the DistanceCalculation class's functions
        /// </summary>
        public class CameraFocusData
        {
            public Position Position { get; set; }
            public Distance Distance { get; set; }
        }
        /// <summary>
        /// Method called when the CustomPins collection property is updated.
        /// </summary>
        /// <param name="bindable">The CustomMap object which contains the custom pins collection.</param>
        /// <param name="oldValue">The previous value or the custom pins collection.</param>
        /// <param name="newValue">The new value or the custom pins collection.</param>
        private static void OnCustomPinsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //Do whatever you want
            //(bindable as CustomMap).UpdateCamera();
        }
        /// <summary>
        /// Class to calcul the distance of zoom for the camera to focus all of the lat/lng points given on the map.
        /// </summary>
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
