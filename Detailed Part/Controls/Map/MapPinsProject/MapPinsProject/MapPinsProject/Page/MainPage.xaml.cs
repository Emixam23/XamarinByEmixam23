using MapPinsProject.Models;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.Page
{
    public partial class MainPage : ContentPage
    {
        public List<CustomPin> CustomPins { get; set; }

        public MainPage()
        {
            base.BindingContext = this;

            CustomPins = new List<CustomPin>()
            {
                new CustomPin() { Name = "Le Mans", Details = "Famous city for race driver !", Position = new Position(48,0.2) },
                new CustomPin() { Name = "Ruaudin", Details = "Where I'm coming from.", Position = new Position(47.9450,0.26) }
            };

            InitializeComponent();
        }
    }
}
