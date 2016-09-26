using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.Models
{
    public class CustomPin
    {
        public CustomPin(ImageSource imageSource)
        {
            (this.Image = new Image()).Source = imageSource;
        }

        public string Name { get; set; }
        public string Details { get; set; }
        public Image Image { get; set; }
        public Position Position { get; set; }
    }
}
