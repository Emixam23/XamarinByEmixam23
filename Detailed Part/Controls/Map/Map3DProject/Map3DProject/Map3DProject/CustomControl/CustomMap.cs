using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Map3DProject.CustomControl
{
    public class CustomMap : Map
    {
        /// <summary>
        /// Just for my test since I can't find the way to find the current location of the camera
        /// </summary>
        public static readonly BindableProperty LocationProperty =
            BindableProperty.Create(nameof(Location), typeof(Position), typeof(CustomMap), new Position(47.942660, 0.261979));

        /// <summary>
        /// Assessor for Location property.
        /// </summary>
        public Position Location
        {
            get { return (Position)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        /// <summary>
        /// Just for my test since I can't find the way to get the current zoom level
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
        /// <summary>
        /// Method called from the renderer once the map is loaded
        /// </summary> 
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
