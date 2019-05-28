using System;
using Windows.UI.Xaml.Data;

namespace GamerRater.Application.Helpers.Converters
{
    public class DoubleToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? 0 : System.Convert.ToInt32((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return typeof(NotImplementedException);
        }
    }
}
