using MapPinsProject.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MapPinsProject.ValueConverter
{
    public class ActionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value as Action<CustomPin>;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
