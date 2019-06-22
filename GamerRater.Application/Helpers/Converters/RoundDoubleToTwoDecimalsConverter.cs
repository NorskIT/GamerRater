using System;
using Windows.UI.Xaml.Data;

namespace GamerRater.Application.Helpers.Converters
{
    internal class RoundDoubleToTwoDecimalsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Concat(Math.Round(double.Parse(value.ToString()), 1));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
