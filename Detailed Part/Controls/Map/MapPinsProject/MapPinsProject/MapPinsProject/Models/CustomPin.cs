using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.Models
{
    public class CustomPin
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public string ImageSource { get; set; }
        public Position Position { get; set; }
        public uint PinSize { get; set; }
        public int PinZoomVisibilityLimit { get; set; }
        public Point AnchorPoint { get; set; }

        public CustomPin()
        {
            Name = "";
            Details = "";
            ImageSource = "";
            Position = new Position(48, 0.2);
            PinSize = 50;
            PinZoomVisibilityLimit = 0;
            AnchorPoint = new Point(0.5, 1);
        }
    }
}
