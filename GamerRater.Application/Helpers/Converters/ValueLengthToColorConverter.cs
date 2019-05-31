using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace GamerRater.Application.Helpers.Converters
{
    internal class ValueLengthToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return new SolidColorBrush(Colors.RoyalBlue);
            var s = (string) value;
            return s.Length > 2 && s.Length < 16 || s.Length == 0
                ? new SolidColorBrush(Colors.RoyalBlue)
                : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
