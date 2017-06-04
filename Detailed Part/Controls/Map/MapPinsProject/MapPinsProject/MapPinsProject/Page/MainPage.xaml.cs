using MapPinsProject.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.Page
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        /// <summary>
        /// Handler for event of updating or changing the 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CustomPin> CustomPins { get; set; }
        public Action<CustomPin> PinActionClicked { get; set; }
        public uint PinsSize { get; set; }

        public MainPage()
        {
            base.BindingContext = this;

            InitializeComponent();

            CustomPins = new ObservableCollection<CustomPin>()
            {
                //new CustomPin() { Id = "a", Address = "California State University Long Beach", Details = "My University !", ImagePath = "CustomIconImage.png", PinZoomVisibilityMaximumLimit = 75, PinZoomVisibilityMinimumLimit = 20, PinSize = 50},
                new CustomPin() { Id = "b", Address = "Ruaudin", Details = "Where I'm coming from.", ImagePath = "CustomIconImage.png", PinZoomVisibilityMaximumLimit = 75, PinZoomVisibilityMinimumLimit = 35, PinSize = 35},
                new CustomPin() { Id = "c", Address = "Chelles", Details = "Someone there.", ImagePath = "CustomIconImage.png", PinZoomVisibilityMaximumLimit = 75, PinZoomVisibilityMinimumLimit = 0, PinSize = 65 },
                new CustomPin() { Id = "d", Address = "Lille", Details = "Le nord..", ImagePath = "CustomIconImage.png", PinZoomVisibilityMaximumLimit = 100, PinZoomVisibilityMinimumLimit = 20, PinSize = 80 },
                //new CustomPin(new Position(33.789618, -118.137626)) { Id = "e", Details = "I have been there ! :o", ImagePath = "CustomIconImage.png", PinZoomVisibilityMaximumLimit = 100, PinZoomVisibilityMinimumLimit = 20, PinSize = 40 },
                //new CustomPin() { Id = "f", Address = "Rome, Italy", Details = "A trip..", ImagePath = "CustomIconImage.png", PinZoomVisibilityMaximumLimit = 100, PinZoomVisibilityMinimumLimit = 50, PinSize = 55 }
            };

            PinActionClicked = PinClickedCallback;

            PinsSize = Convert.ToUInt32(200);
        }

        private void PinClickedCallback(CustomPin customPinClicked)
        {
            Debug.WriteLine("{0}: {1}/{2}", customPinClicked.Address, customPinClicked.Location.Latitude, customPinClicked.Location.Longitude);
        }

        public void PinsCollectionChanged()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomPins)));
        }
    }
}
