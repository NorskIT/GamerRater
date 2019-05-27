using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using GamerRater.Application.Services;

namespace GamerRater.Application.Helpers
{
    public class VisibilityListSizeConvert : ResourceDictionary, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
