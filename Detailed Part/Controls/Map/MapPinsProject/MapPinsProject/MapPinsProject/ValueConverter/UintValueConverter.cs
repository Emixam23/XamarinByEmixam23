using System;
using System.Globalization;
using Xamarin.Forms;

namespace MapPinsProject.ValueConverter
{
    public class UintValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is string)
                {
                    try
                    {
                        return UInt32.Parse(value as string);
                    }
                    catch (Exception) { return value; }
                }
                else if (value is uint)
                    return ((uint)value);
                else
                    return value;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
