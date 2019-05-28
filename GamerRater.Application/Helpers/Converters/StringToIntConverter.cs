using System;
using Windows.UI.Xaml.Data;

namespace GamerRater.Application.Helpers.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.Equals("") ? 0 : int.Parse((string) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return typeof(NotImplementedException);
        }
    }
}
