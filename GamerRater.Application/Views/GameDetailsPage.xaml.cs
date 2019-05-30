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
            ReviewEditBox.Visibility = Visibility.Visible;
            ReviewEditBox.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!(e.Parameter is GameRoot game)) return;
            ViewModel.Initialize(game);
        }

        private void DeleteReviewButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button)) return;
            if (button.DataContext is Review review)
            {
                button.IsEnabled = false;
                ViewModel.DeleteReview(review);
                button.IsEnabled = true;
            }
        }

        public void RatingGridBorderColor(bool x)
        {
            RatingStarsGrid.BorderBrush = x ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.White);
        }

        public void EnableReviewSubmitButton(bool enabled)
        {
            if(enabled)
            { 
                SubmitReviewButton.IsEnabled = enabled;
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            }
            else
            {
                SubmitReviewButton.IsEnabled = enabled;
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 0);
            }
        }

        private void SetReviewBox(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var review = (Review)button.DataContext;
            ReviewText.Text = review.ReviewText;
            //TODO: FIX
            ReviewId.Text = review.Id + "";
            RatingStars.Value = review.Stars;
            ViewModel.ShowReviewEditor = Visibility.Visible;
            EditReviewBoxText.Visibility = Visibility.Visible;
            WriteReviewBoxText.Visibility = Visibility.Collapsed;
            BringViewToReviewEditBox();
        }

        public void ClearReviewBox(object sender, RoutedEventArgs e)
        {
            ReviewText.Text = "";
            ReviewId.Text = "";
            RatingStars.Value = -1;
            EditReviewBoxText.Visibility = Visibility.Collapsed;
            WriteReviewBoxText.Visibility = Visibility.Visible;
        }

        public void BringViewToReviews()
        {
            BottomPage.StartBringIntoView();
        }

        public void BringViewToReviewEditBox()
        {
            BelowReviewEditBox.StartBringIntoView();
        }

        public void ShowReviewBox(bool b)
        {
            ReviewGridView.Visibility = Visibility.Visible;
        }
    }
}
