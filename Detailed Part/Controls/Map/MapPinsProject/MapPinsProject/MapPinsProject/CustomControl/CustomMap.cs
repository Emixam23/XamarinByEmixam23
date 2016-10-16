using MapPinsProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapPinsProject.CustomControl
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty CustomPinsProperty =
            BindableProperty.Create(nameof(CustomPins), typeof(IList<CustomPin>), typeof(CustomMap), null,
                propertyChanged: OnCustomPinsPropertyChanged);
        public IList<CustomPin> CustomPins
        {
            get { return (IList<CustomPin>)GetValue(CustomPinsProperty); }
            set { SetValue(CustomPinsProperty, value); }
        }

        public static readonly BindableProperty CameraFocusProperty =
            BindableProperty.Create(nameof(CameraFocus), typeof(CameraFocusData), typeof(CustomMap), null);
        public CameraFocusData CameraFocus
        {
            get { return (CameraFocusData)GetValue(CameraFocusProperty); }
            set { SetValue(CameraFocusProperty, value); }
        }

        public enum CameraFocusReference
        {
            None,
            OnPins
        }
        public static readonly BindableProperty CameraFocusParameterProperty =
            BindableProperty.Create(nameof(CameraFocusParameter), typeof(CameraFocusReference), typeof(CustomMap), CameraFocusReference.None);
        public CameraFocusReference CameraFocusParameter
        {
            get { return (CameraFocusReference)GetValue(CameraFocusParameterProperty); }
            set { SetValue(CameraFocusParameterProperty, value); }
        }

        public static readonly BindableProperty PinSizeProperty =
            BindableProperty.Create(nameof(PinSize), typeof(uint), typeof(CustomMap), Convert.ToUInt32(50));
        public uint PinSize
        {
            get { return (uint)GetValue(PinSizeProperty); }
            set { SetValue(PinSizeProperty, value); }
        }
        
        public static readonly BindableProperty PinSizedFromElementProperty =
            BindableProperty.Create(nameof(PinSizedFromElement), typeof(bool), typeof(CustomMap), false);
        public bool PinSizedFromElement
        {
            get { return (bool)GetValue(PinSizedFromElementProperty); }
            set { SetValue(PinSizedFromElementProperty, value); }
        }

        public static readonly BindableProperty PinImageSourceProperty =
            BindableProperty.Create(nameof(PinImageSource), typeof(ImageSource), typeof(CustomMap), null);
        public uint PinImageSource
        {
            get { return (uint)GetValue(PinImageSourceProperty); }
            set { SetValue(PinImageSourceProperty, value); }
        }

        public static readonly BindableProperty ZoomLevelProperty =
            BindableProperty.Create(nameof(ZoomLevel), typeof(Distance), typeof(CustomMap), new Distance());
        public Distance ZoomLevel
        {
            get { return (Distance)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        public static readonly BindableProperty PinZoomVisibilityLimitProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityLimit), typeof(double), typeof(CustomMap), 0.0);
        public double PinZoomVisibilityLimit
        {
            get { return (double)GetValue(PinZoomVisibilityLimitProperty); }
            set { SetValue(PinZoomVisibilityLimitProperty, value); }
        }

        public enum PinZoomVisibilityLimitUnityName
        {
            Kilometers,
            Meters,
            Miles
        }
        public static readonly BindableProperty PinZoomVisibilityLimitUnityProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityLimitUnity), typeof(PinZoomVisibilityLimitUnityName), typeof(CustomMap), PinZoomVisibilityLimitUnityName.Kilometers);
        public PinZoomVisibilityLimitUnityName PinZoomVisibilityLimitUnity
        {
            get { return (PinZoomVisibilityLimitUnityName)GetValue(PinZoomVisibilityLimitUnityProperty); }
            set { SetValue(PinZoomVisibilityLimitUnityProperty, value); }
        }

        public enum PinZoomVisibilityLimitSourceEnum
        {
            Map,
            None,
            Pin
        }
        public static readonly BindableProperty PinZoomVisibilityLimitSourceProperty =
            BindableProperty.Create(nameof(PinZoomVisibilityLimitSource), typeof(PinZoomVisibilityLimitSourceEnum), typeof(CustomMap), PinZoomVisibilityLimitSourceEnum.None);
        public PinZoomVisibilityLimitSourceEnum PinZoomVisibilityLimitSource
        {
            get { return (PinZoomVisibilityLimitSourceEnum)GetValue(PinZoomVisibilityLimitSourceProperty); }
            set { SetValue(PinZoomVisibilityLimitSourceProperty, value); }
        }

        public Action<CustomPin> PinClickedCallback { get; set; }

        #region Constructor
        public CustomMap()
        {
            this.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                CustomMap map = sender as CustomMap;
                if (map.VisibleRegion != null)
                {
                    this.ZoomLevel = (map.VisibleRegion.Radius);
                    //Debug.WriteLine("Xamarin Forms Map Radius: {0} Kilometers | {1} Meters | {2} Miles.", ZoomLevel.Kilometers, ZoomLevel.Meters, ZoomLevel.Miles);
                }
            };
        }
        #endregion

        #region Camera focus definition
        public class CameraFocusData
        {
            public Position Position { get; set; }
            public Distance Distance { get; set; }
        }
        private static void OnCustomPinsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap customMap = ((CustomMap)bindable);

            if (customMap.CameraFocusParameter == CameraFocusReference.OnPins)
            {
                List<double> latitudes = new List<double>();
                List<double> longitudes = new List<double>();

                foreach (CustomPin pin in (newValue as List<CustomPin>))
                {
                    latitudes.Add(pin.Position.Latitude);
                    longitudes.Add(pin.Position.Longitude);
                }

                double lowestLat = latitudes.Min();
                double highestLat = latitudes.Max();
                double lowestLong = longitudes.Min();
                double highestLong = longitudes.Max();
                double finalLat = (lowestLat + highestLat) / 2;
                double finalLong = (lowestLong + highestLong) / 2;

                double distance = DistanceCalculation.GeoCodeCalc.CalcDistance(lowestLat, lowestLong, highestLat, highestLong, DistanceCalculation.GeoCodeCalcMeasurement.Kilometers);

                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(finalLat, finalLong), Distance.FromKilometers(distance * 0.7)));
            }
        }
        private class DistanceCalculation
        {
            public static class GeoCodeCalc
            {
                public const double EarthRadiusInMiles = 3956.0;
                public const double EarthRadiusInKilometers = 6367.0;

                public static double ToRadian(double val) { return val * (Math.PI / 180); }
                public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

                public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
                {
                    return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Miles);
                }

                public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
                {
                    double radius = GeoCodeCalc.EarthRadiusInMiles;

                    if (m == GeoCodeCalcMeasurement.Kilometers) { radius = GeoCodeCalc.EarthRadiusInKilometers; }
                    return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
                }
            }

            public enum GeoCodeCalcMeasurement : int
            {
                Miles = 0,
                Kilometers = 1
            }
        }
        #endregion
    }
}