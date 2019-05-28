using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using GamerRater.Application.Services;
using GamerRater.Application.Views;

namespace GamerRater.Application.Helpers.Converters
{
    class ShowLogoutButtonConverter : IValueConverter
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
