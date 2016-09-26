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
                new CustomPin(ImageSource.FromResource("MapPinsProject.Image.CustomIconImage.png")) { Name = "Le Mans", Details = "Famous city for race driver !", Position = new Position(48,0.2) },
                new CustomPin(ImageSource.FromResource("MapPinsProject.Image.CustomIconImage.png")) { Name = "Ruaudin", Details = "Where I'm coming from.", Position = new Position(47.9450,0.26) },
                new CustomPin(ImageSource.FromResource("MapPinsProject.Image.CustomIconImage.png")) { Name = "Chelles", Details = "Someone there.", Position = new Position(48.877535,2.590160) },
                new CustomPin(ImageSource.FromResource("MapPinsProject.Image.CustomIconImage.png")) { Name = "Lille", Details = "Le nord..", Position = new Position(50.629250, 3.057256) },
                new CustomPin(ImageSource.FromResource("MapPinsProject.Image.CustomIconImage.png")) { Name = "Limoges", Details = "I was there ! :o", Position = new Position(45.833619, 1.261105) },
                new CustomPin(ImageSource.FromResource("MapPinsProject.Image.CustomIconImage.png")) { Name = "Douarnenez", Details = "A trip..", Position = new Position(48.093228,-4.328619) }
            };

            InitializeComponent();
        }
    }
}
