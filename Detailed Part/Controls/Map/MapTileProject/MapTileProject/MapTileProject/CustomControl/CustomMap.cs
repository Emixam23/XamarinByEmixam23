using Xamarin.Forms.Maps;

namespace MapTileProject.CustomControl
{
    /// <summary>
    /// CustomMap inherits from Map, which give us the same behavior of a basic Xamarin.Forms.Map with the possibility of add some functionalities.
    /// </summary>
    public class CustomMap : Map
    {
        /// <summary>
        /// This string contains the URL of your design template for the tiles of the CustomMap
        /// </summary>
        public string MapTileTemplate { get; set; }
    }
}
