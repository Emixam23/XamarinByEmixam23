using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPolylineProject.CustomControl
{
    public class CustomMap : Map, INotifyPropertyChanged
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
            set { SetValue(PolylineCoordinatesProperty, value); }
        }

        public static readonly BindableProperty PolylineAutoUpdateProperty =
            BindableProperty.Create(nameof(PolylineAutoUpdate), typeof(bool), typeof(CustomMap), true);
        public bool PolylineAutoUpdate
        {
            get { return (bool)GetValue(PolylineAutoUpdateProperty); }
            set { SetValue(PolylineAutoUpdateProperty, value); }
        }

        public static readonly BindableProperty PolylineColorProperty =
            BindableProperty.Create(nameof(PolylineColor), typeof(Color), typeof(CustomMap), Color.Red);
        public Color PolylineColor
        {
            get { return (Color)GetValue(PolylineColorProperty); }
            set { SetValue(PolylineColorProperty, value); }
        }

        public static readonly BindableProperty PolylineThicknessProperty =
            BindableProperty.Create(nameof(PolylineThickness), typeof(double), typeof(CustomMap), 5.0);
        public double PolylineThickness
        {
            get { return (double)GetValue(PolylineThicknessProperty); }
            set { SetValue(PolylineThicknessProperty, value); }
        }

        /// <summary>
        /// WORK IN PROGRESS
        /// </summary>
        /*public enum CameraFocusReference
        {
            None,
            OnCurrentPosition,
            OnPolyline
        }
        public static readonly BindableProperty CameraFocusProperty =
            BindableProperty.Create(nameof(CameraFocus), typeof(CameraFocusReference), typeof(CustomMap), CameraFocusReference.None);
        public CameraFocusReference CameraFocus
        {
            get { return (CameraFocusReference)GetValue(CameraFocusProperty); }
            set { SetValue(CameraFocusProperty, value); }
        }*/

        public static readonly BindableProperty IsUIButtonVisibleProperty =
            BindableProperty.Create(nameof(IsUIButtonVisible), typeof(bool), typeof(CustomMap), true);
        public bool IsUIButtonVisible
        {
            get { return (bool)GetValue(IsUIButtonVisibleProperty); }
            set { SetValue(IsUIButtonVisibleProperty, value); }
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

        #region Generation methods
        public event PropertyChangedEventHandler PropertyChanged;

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
            }
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
                catch (NullReferenceException e)
                {
                    Debug.WriteLine("NullReferenceException is thrown ! " + e.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something is thrown ! " + e.ToString());
                }
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
