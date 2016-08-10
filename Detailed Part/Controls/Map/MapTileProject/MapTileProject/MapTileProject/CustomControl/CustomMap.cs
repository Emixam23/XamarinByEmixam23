using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapTileProject.CustomControl
{
    /// <summary>
    /// CustomMap inherits from Map, which give us the same behavior of a basic Xamarin.Forms.Map with the possibility of add some functionalities.
    /// </summary>
    public class CustomMap : Map
    {
        /// <summary>
        /// This string contains the URL of your design template for the tiles of the CustomMap.
        /// </summary>
        public static readonly BindableProperty MapTileTemplateProperty = 
            BindableProperty.Create(nameof(MapTileTemplate), typeof(string), typeof(CustomMap), null);

        /// <summary>
        /// Assessor for MapTileTemplate property.
        /// </summary>
        public string MapTileTemplate
        {
            get { return (string)GetValue(MapTileTemplateProperty); }
            set { SetValue(MapTileTemplateProperty, value); }
        }
    }
}
