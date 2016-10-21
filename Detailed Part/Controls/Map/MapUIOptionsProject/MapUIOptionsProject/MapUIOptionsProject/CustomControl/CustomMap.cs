using Plugin.Geolocator;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapUIOptionsProject.CustomControl
{
    /// <summary>
    /// CustomMap inherits from Map, which give us the same behavior of a basic Xamarin.Forms.Map with the possibility of add some functionalities.
    /// </summary>
    public class CustomMap : Map
    {
        /// <summary>
        /// Boolean to enable/disable the UI options of the Map.
        /// </summary>
        public static readonly BindableProperty IsUIOptionsEnableProperty =
            BindableProperty.Create(nameof(IsUIOptionsEnable), typeof(bool), typeof(CustomMap), true);

        /// <summary>
        /// Assessor for IsUIOptionsEnable property.
        /// </summary>
        public bool IsUIOptionsEnable
        {
            get { return (bool)GetValue(IsUIOptionsEnableProperty); }
            set { SetValue(IsUIOptionsEnableProperty, value); }
        }

        /// <summary>
        /// The current location of the user.
        /// </summary>
        public static readonly BindableProperty UserLocationProperty =
            BindableProperty.Create(nameof(UserLocation), typeof(Position), typeof(CustomMap), new Position());

        /// <summary>
        /// Assessor for UserLocation property.
        /// </summary>
        public Position UserLocation
        {
            get { return (Position)GetValue(UserLocationProperty); }
            private set { SetValue(UserLocationProperty, value); }
        }

        /// <summary>
        /// Get the current position of the user.
        /// </summary>
        /// <returns>The current location of the user.</returns>
        public async static Task<Position> GetPosition()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var LocatorPosition = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                Position position = new Position(LocatorPosition.Latitude, LocatorPosition.Longitude);

                getCustomMapInstance().UserLocation = position;
                return (position);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }

            return (new Position());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CustomMap()
        {
            CustomMap.getCustomMapInstance(this);
        }

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
