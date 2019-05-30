using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using GamerRater.Application.Services;

namespace GamerRater.Application.Helpers.Converters
{
    public class CheckIfUserIsOwner : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (UserAuthenticator.SessionUserAuthenticator.User == null) return Visibility.Collapsed;
            return (int)value == UserAuthenticator.SessionUserAuthenticator.User.Id ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
