using System;

using GamerRater.Application.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GamerRater.Model.IGDBModels;

namespace GamerRater.Application.Views
{
    public sealed partial class GameDetailsPage : Page
    {
        public GameDetailsViewModel ViewModel { get; } = new GameDetailsViewModel();

        public GameDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is GameRoot orderId)
            {
                ViewModel.Initialize(orderId);
            }
        }
    }
}
