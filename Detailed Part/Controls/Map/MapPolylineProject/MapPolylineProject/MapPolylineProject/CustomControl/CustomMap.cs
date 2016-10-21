using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPolylineProject.CustomControl
{
    /// <summary>
    /// CustomMap inherits from Map, which give us the same behavior of a basic Xamarin.Forms.Map with the possibility of add some functionalities.
    /// </summary>
    public class CustomMap : Map
    {
        /// <summary>
        /// This IList<string> is containing all of the address given by the user to create the polyline.
        /// </summary>
        public static readonly BindableProperty PolylineAddressPointsProperty =
            BindableProperty.Create(nameof(PolylineAddressPoints), typeof(IList<string>), typeof(CustomMap), null,
                propertyChanged: OnPolyLineAddressPointsPropertyChanged);

        /// <summary>
        /// Assessor for PolylineAddressPoints property.
        /// </summary>
        public IList<string> PolylineAddressPoints
        {
            get { return (IList<string>)GetValue(PolylineAddressPointsProperty); }
            set { SetValue(PolylineAddressPointsProperty, value); }
        }

        /// <summary>
        /// This List<GeoPosition> is containing all of the Position of the polyline.
        /// </summary>
        public static readonly BindableProperty PolylineCoordinatesProperty =
            BindableProperty.Create(nameof(PolylineCoordinates), typeof(List<GeoPosition>), typeof(CustomMap), null);

        /// <summary>
        /// Assessor for PolylineCoordinates property.
        /// </summary>
        public List<GeoPosition> PolylineCoordinates
        {
            get { return (List<GeoPosition>)GetValue(PolylineCoordinatesProperty); }
            set { SetValue(PolylineCoordinatesProperty, value); }
        }

        /// <summary>
        /// Color of the Polyline.
        /// </summary>
        public static readonly BindableProperty PolylineColorProperty =
            BindableProperty.Create(nameof(PolylineColor), typeof(Color), typeof(CustomMap), Color.Red);

        /// <summary>
        /// Assessor for PolylineColor property.
        /// </summary>
        public Color PolylineColor
        {
            get { return (Color)GetValue(PolylineColorProperty); }
            set { SetValue(PolylineColorProperty, value); }
        }

        /// <summary>
        /// Width of the Polyline.
        /// </summary>
        public static readonly BindableProperty PolylineThicknessProperty =
            BindableProperty.Create(nameof(PolylineThickness), typeof(double), typeof(CustomMap), 5.0);

        /// <summary>
        /// Assessor for PolylineThickness property.
        /// </summary>
        public double PolylineThickness
        {
            get { return (double)GetValue(PolylineThicknessProperty); }
            set { SetValue(PolylineThicknessProperty, value); }
        }

        /// <summary>
        /// Point of interest possibilities.
        /// </summary>
        public enum CameraFocusReference
        {
            None,
            OnPolyline
        }

        /// <summary>
        /// Point of interest for the Camera.
        /// </summary>
        public static readonly BindableProperty CameraFocusParameterProperty =
            BindableProperty.Create(nameof(CameraFocusParameter), typeof(CameraFocusReference), typeof(CustomMap), CameraFocusReference.None);

        /// <summary>
        /// Assessor for CameraFocusParameter property.
        /// </summary>
        public CameraFocusReference CameraFocusParameter
        {
            get { return (CameraFocusReference)GetValue(CameraFocusParameterProperty); }
            set { SetValue(CameraFocusParameterProperty, value); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CustomMap()
        {
            PolylineSteps = new List<Position>();
            getCustomMapInstance(this);
        }

        #region Additional resources
        public class GeoPosition
        {
            public GeoPosition()
            {
                this.Latitude = 0.0;
                this.Longitude = 0.0;
            }

            public GeoPosition(double latitude, double longitude)
            {
                this.Latitude = latitude;
                this.Longitude = longitude;
            }

            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        #endregion

        #region OnPolyLineAddressPointsPropertyChanged
        /// <summary>
        /// Method call each time the PolyLineAddressPointsProperty is set.
        /// This method starts the generation of the polyline.
        /// </summary>
        /// <param name="bindable">CustomMap instance.</param>
        /// <param name="oldValue">Previous value of PolyLineAddressPointsProperty.</param>
        /// <param name="newValue">New value of PolyLineAddressPointsProperty.</param>        
        public static void OnPolyLineAddressPointsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomMap)bindable).OnPolyLineAddressPointsPropertyChanged((IList<string>)oldValue, (IList<string>)newValue);
        }
        /// <summary>
        /// Call GeneratePolylineCoordinatesInner() which generates the polyline by the CustomMap itself.
        /// </summary>
        /// <param name="oldValue">Previous value of PolyLineAddressPointsProperty.</param>
        /// <param name="newValue">New value of PolyLineAddressPointsProperty.</param>   
        public void OnPolyLineAddressPointsPropertyChanged(IList<string> oldValue, IList<string> newValue)
        {
            GeneratePolylineCoordinatesInner();
        }
        #endregion

        #region Generation methods
        /// <summary>
        /// Event for updates of map's properties.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
       
        /// <summary>
        /// Translate the PolylineAddressPoints list to GeoPosition usable to create the polyline. This method, then, fire the changement of the property by PropertyChangedEventArgs.
        /// This method also call CoverFieldOfFocus() which move the camera to the POI of CameraFocusParameterProperty.
        /// </summary>
        public async void GeneratePolylineCoordinatesInner()
        {
            if (this.PolylineAddressPoints != null)
            {
                this.PolylineCoordinates = await GeneratePolylineCoordinates(this.PolylineAddressPoints);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs(nameof(PolylineCoordinatesProperty)));
                }
                CoverFieldOfFocus();
            }
        }
        /// <summary>
        /// This method get every information from the Google API and then, by the help of other method, decode every polylines for each step, get the major step of the polyline.
        /// At the end, this function return a List of GeoPositions corresponding to the polyline of the address mail's list.
        /// This method is also the parser for the data of the Google API.
        /// </summary>
        /// <param name="AddressPoints">Take a List<string> of mail address.</param>
        /// <returns>List of GeoPositions corresponding to the polyline of the address mail's list.</returns>
        public static async Task<List<GeoPosition>> GeneratePolylineCoordinates(IList<string> AddressPoints)
        {
            if (AddressPoints.Count > 1)
            {
                List<GeoPosition> GeoPositionList = new List<GeoPosition>();
                List<JObject> DirectionResponseList = new List<JObject>();
                string addr_tmp = null;

                foreach (string item in AddressPoints)
                {
                    if (addr_tmp == null)
                    {
                        addr_tmp = item;
                    }
                    else
                    {
                        JObject response = await GetDirectionFromGoogleAPI(addr_tmp, item);
                        if (response != null)
                        {
                            DirectionResponseList.Add(response);
                        }
                        addr_tmp = item;
                    }
                }
                foreach (JObject item in DirectionResponseList)
                {
                    bool finished = false;
                    int index = 0;

                    try
                    {
                        CustomMap.getCustomMapInstance().PolylineSteps.Add(new Position(
                                Double.Parse((item["routes"][0]["legs"][0]["steps"][index]["start_location"]["lat"]).ToString()),
                                Double.Parse((item["routes"][0]["legs"][0]["steps"][index]["start_location"]["lng"]).ToString())));
                    }
                    catch (Exception)
                    {
                        finished = true;
                    }

                    while (!finished)
                    {
                        try
                        {
                            CustomMap.getCustomMapInstance().PolylineSteps.Add(new Position(
                                Double.Parse((item["routes"][0]["legs"][0]["steps"][index]["end_location"]["lat"]).ToString()),
                                Double.Parse((item["routes"][0]["legs"][0]["steps"][index]["end_location"]["lng"]).ToString())));

                            GeoPositionList.AddRange(Decode((item["routes"][0]["legs"][0]["steps"][index]["polyline"]["points"]).ToString()));
                            index++;
                        }
                        catch (Exception)
                        {
                            finished = true;
                        }
                    }
                }
                int a = 0;
                foreach (GeoPosition gp in GeoPositionList)
                {
                    a++;
                }
                return (GeoPositionList);
            }
            else
            {
                return (new List<GeoPosition>());
            }
        }
        /// <summary>
        /// Make a request to the Google API to get all of the information between the two mail address points given as parameters.
        /// </summary>
        /// <param name="origin">Origin mail address.</param>
        /// <param name="destination">Destination mail address.</param>
        /// <returns>The response of the request for the polyline/steps between the two mail address paramter.</returns>
        public static async Task<JObject> GetDirectionFromGoogleAPI(string origin, string destination)
        {
            string url = "https://maps.googleapis.com/maps/api/directions/json";
            string aditionnal_URL = "?origin=" + origin
            + "&destination=" + destination
            + "&key=" + App.GOOGLE_MAP_API_KEY;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(url);

                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(aditionnal_URL, content);
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
        /// <summary>
        /// Decode google style polyline coordinates.
        /// </summary>
        /// <param name="encodedPoints">The encoded polyline string.</param>
        /// <returns>The convertion of the encoded string into a list a usable GeoPositions.</returns>
        public static List<GeoPosition> Decode(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                throw new ArgumentNullException("encodedPoints");

            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            List<GeoPosition> polylinesPosition = new List<GeoPosition>();

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                polylinesPosition.Add(new GeoPosition(Convert.ToDouble(currentLat) / 1E5, Convert.ToDouble(currentLng) / 1E5));
            }

            return (polylinesPosition);
        }
        #endregion

        #region Camera focus definition
        /// <summary>
        /// Class data for store information about the focus of the Camera.
        /// </summary>
        public class CameraFocusData
        {
            public Position Position { get; set; }
            public Distance Distance { get; set; }
        }
        /// <summary>
        /// List of steps which will be use as points of interest to place the Camera.
        /// </summary>
        public List<Position> PolylineSteps;
        /// <summary>
        /// Prepare data for calcul and use DistanceCalculation to make the calcul of the final data.
        /// </summary>
        private void CoverFieldOfFocus()
        {
            if (this.CameraFocusParameter == CameraFocusReference.OnPolyline)
            {
                List<double> latitudes = new List<double>();
                List<double> longitudes = new List<double>();

                foreach (Position step in this.PolylineSteps)
                {
                    latitudes.Add(step.Latitude);
                    longitudes.Add(step.Longitude);
                }

                double lowestLat = latitudes.Min();
                double highestLat = latitudes.Max();
                double lowestLong = longitudes.Min();
                double highestLong = longitudes.Max();
                double finalLat = (lowestLat + highestLat) / 2;
                double finalLong = (lowestLong + highestLong) / 2;

                double distance = DistanceCalculation.GeoCodeCalc.CalcDistance(lowestLat, lowestLong, highestLat, highestLong, DistanceCalculation.GeoCodeCalcMeasurement.Kilometers);

                this.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(finalLat, finalLong), Distance.FromKilometers(distance * 0.7)));
            }
        }
        /// <summary>
        /// Class which makes the calcul of the data to place the camera over the points of interests.
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

        #region Single Town part
        /// <summary>
        /// Instance of the current object for static uses.
        /// </summary>
        private static CustomMap customMapInstance;
        /// <summary>
        /// Get the current object instance or create one if the customMapInstance is null.
        /// </summary>
        /// <param name="instance">Current instance of the object for initialization.</param>
        /// <returns>The instance of the current object.</returns>
        public static CustomMap getCustomMapInstance(CustomMap instance = null)
        {
            if (customMapInstance == null)
            {
                customMapInstance = instance;
            }
            return (customMapInstance);
        }
        #endregion
    }
}