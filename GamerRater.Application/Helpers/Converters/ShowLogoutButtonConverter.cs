using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GamerRater.Application.Helpers.Converters
{
    internal class ShowLogoutButtonConverter : IValueConverter
    {
        //Bound to IsLoggedIn bool
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //IsLoggedIn true, show logout button, else do not show logout button
            return value != null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return typeof(NotImplementedException);
        }
    }
}
