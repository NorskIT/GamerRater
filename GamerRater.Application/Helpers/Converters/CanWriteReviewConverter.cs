using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using GamerRater.Application.Services;
using GamerRater.Model;

namespace GamerRater.Application.Helpers.Converters
{
    public class CanWriteReviewConverter : ResourceDictionary, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ObservableCollection<Review> reviews)
            {
                if (reviews.Count == 0) return Visibility.Visible;
                if(UserAuthenticator.SessionUserAuthenticator.User == null) return Visibility.Visible;
                foreach (var review in reviews)
                {
                    if (review.UserId == UserAuthenticator.SessionUserAuthenticator.User.Id) return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;



        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
