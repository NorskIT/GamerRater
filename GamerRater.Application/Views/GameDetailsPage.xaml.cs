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

        /// <summary>Invoked when the Page is loaded and becomes the current source of a parent Frame.</summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!(e.Parameter is GameRoot game)) return;
            ViewModel.Initialize(game);
            if (game.Reviews == null || game.Reviews.Count == 0)
            {
                ReviewGridView.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>Invoked when 'Delete review'-button has been clicked. Sends review to VIewModel review delete method</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>Enables/Disables the review submit button.</summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
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

        /// <summary>Sets review edit box with info from selected review</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SetReviewBox(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var review = (Review)button.DataContext;
            ReviewText.Text = review.ReviewText;
            ReviewId.Text = review.Id + "";
            RatingStars.Value = review.Stars;
            ViewModel.ShowReviewEditor = Visibility.Visible;
            EditReviewBoxText.Visibility = Visibility.Visible;
            WriteReviewBoxText.Visibility = Visibility.Collapsed;
            BringViewToReviewEditBox();
        }

        /// <summary>Clears the review box.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void ClearReviewBox(object sender, RoutedEventArgs e)
        {
            ReviewText.Text = "";
            ReviewId.Text = "";
            RatingStars.Value = -1;
            EditReviewBoxText.Visibility = Visibility.Collapsed;
            WriteReviewBoxText.Visibility = Visibility.Visible;
        }

        /// <summary>  Tries to bring the view to reviews.</summary>
        public void BringViewToReviews()
        {
            BottomPage.StartBringIntoView();
        }

        /// <summary>  Tries to bring the view to reviews edit box.</summary>
        public void BringViewToReviewEditBox()
        {
            BelowReviewEditBox.StartBringIntoView();
        }

        /// <summary>  Shows/Hides the reviews box</summary>
        public void ShowReviewBox(bool b)
        {
            ReviewGridView.Visibility = Visibility.Visible;
        }
    }
}
