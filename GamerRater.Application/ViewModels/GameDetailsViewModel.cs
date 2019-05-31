using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;

namespace GamerRater.Application.ViewModels
{
    public class GameDetailsViewModel : Observable
    {
        public static ObservableCollection<Review> _reviews = new ObservableCollection<Review>();
        private int _averageScore = -1; //-1 equals 0 stars.
        private Visibility _showReviewEditor = Visibility.Collapsed;
        public ObservableCollection<Platform> Platforms = new ObservableCollection<Platform>();

        public GameDetailsViewModel()
        {
            Session = UserAuthenticator.SessionUserAuthenticator;
        }

        public ICommand CloseReviewWriter => new RelayCommand(() => ShowReviewEditor = Visibility.Collapsed);
        public ICommand AddReviewCommand { get; set; }
        public GameRoot MainGame { get; set; }
        public GameDetailsPage Page { get; set; }
        public UserAuthenticator Session { get; set; }

        public Visibility ShowReviewEditor
        {
            get => _showReviewEditor;
            set => Set(ref _showReviewEditor, value);
        }

        public int AverageScore
        {
            get => _averageScore;
            set => Set(ref _averageScore, value);
        }

        public ObservableCollection<Review> Reviews
        {
            get => _reviews;
            set => Set(ref _reviews, value);
        }

        public ICommand OpenReviewWriter => new RelayCommand(() =>
        {
            ShowReviewEditor = Visibility.Visible;
            Page.BringViewToReviewEditBox();
        });


        /// <summary>Initializes the specified game for detailed view</summary>
        /// <param name="game">The game.</param>
        public async void Initialize(GameRoot game)
        {
            MainGame = game;
            AddReviewCommand = new RelayCommand<Review>(InitializeAddReview);
            _reviews?.Clear();
            if (!NetworkInterface.GetIsNetworkAvailable()) return;
            await InitializePlatforms().ConfigureAwait(true);
            await InitializeReviews().ConfigureAwait(true);
        }

        /// <summary>Initializes process of fetching all reviews related to current game</summary>
        /// <returns></returns>
        public async Task<bool> InitializeReviews()
        {
            Reviews.Clear();
            var conn = new Games();
            var gameFromDb = await conn.GetGame(MainGame).ConfigureAwait(true);
            if (gameFromDb == null) return false;
            foreach (var rating in gameFromDb.Reviews)
            {
                using (var usersConn = new Users())
                {
                    var user = await usersConn.GetUser(rating.UserId).ConfigureAwait(true);
                    if (user == null)
                    {
                        GrToast.SmallToast(GrToast.Errors.NetworkError);
                        return false;
                    }

                    rating.User = user;
                }

                Reviews.Add(rating);
            }

            SetAverageScore();
            return true;
        }

        /// <summary>Initializes process of fetching all platforms related to current game</summary>
        /// <returns></returns>
        public async Task InitializePlatforms()
        {
            if (MainGame.PlatformsIds == null || MainGame.PlatformsIds.Length == 0) return;
            try
            {
                using (var igdb = new IgdbAccess())
                {
                    MainGame.PlatformList = await igdb.GetPlatformsAsync(MainGame).ConfigureAwait(true);
                    foreach (var platform in MainGame.PlatformList) Platforms.Add(platform);
                }
            }
            catch (HttpRequestException)
            {
                GrToast.SmallToast(GrToast.Errors.IgdbError);
            }
        }

        /// <summary>  Problem with connection. Reports to user wether Network or Api problem.</summary>
        public static void NoConnection()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                GrToast.SmallToast(GrToast.Errors.ApiError);
                return;
            }

