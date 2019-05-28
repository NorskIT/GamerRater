using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using GamerRater.Application.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GamerRater.Application.DataAccess;
using GamerRater.Model;
using Microsoft.Toolkit.Uwp.UI.Extensions;

namespace GamerRater.Application.Views
{
    public sealed partial class GameDetailsPage : Page
    {


        public GameDetailsViewModel ViewModel { get; } = new GameDetailsViewModel();

        public GameDetailsPage()
        {
            InitializeComponent();
            ViewModel.Page = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!(e.Parameter is GameRoot game)) return;
            ViewModel.MainGame = game;
            ViewModel.Initialize();
        }

        private async void DeleteReview(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!(sender is Button button)) return;
                if (button.DataContext is Review review)
                    ViewModel.DeleteReview(review);
            }
            catch (NullReferenceException ex)
            {
                //Tried connecting to db without internet
            }
        }

        public void RatingGridBorderColor(bool x)
        {
            RatingStarsGrid.BorderBrush = x ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Gray);
        }

        private void SetReviewBox(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var review = (Review)button.DataContext;
            ReviewText.Text = review.ReviewText;
            ReviewId.Text = review.Id.ToString();
            RatingStars.Value = review.Stars;
            ViewModel.ShowReviewEditor = Visibility.Visible;
            BringViewToReviewEditBox();
        }

        public void ClearReviewBox(object sender, RoutedEventArgs e)
        {
            ReviewText.Text = "";
            ReviewId.Text = "";
            RatingStars.Value = -1;
        }

        public void BringViewToReviews()
        {
            ReviewGridView.StartBringIntoView();
        }

        public void BringViewToReviewEditBox()
        {
            ReviewEditBox.StartBringIntoView();
        }

        private void WriteReviewButtonClicked(object sender, RoutedEventArgs e)
        {
            BringViewToReviewEditBox();
        }
    }
}
