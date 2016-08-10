using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPolylineProject.CustomControl
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty PolylineAddressPointsProperty =
            BindableProperty.Create(nameof(PolylineAddressPoints), typeof(List<string>), typeof(CustomMap), new List<string>(), BindingMode.TwoWay);
        public List<string> PolylineAddressPoints
        {
            get { return (List<string>)GetValue(PolylineAddressPointsProperty); }
            set
            {
                SetValue(PolylineAddressPointsProperty, value);

                if (PolylineAutoUpdate)
                {
                    generatePolylineCoordinates(value);
                }
                else
                {
                    Debug.WriteLine("PolylineAutoUpdate is false");
                }
            }
        }

        public static readonly BindableProperty PolylineCoordinatesProperty =
            BindableProperty.Create(nameof(PolylineCoordinates), typeof(List<GeoPosition>), typeof(CustomMap), new List<GeoPosition>(), BindingMode.TwoWay);
        public List<GeoPosition> PolylineCoordinates
        {
            get { return (List<GeoPosition>)GetValue(PolylineCoordinatesProperty); }
            set { SetValue(PolylineCoordinatesProperty, value); }
        }

        public static readonly BindableProperty PolylineColorProperty =
            BindableProperty.Create(nameof(PolylineColor), typeof(Color), typeof(CustomMap), Color.Red, BindingMode.TwoWay);
        public Color PolylineColor
        {
            get { return (Color)GetValue(PolylineColorProperty); }
            set { SetValue(PolylineColorProperty, value); }
        }

        public static readonly BindableProperty PolylineWidthProperty =
            BindableProperty.Create(nameof(PolylineWidth), typeof(int), typeof(CustomMap), 5, BindingMode.TwoWay);
        public int PolylineWidth
        {
            get { return (int)GetValue(PolylineWidthProperty); }
            set { SetValue(PolylineWidthProperty, value); }
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
        public bool PolylineAutoUpdate { get; set; }
        #endregion

        #region Generation methods
        private async void generatePolylineCoordinates(List<string> value)
        {
            this.PolylineCoordinates = await GeneratePolylineCoordinates(value);
        }
        public static async Task<List<GeoPosition>> GeneratePolylineCoordinates(List<string> AddressPoints)
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

                    while (!finished)
                    {
                        try
                        {
                            GeoPositionList.AddRange(Decode((item["routes"][0]["legs"][0]["steps"][index]["polyline"]["points"]).ToString()));
                            index++;
                        }
                        catch (Exception)
                        {
                            finished = true;
                        }
                    }
                }
                return (GeoPositionList);
            }
            else
            {
                return (new List<GeoPosition>());
            }
        }
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
                HttpResponseMessage response = await client.PostAsync(aditionnal_URL, content);

                // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
                string result = await response.Content.ReadAsStringAsync();
                if (result != null)
                {
                    try
                    {
                        return JObject.Parse(result);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                        return null;
                    }
                }
                else
                {
                    Debug.WriteLine("result == null");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
        /// <summary>
        /// Decode google style polyline coordinates.
        /// </summary>
        /// <param name="encodedPoints"></param>
        /// <returns></returns>
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
    }
}
