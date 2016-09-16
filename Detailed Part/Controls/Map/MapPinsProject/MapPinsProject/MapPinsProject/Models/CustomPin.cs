using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.Models
{
    public class CustomPin
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public ImageSource ImageSource { get; set; }
        public Position Position { get; set; }
    }
}
