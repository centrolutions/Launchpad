using Launchpad.Devices;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Launchpad.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //color incoming
            var color = (ButtonColor)value;
            var paramColor = (ButtonColor)parameter;

            return color == paramColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //boolean incoming
            var isTrue = (bool)value;

            if (isTrue)
                return (ButtonColor)parameter;

            return default(ButtonColor);
        }
    }
}
