using MapPinsProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        #region Camera focus method
        private static void OnCustomPinsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap customMap = ((CustomMap)bindable);

            if (customMap.CameraFocusParameter == CameraFocusReference.OnPins)
            {
                List<Position> PositionPins = new List<Position>();
                bool onlyOnePointPresent;

                foreach (CustomPin pin in (newValue as List<CustomPin>))
                {
                    PositionPins.Add(pin.Position);
                }
                Position CentralPosition = GetCentralPosition(PositionPins);
                if (PositionPins.Count > 1)
                {
                    Position[] FarestPoints = GetTwoFarestPointsOfCenterPointReference(PositionPins, CentralPosition);
                    customMap.CameraFocus = GetPositionAndZoomLevelForCameraAboutPositions(FarestPoints);
                    onlyOnePointPresent = false;
                }
                else
                {
                    customMap.CameraFocus = new CameraFocusData() { Position = CentralPosition };
                    onlyOnePointPresent = true;
                }
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(customMap.CameraFocus.Position,
                (!onlyOnePointPresent) ? (customMap.CameraFocus.Distance) : (new Distance(5))));
            }
        }
        public static Position GetCentralPosition(List<Position> positions)
        {
            if (positions.Count == 1)
            {
                foreach (Position pos in positions)
                {
                    return (pos);
                }
            }

            double lat = 0;
            double lng = 0;

            foreach (var pos in positions)
            {
                lat += pos.Latitude;
                lng += pos.Longitude;
            }

            var total = positions.Count;

            lat = lat / total;
            lng = lng / total;

            return new Position(lat, lng);
        }
        public class DataCalc
        {
            public Position Pos { get; set; }
            public double Distance { get; set; }
        }
        // bug ici a test !
        public static Position[] GetTwoFarestPointsOfCenterPointReference(List<Position> farestPosition, Position centerPosition)
        {
            Position[] FarestPos = new Position[2];
            List<DataCalc> dataCalc = new List<DataCalc>();

            foreach (Position pos in farestPosition)
            {
                dataCalc.Add(new DataCalc()
                {
                    Pos = pos,
                    Distance = Math.Sqrt(Math.Pow(pos.Latitude - centerPosition.Latitude, 2) + Math.Pow(pos.Longitude - centerPosition.Longitude, 2))
                });
            }

            DataCalc First = new DataCalc() { Distance = 0 };
            foreach (DataCalc dc in dataCalc)
            {
                if (dc.Distance > First.Distance)
                {
                    First = dc;
                }
            }

            DataCalc Second = new DataCalc() { Distance = 0 };
            foreach (DataCalc dc in dataCalc)
            {
                if (dc.Distance > Second.Distance
                    && (dc.Pos.Latitude != First.Pos.Latitude && dc.Pos.Longitude != First.Pos.Longitude))
                {
                    Second = dc;
                }
            }

            FarestPos[0] = First.Pos;
            FarestPos[1] = Second.Pos;

            return (FarestPos);
        }
        public class CameraFocusData
        {
            public Position Position { get; set; }
            public Distance Distance { get; set; }
        }
        public static CameraFocusData GetPositionAndZoomLevelForCameraAboutPositions(Position[] FarestPoints)
        {
            double earthRadius = 6371000; //metros

            Position pos1 = FarestPoints[0];
            Position pos2 = FarestPoints[1];

            double latitud1Radianes = pos1.Latitude * (Math.PI / 180.0);
            double latitud2Radianes = pos2.Latitude * (Math.PI / 180.0);
            double longitud1Radianes = pos2.Longitude * (Math.PI / 180.0);
            double longitud2Radianes = pos2.Longitude * (Math.PI / 180.0);

            double deltaLatitud = (pos2.Latitude - pos1.Latitude) * (Math.PI / 180.0);
            double deltaLongitud = (pos2.Longitude - pos1.Longitude) * (Math.PI / 180.0);

            double sum1 = Math.Sin(deltaLatitud / 2) * Math.Sin(deltaLatitud / 2);
            double sum2 = Math.Cos(latitud1Radianes) * Math.Cos(latitud2Radianes) * Math.Sin(deltaLongitud / 2) * Math.Sin(deltaLongitud / 2);

            var a = sum1 + sum2;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = earthRadius * c;

            /* lt is deltaLatitud
             * lng is deltaLongitud*/
            var Bx = Math.Cos(latitud2Radianes) * Math.Cos(deltaLongitud);
            var By = Math.Cos(latitud2Radianes) * Math.Sin(deltaLongitud);
            var lt = Math.Atan2(Math.Sin(latitud1Radianes) + Math.Sin(latitud2Radianes),
                                Math.Sqrt((Math.Cos(latitud1Radianes) + Bx) * (Math.Cos(latitud2Radianes) + Bx) + By * By));//Latitud del punto medio
            var lng = longitud1Radianes + Math.Atan2(By, Math.Cos(longitud1Radianes) + Bx);//Longitud del punto medio

            return (new CameraFocusData() { Position = new Position(lt, lng), Distance = new Distance(distance + 0.2) });
        }
        #endregion
    }
}


/*
 public static Position GetCentralPosition(List<Position> positions)
        {
            if (positions.Count == 1)
            {
                foreach (Position pos in positions)
                {
                    return (pos);
                }
            }

            double x = 0;
            double y = 0;
            double z = 0;

            foreach (var pos in positions)
            {
                var latitude = pos.Latitude * Math.PI / 180;
                var longitude = pos.Longitude * Math.PI / 180;

                Debug.WriteLine("|Lat = {0}|Long = {1}|", latitude, longitude);

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
            }

            Debug.WriteLine("Moyennes => |x = {0}|y = {1}|z = {2}|", x, y, z);

            var total = positions.Count;

            Debug.WriteLine("Nombre de positions => |total = {0}|", total);

            x = x / total;
            y = y / total;
            z = z / total;

            Debug.WriteLine("Moyennes => |x = {0}|y = {1}|z = {2}|", x, y, z);

            var centralLongitude = Math.Atan2(y, x);
            var centralSquareRoot = Math.Sqrt(x * x + y * y);
            var centralLatitude = Math.Atan2(z, centralSquareRoot);

            Debug.WriteLine("Final LAT/LNG => |lat = {0}|lng = {1}|", centralLatitude * 180 / Math.PI, centralLongitude * 180 / Math.PI);

            return new Position(centralLatitude * 180 / Math.PI, centralLongitude * 180 / Math.PI);
        }
     */