            GrToast.SmallToast(GrToast.Errors.NetworkError);
        }

        /// <summary>Initializes the process of adding a review.</summary>
        /// <param name="review">The review.</param>
        public async void InitializeAddReview(Review review)
        {
            Page.EnableReviewSubmitButton(false);
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                NoConnection();
                Page.EnableReviewSubmitButton(true);
                return;
            }

            if (review.Stars == -1)
            {
                Page.RatingGridBorderColor(true);
                Page.EnableReviewSubmitButton(true);
                return;
            }

            Page.RatingGridBorderColor(false);
            if (!await CheckIfGameExistsValidation().ConfigureAwait(true))
            {
                GrToast.SmallToast(GrToast.Errors.ApiError);
                return;
            }

            if (!await AddReview(review).ConfigureAwait(true))
            {
                GrToast.SmallToast(GrToast.Errors.AddReviewError);
                return;
            }

            ShowReviewEditor = Visibility.Collapsed;
            Page.ShowReviewBox(true);
            Page.BringViewToReviews();
            Page.ClearReviewBox(null, null);
        }

        /// <summary>Adds the review to database</summary>
        /// <param name="review">The review.</param>
        /// <returns>true if success, else false.</returns>
        private async Task<bool> AddReview(Review review)
        {
            review.date = DateTime.UtcNow;
            var reviewsConn = new Reviews();
            using (reviewsConn)
            {
                if (review.Id != 0)
                {
                    if (!await reviewsConn.UpdateReview(review).ConfigureAwait(true))
                        return false;
                }
                else
                {
                    if (!await reviewsConn.AddReview(review).ConfigureAwait(true))
                        return false;
                }
            }

            await UserAuthenticator.SessionUserAuthenticator.UpdateUser().ConfigureAwait(true);
            UpdateReviews(review);
            return true;
        }

        /// <summary>Checks if game exists within the database.</summary>
        /// <returns>true if found, else false</returns>
        private async Task<bool> CheckIfGameExistsValidation()
        {
            using (var game = new Games())
            {
                var gameFromDb = await game.GetGame(MainGame).ConfigureAwait(true);
                if (gameFromDb != null) return true;
                var result = await game.AddGame(MainGame).ConfigureAwait(true);
                if (result.StatusCode != HttpStatusCode.Created) return false;
            }

            return true;
        }

        /// <summary>Updates UI list containing reviews</summary>
        /// <param name="newReview">The new review.</param>
        public async void UpdateReviews(Review newReview)
        {
            //Removes old review so new one can be loaded.
            if (newReview.Id != 0)
                Reviews.Remove(Reviews.Single(x => x.Id == newReview.Id));
            var conn = new Games();

            var gameFromDb = await conn.GetGame(MainGame).ConfigureAwait(true);
            if (gameFromDb == null) return;
            {
                foreach (var review in gameFromDb.Reviews)
                {
                    using (var usersConn = new Users())
                    {
                        var user = await usersConn.GetUser(review.UserId).ConfigureAwait(true);
                        if (user == null)
                        {
                            GrToast.SmallToast(GrToast.Errors.NetworkError);
                            return;
                        }

                        review.User = user;
                    }

                    if (Reviews.All(x => x.Id != review.Id))
                        Reviews.Insert(0, review);
                }

                SetAverageScore();
                Page.EnableReviewSubmitButton(true);
            }
        }

        /// <summary>Sets the average GamerRater score based on all available reviews related to current game</summary>
        private void SetAverageScore()
        {
            if (Reviews.Count == 0)
                return;
            var avg = 0.00;
            foreach (var review in Reviews) avg += review.Stars;
            AverageScore = Convert.ToInt32(avg / Reviews.Count);
        }

        /// <summary>Deletes the review from database. If success, removes it from UI review list.</summary>
        /// <param name="review">The review.</param>
        public async void DeleteReview(Review review)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 0);
            if (await new Reviews().DeleteReview(review.Id).ConfigureAwait(true))
                if (await UserAuthenticator.SessionUserAuthenticator.UpdateUser().ConfigureAwait(true))
                {
                    Reviews.Remove(review);
                    SetAverageScore();
                    Page.EnableReviewSubmitButton(true);
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
                    return;
                }

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            GrToast.SmallToast(GrToast.Errors.DeleteReviewError);
        }
    }
}
