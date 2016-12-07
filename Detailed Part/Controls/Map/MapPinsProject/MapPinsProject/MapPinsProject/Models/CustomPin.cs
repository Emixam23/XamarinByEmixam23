using MapPinsProject.CustomControl;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using MapPinsProject.Page;

namespace MapPinsProject.Models
{
    public class CustomPin : BindableObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Handler for event of updating or changing the 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly BindableProperty AddressProperty =
            BindableProperty.Create(nameof(Address), typeof(string), typeof(CustomPin), "", BindingMode.TwoWay,
                propertyChanged: OnAddressPropertyChanged);
        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }
        private static void OnAddressPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CustomPin).SetAddress(newValue as string);
        }
        private async void SetAddress(string address)
        {
            if (setter == SetFrom.None)
            {
                setter = SetFrom.Address;
                SetLocation(await CustomMap.GetAddressPosition(address));
                setter = SetFrom.None;
                NotifyChanges();
            }
            else if (setter == SetFrom.Location)
            {
                setter = SetFrom.Done;
                SetValue(AddressProperty, address);
            }
        }

        private enum SetFrom
        {
            Address,
            Done,
            Location,
            None,
        }
        private SetFrom setter;

        private async void SetLocation(Position location)
        {
            if (setter == SetFrom.None)
            {
                setter = SetFrom.Location;
                SetAddress(await CustomMap.GetAddressName(location));
                setter = SetFrom.None;
                NotifyChanges();
            }
            else if (setter == SetFrom.Address)
            {
                setter = SetFrom.Done;
                SetValue(LocationProperty, location);
            }
        }

        public static readonly BindableProperty LocationProperty =
          BindableProperty.Create(nameof(Location), typeof(Position), typeof(CustomPin), new Position(Double.MaxValue, Double.MaxValue), BindingMode.TwoWay,
              propertyChanged: OnLocationPropertyChanged);
        public Position Location
        {
            get { return (Position)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }
        private static async void OnLocationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CustomPin).SetLocation((Position)newValue);
        }

        private void NotifyChanges()
        {
            (App.Current.MainPage as MainPage).PinsCollectionChanged();
        }

        public string Name { get; set; }
        public string Details { get; set; }
        public string ImagePath { get; set; }
        public uint PinSize { get; set; }
        public uint PinZoomVisibilityMinimumLimit { get; set; }
        public uint PinZoomVisibilityMaximumLimit { get; set; }
        public Point AnchorPoint { get; set; }
        public Action<CustomPin> PinClickedCallback { get; set; }
        private bool hasALocation;
        public string Id { get; set; }

        public CustomPin(Position location)
        {
            setter = SetFrom.None;
            Location = location;
            Name = "";
            Details = "";
            ImagePath = "";
            PinSize = 50;
            PinZoomVisibilityMinimumLimit = uint.MinValue;
            PinZoomVisibilityMaximumLimit = uint.MaxValue;
            AnchorPoint = new Point(0.5, 1);
            PinClickedCallback = null;
            Id = "";
        }
        public CustomPin(string address)
        {
            setter = SetFrom.None;
            Address = address;
            Name = "";
            Details = "";
            ImagePath = "";
            PinSize = 50;
            PinZoomVisibilityMinimumLimit = uint.MinValue;
            PinZoomVisibilityMaximumLimit = uint.MaxValue;
            AnchorPoint = new Point(0.5, 1);
            PinClickedCallback = null;
            Id = "";
        }
        public CustomPin()
        {
            setter = SetFrom.None;
            Name = "";
            Details = "";
            ImagePath = "";
            PinSize = 50;
            PinZoomVisibilityMinimumLimit = uint.MinValue;
            PinZoomVisibilityMaximumLimit = uint.MaxValue;
            AnchorPoint = new Point(0.5, 1);
            PinClickedCallback = null;
            Id = "";
        }
    }
}
