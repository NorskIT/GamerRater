using System;
using System.Net.NetworkInformation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using GamerRater.Model;

namespace GamerRater.Application.Helpers.Converters
{
    internal class NullToDefaultImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || !NetworkInterface.GetIsNetworkAvailable())
                return new BitmapImage(new Uri("ms-appx:///Assets/DefaultImage.png"));
            if (!(value is GameCover cover)) return new BitmapImage(new Uri("ms-appx:///Assets/DefaultImage.png"));
            return new BitmapImage(new Uri(cover.url));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
