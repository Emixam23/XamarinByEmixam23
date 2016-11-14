using MapPinsProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

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

            Debug.WriteLine("Intialization....");

            CustomPins = new ObservableCollection<CustomPin>()
            {
                new CustomPin() { Address="4720 E Atherton Street, Long Beach", Details = "Famous city for race driver !", ImagePath = "CustomIconImage.png"},
                new CustomPin() { Address = "Ruaudin", Details = "Where I'm coming from.", ImagePath = "CustomIconImage.png" },
                new CustomPin() { Address = "Chelles", Details = "Someone there.", ImagePath = "CustomIconImage.png" },
                new CustomPin() { Address = "Lille", Details = "Le nord..", ImagePath = "CustomIconImage.png" },
                new CustomPin() { Address = "Limoges", Details = "I have been there ! :o", ImagePath = "CustomIconImage.png" },
                new CustomPin() { Address = "Douarnenez", Details = "A trip..", ImagePath = "CustomIconImage.png" }
            };

            Debug.WriteLine("Initialization done.");

            PinActionClicked = PinClickedCallback;

            PinsSize = Convert.ToUInt32(100);

            InitializeComponent();
            Debug.WriteLine("Components done.");


            Task.Run(() =>
            {
                Task.Delay(5000);
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomPins)));
            });
        }

        private void PinClickedCallback(CustomPin customPinClicked)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine("{0}: {1}/{2}", customPinClicked.Address, customPinClicked.PinZoomVisibilityMinimumLimit, customPinClicked.PinZoomVisibilityMaximumLimit);
            });
        }
    }
}
