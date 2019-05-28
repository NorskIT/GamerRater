using System;

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
            GameRootSearchBar.IsEnabled = enabled;
        }
    }
}
