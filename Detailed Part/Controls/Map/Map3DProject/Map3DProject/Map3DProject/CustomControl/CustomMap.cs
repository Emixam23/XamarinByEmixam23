using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Map3DProject.CustomControl
{
    public class CustomMap : Map
    {
        /// <summary>
        /// This IList<string> is containing all of the address given by the user to create the polyline.
        /// </summary>
        public static readonly BindableProperty LocationProperty =
            BindableProperty.Create(nameof(Location), typeof(Position), typeof(CustomMap), new Position(47.942660, 0.261979));

        /// <summary>
        /// Assessor for PolylineAddressPoints property.
        /// </summary>
        public Position Location
        {
            get { return (Position)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        public static readonly BindableProperty ZoomLevelProperty =
            BindableProperty.Create(nameof(ZoomLevel), typeof(Distance), typeof(CustomMap), new Distance());
        public Distance ZoomLevel
        {
            get { return (Distance)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        #region Constructor
        public CustomMap()
        {
            this.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                CustomMap map = sender as CustomMap;
                if (map.VisibleRegion != null)
                {
                    this.ZoomLevel = map.VisibleRegion.Radius;
                }
            };
            isMapLoaded = false;
        }
        #endregion

        #region Additionnals
        private bool isMapLoaded;
        public void MapLoaded()
        {
            isMapLoaded = true;
            if (this.VisibleRegion != null)
            {
                this.ZoomLevel = this.VisibleRegion.Radius;
            }
        }
        public bool IsMapLoaded { get { return (isMapLoaded); } }
        #endregion
    }
}
