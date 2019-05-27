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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is GameRoot game)
            {
                ViewModel.Initialize(game, this);
            }
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

        public void ComboBoxBorderColor(bool x)
        {
            StarsBox.BorderBrush = x ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Gray);
        }

        private void UpdateReview(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var review = (Review)button.DataContext;
            ReviewText.Text = review.ReviewText;
            ReviewId.Text = review.Id.ToString();
            StarsBox.SelectedIndex = (review.Stars==0) ? 0 : review.Stars-1;
            ViewModel.ShowReviewEditor = Visibility.Visible;
            ScrollViewer.SetBringIntoViewOnFocusChange(WriteReviewBox, true);
            ScrollViewer.StartBringIntoView();
            StartBringIntoView();
        }

        private void ClearReviewBox(object sender, RoutedEventArgs e)
        {
            ReviewText.Text = "";
            ReviewId.Text = "";
            StarsBox.SelectedIndex = 0;
        }
    }
}
