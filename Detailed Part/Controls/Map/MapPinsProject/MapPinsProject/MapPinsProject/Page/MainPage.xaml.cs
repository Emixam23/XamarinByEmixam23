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

            CustomPins = new ObservableCollection<CustomPin>()
            {
                new CustomPin() { Address = "4720 E Atherton Street, Long Beach", Details = "Famous city for race driver !", ImagePath = "CustomIconImage.png"},
                new CustomPin() { Address = "Ruaudin", Details = "Where I'm coming from.", ImagePath = "CustomIconImage.png" },
                new CustomPin() { Address = "Chelles", Details = "Someone there.", ImagePath = "CustomIconImage.png" },
                new CustomPin() { Address = "Lille", Details = "Le nord..", ImagePath = "CustomIconImage.png" },
                new CustomPin(new Position(33.789618, -118.137626)) { Address = "Appart Alvista", Details = "I have been there ! :o", ImagePath = "CustomIconImage.png" },
                new CustomPin() { Address = "Rome, Italy", Details = "A trip..", ImagePath = "CustomIconImage.png" }
            };

            PinActionClicked = PinClickedCallback;

            PinsSize = Convert.ToUInt32(100);

            InitializeComponent();
        }

        private void PinClickedCallback(CustomPin customPinClicked)
        {
            Debug.WriteLine("{0}: {1}/{2}", customPinClicked.Address, customPinClicked.PinZoomVisibilityMinimumLimit, customPinClicked.PinZoomVisibilityMaximumLimit);
        }

        public void PinsCollectionChanged()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomPins)));
            Debug.WriteLine("Updated !!!");
        }
    }
}
