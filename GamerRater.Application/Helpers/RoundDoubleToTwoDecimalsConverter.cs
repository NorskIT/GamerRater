using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GamerRater.Application.Helpers
{
    internal class RoundDoubleToTwoDecimalsConverter : ResourceDictionary, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Concat(Math.Round(double.Parse(value.ToString()), 1));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
