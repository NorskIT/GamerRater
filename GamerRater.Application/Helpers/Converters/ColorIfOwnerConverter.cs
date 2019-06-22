using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using GamerRater.Application.Services;

namespace GamerRater.Application.Helpers.Converters
{
    public class ColorIfOwnerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (UserAuthenticator.SessionUserAuthenticator.User == null) return new SolidColorBrush(Colors.White);
            return (int) value == UserAuthenticator.SessionUserAuthenticator.User.Id
                ? new SolidColorBrush(Colors.LightSkyBlue)
                : new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
