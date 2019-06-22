using System;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GamerRater.Application.Helpers
{
    public class NavHelper
    {
        public static readonly DependencyProperty NavigateToProperty =
            DependencyProperty.RegisterAttached("NavigateTo", typeof(Type), typeof(NavHelper),
                new PropertyMetadata(null));

        public static Type GetNavigateTo(NavigationViewItem item)
        {
            return (Type) item.GetValue(NavigateToProperty);
        }

        public static void SetNavigateTo(NavigationViewItem item, Type value)
        {
            item.SetValue(NavigateToProperty, value);
        }
    }
}
