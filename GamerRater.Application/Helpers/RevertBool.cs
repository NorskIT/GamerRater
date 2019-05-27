﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using GamerRater.Application.ViewModels;

namespace GamerRater.Application.Helpers
{
    public class RevertBool : ResourceDictionary, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
