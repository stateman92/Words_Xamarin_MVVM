using System;
using System.Globalization;

using Xamarin.Forms;

namespace Words_MVVM.Services
{
    public class ColorConverter : IValueConverter
    {
        // Return a Color (from hexadecimal) based on the value's value.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Color.FromHex(value.ToString());
        }

        // Do nothing, if an implementation needed, write it.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
