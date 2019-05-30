using System;
using Windows.UI.Xaml;
using GamerRater.Application.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GamerRater.Application.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
            ViewModel.CheckCache();
            ViewModel.Page = this;
        }

        public void WaitVisual(bool enabled)
        {
            if (enabled)
            {
                GameRootSearchBar.IsEnabled = false;
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 0);
            }
            else
            {
                GameRootSearchBar.IsEnabled = true;
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            }
        }
    }
}
