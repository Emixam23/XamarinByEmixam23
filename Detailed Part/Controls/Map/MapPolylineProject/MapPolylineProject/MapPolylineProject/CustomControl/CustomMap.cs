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
            BindableProperty.Create(nameof(PolylineAddressPoints), typeof(List<string>), typeof(CustomMap), null);
        public List<string> PolylineAddressPoints
        {
            get { return (List<string>)GetValue(PolylineAddressPointsProperty); }
            set
            {
                SetValue(PolylineAddressPointsProperty, value);
                this.GeneratePolylineCoordinatesInner();
            }
        }

        public static readonly BindableProperty PolylineCoordinatesProperty =
            BindableProperty.Create(nameof(PolylineCoordinates), typeof(List<GeoPosition>), typeof(CustomMap), null);
        public List<GeoPosition> PolylineCoordinates
        {
            get { return (List<GeoPosition>)GetValue(PolylineCoordinatesProperty); }
            set
            {
                Debug.WriteLine("edit");
                SetValue(PolylineCoordinatesProperty, value);
            }
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
        public async void GeneratePolylineCoordinatesInner()
        {
            if (this.PolylineAddressPoints != null)
            {
                Debug.WriteLine("Debug is working also here!");
                this.PolylineCoordinates = await GeneratePolylineCoordinates(this.PolylineAddressPoints);
            }
        }
        public static async Task<List<GeoPosition>> GeneratePolylineCoordinates(List<string> AddressPoints)
        {
            Debug.WriteLine("GeneratePolylineCoordinates => 1");
            if (AddressPoints.Count > 1)
            {
                Debug.WriteLine("GeneratePolylineCoordinates => 2");
                List<GeoPosition> GeoPositionList = new List<GeoPosition>();
                List<JObject> DirectionResponseList = new List<JObject>();
                string addr_tmp = null;

                foreach (string item in AddressPoints)
                {
                    if (addr_tmp == null)
                    {
                        addr_tmp = item;
                        Debug.WriteLine("GeneratePolylineCoordinates => addr_tmp = item => " + item);
                    }
                    else
                    {
                        Debug.WriteLine("GeneratePolylineCoordinates => else => [{0}/{1}]", addr_tmp, item);
                        JObject response = await GetDirectionFromGoogleAPI(addr_tmp, item);
                        Debug.WriteLine("Response got !");
                        if (response != null)
                        {
                            Debug.WriteLine("response != null");
                            DirectionResponseList.Add(response);
                        }
                        addr_tmp = item;
                    }
                }
                foreach (JObject item in DirectionResponseList)
                {
                    bool finished = false;
                    int index = 0;

                    Debug.WriteLine("GeneratePolylineCoordinates => 2");
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
                int a = 0;
                foreach (GeoPosition gp in GeoPositionList)
                {
                    Debug.WriteLine("[{0}]GeoPosition = {1}/{2}", a, gp.Latitude, gp.Longitude);
                    a++;
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

            Debug.WriteLine("Test 1");
            try
            {
                var client = new HttpClient();
                Debug.WriteLine("Test 2");
                client.BaseAddress = new Uri(url);
                Debug.WriteLine("Test 3");

                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                Debug.WriteLine("Test 4");
                HttpResponseMessage response = null;
                try
                {
                    Debug.WriteLine("Test 4.1");
                    response = await client.PostAsync(aditionnal_URL, content);
                    Debug.WriteLine("Test 4.2");
                }
                catch (NullReferenceException e)
                {
                    Debug.WriteLine("NullReferenceException is thrown ! " + e.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something is thrown ! " + e.ToString());
                }
                Debug.WriteLine("Test 5");
                string result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Test 6");
                if (result != null)
                {
                    try
                    {
                        Debug.WriteLine("Test 7");
                        return JObject.Parse(result);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Test 7.1");
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
                Debug.WriteLine("Test 8");
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
