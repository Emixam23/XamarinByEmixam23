using MapPinsProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.Page
{
    public partial class MainPage : ContentPage
    {
        public List<CustomPin> CustomPins { get; set; }
        public Action<CustomPin> PinActionClicked { get; set; }
        public uint TMP { get; set; }

        public MainPage()
        {
            base.BindingContext = this;

            CustomPins = new List<CustomPin>()
            {
                new CustomPin() { Name = "Le Mans", Details = "Famous city for race driver !", ImageSource = "CustomIconImage.png", Position = new Position(48,0.2), PinZoomVisibilityLimit = 150, PinSize = 75},
                new CustomPin() { Name = "Ruaudin", Details = "Where I'm coming from.", ImageSource = "CustomIconImage.png", Position = new Position(47.9450,0.26), PinZoomVisibilityLimit = 75, PinSize = 65 },
                new CustomPin() { Name = "Chelles", Details = "Someone there.", ImageSource = "CustomIconImage.png", Position = new Position(48.877535,2.590160), PinZoomVisibilityLimit = 50, PinSize = 70 },
                new CustomPin() { Name = "Lille", Details = "Le nord..", ImageSource = "CustomIconImage.png", Position = new Position(50.629250, 3.057256), PinZoomVisibilityLimit = 44, PinSize = 40 },
                new CustomPin() { Name = "Limoges", Details = "I have been there ! :o", ImageSource = "CustomIconImage.png", Position = new Position(45.833619, 1.261105), PinZoomVisibilityLimit = 65, PinSize = 20 },
                new CustomPin() { Name = "Douarnenez", Details = "A trip..", ImageSource = "CustomIconImage.png", Position = new Position(48.093228,-4.328619), PinZoomVisibilityLimit = 110, PinSize = 50 }
            };

            PinActionClicked = PinClickedCallback;

            TMP = Convert.ToUInt32(23);

            InitializeComponent();
        }

        private void PinClickedCallback(CustomPin customPinClicked)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine("{0}, {1}", customPinClicked.PinZoomVisibilityLimit, customPinClicked.Name);
            });
        }
    }
}
