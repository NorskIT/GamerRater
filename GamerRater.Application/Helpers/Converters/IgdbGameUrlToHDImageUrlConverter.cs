using System;
using System.Net.NetworkInformation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using GamerRater.Model;

namespace GamerRater.Application.Helpers.Converters
{
    internal class IgdbGameUrlToHDImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var defaultImage = new BitmapImage(new Uri("ms-appx:///Assets/DefaultImage.png"));
            if (value == null || !NetworkInterface.GetIsNetworkAvailable()) return defaultImage;
            if (!(value is GameCover cover)) return defaultImage;
            cover.url = "https://images.igdb.com/igdb/image/upload/t_720p/" + cover.image_id + ".jpg";
            return new BitmapImage(new Uri(cover.url));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
