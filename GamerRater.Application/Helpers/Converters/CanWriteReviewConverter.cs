using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using GamerRater.Application.Services;
using GamerRater.Application.ViewModels;

namespace GamerRater.Application.Helpers.Converters
{
    public class CanWriteReviewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (GameDetailsViewModel._reviews.Count == 0) return Visibility.Visible;
            if (UserAuthenticator.SessionUserAuthenticator.User == null) return Visibility.Visible;
            foreach (var review in GameDetailsViewModel._reviews)
                if (review.UserId == UserAuthenticator.SessionUserAuthenticator.User.Id)
                    return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
