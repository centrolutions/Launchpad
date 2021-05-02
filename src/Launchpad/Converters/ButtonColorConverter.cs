using Launchpad.Devices;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Launchpad.Converters
{
    public class ButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ButtonColor)value)
            {
                case ButtonColor.None:
                    return Colors.LightGray;
                case ButtonColor.Green:
                    return Colors.Green;
                case ButtonColor.Red:
                    return Colors.Red;
                case ButtonColor.Orange:
                    return Colors.Orange;
                default:
                    return Colors.LightGray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
