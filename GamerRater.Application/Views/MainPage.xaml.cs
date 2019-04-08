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
            Loaded += ViewModel.LoadGamesAsync;
        }
    }
}
