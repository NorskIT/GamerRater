using System;
using System.Net.NetworkInformation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using GamerRater.Application.Services;
using GamerRater.Application.Views;

namespace GamerRater.Application.Helpers.Converters
{
    public class ShowLoginButtonConverter : IValueConverter
    {
        //Bound to NotOnRegistrationLoginPage bool
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //If NotOnRegistrationLoginPage
            if (!(bool)value)
            {
                return Visibility.Collapsed;
            }
            //If user not logged in
            return (UserAuthenticator.SessionUserAuthenticator?.UserLoggedInBool == false) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
