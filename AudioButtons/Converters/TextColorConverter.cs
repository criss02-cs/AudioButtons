using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioButtons.Converters
{
    public class TextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string color)
            {
                color = color.Trim('(', ')');
                var colors = color.Split(',');
                if (colors.Length == 3 &&
                    byte.TryParse(colors[0], out byte red) &&
                    byte.TryParse(colors[1], out byte green) &&
                    byte.TryParse(colors[2], out byte blue))
                {
                    return IsColoreScuro(red, green, blue) ? Colors.White : Colors.Black;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private bool IsColoreScuro(byte red, byte green, byte blue)
        {
            // calcolo la luminosità del colore
            double luminosita = 0.299 * red + 0.587 * green + 0.114 * blue;
            // se la luminosità è inferiore a 0.5 allora è scuro
            return luminosita < 0.5;
        }
    }
}
