using System;
using System.Linq;
using Windows.UI.Xaml;
using GamerRater.Application.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GamerRater.Application.DataAccess;
using GamerRater.Model;

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
            if (e.Parameter is GameRoot game)
            {
                ViewModel.Initialize(game);
            }
        }

        private void AddGameButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.AddGame(ViewModel.MainGame);
        }

        private async void AddReviewToGame_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (await ViewModel.AddReview())
            {
                //TODO:Tell user review was added.
                return;
            }
            //TODO:Tell user it failed.
        }

        private async void DeleteReview(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(sender is Button button)) return;
                if (button.DataContext is Review review && await new Reviews().DeleteReview(review.Id))
                    ViewModel.Reviews.Remove(review);
            }
            catch (NullReferenceException ex)
            {
                //Tried connecting to db without internet
            }
        }
    }
}
