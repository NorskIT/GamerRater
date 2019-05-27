﻿using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using GamerRater.Application.Views;

namespace GamerRater.Application.Helpers
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            return value.Equals("") ? 0 : int.Parse((string) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return typeof(NotImplementedException);
        }
    }
}
