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
                new CustomPin() { Name = "Le Mans", Details = "Famous city for race driver !", ImageSource = "CustomIconImage.png", Position = new Position(48,0.2), AnchorPoint = new Point(0.5, 1) },
                new CustomPin() { Name = "Ruaudin", Details = "Where I'm coming from.", ImageSource = "CustomIconImage.png", Position = new Position(47.9450,0.26) },
                new CustomPin() { Name = "Chelles", Details = "Someone there.", ImageSource = "CustomIconImage.png", Position = new Position(48.877535,2.590160) },
                new CustomPin() { Name = "Lille", Details = "Le nord..", ImageSource = "CustomIconImage.png", Position = new Position(50.629250, 3.057256) },
                new CustomPin() { Name = "Limoges", Details = "I have been there ! :o", ImageSource = "CustomIconImage.png", Position = new Position(45.833619, 1.261105) },
                new CustomPin() { Name = "Douarnenez", Details = "A trip..", ImageSource = "CustomIconImage.png", Position = new Position(48.093228,-4.328619) }
            };

            PinActionClicked = PinClickedCallback;

            TMP = Convert.ToUInt32(23);

            InitializeComponent();
        }

        private void PinClickedCallback(CustomPin customPinClicked)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine("{0}, {1}", customPinClicked.Name, customPinClicked.Details);
            });
        }
    }
}
