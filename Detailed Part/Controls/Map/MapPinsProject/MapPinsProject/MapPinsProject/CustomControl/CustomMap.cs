using MapPinsProject.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.CustomControl
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty CustomPinsProperty =
            BindableProperty.Create(nameof(CustomPins), typeof(List<CustomPin>), typeof(CustomMap), null);
        public List<CustomPin> CustomPins
        {
            get { return (List<CustomPin>)GetValue(CustomPinsProperty); }
            set { SetValue(CustomPinsProperty, value); }
        }
    }
}
