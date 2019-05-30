using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using GamerRater.Application.Services;

namespace GamerRater.Application.Helpers.Converters
{
    class ValueLengthToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return new SolidColorBrush(Windows.UI.Colors.DarkGray);
            var s = (string) value;
            return ((s.Length > 2 && s.Length < 16) || s.Length == 0)
                ? new SolidColorBrush(Windows.UI.Colors.DarkGray) : new SolidColorBrush(Windows.UI.Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

