using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioButtons.Converters
{
    public class RgbToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string rgbString)
            {
                rgbString = rgbString.Trim('(', ')');
                var colors = rgbString.Split(',');
                if (colors.Length == 3 &&
                    byte.TryParse(colors[0], out byte red) &&
                    byte.TryParse(colors[1], out byte green) &&
                    byte.TryParse(colors[2], out byte blue))
                {
                    return Color.FromRgb(red, green, blue);
                }
            }
            return Color.FromRgb(0,0,0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
