using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.ViewModels
{
    public class GameDetailsViewModel : Observable
    {
        public static ObservableCollection<Review> _reviews = new ObservableCollection<Review>();
        private int _averageScore = -1;

        private Visibility _showReviewEditor = Visibility.Collapsed;

        public ICommand AddReviewCommand;
        public GameRoot MainGame;
        public GameDetailsPage Page;
        public ObservableCollection<Platform> Platforms = new ObservableCollection<Platform>();
        public UserAuthenticator Session = UserAuthenticator.SessionUserAuthenticator;

        public ICommand CloseReviewWriter => new RelayCommand(() => ShowReviewEditor = Visibility.Collapsed);

        public ICommand OpenReviewWriter => new RelayCommand(() =>
        {
            ShowReviewEditor = Visibility.Visible;
            Page.BringViewToReviewEditBox();
        });

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


        public async void Initialize()
        {
            if (MainGame == null) return;
            AddReviewCommand = new RelayCommand<Review>(AddReview);
            if (!NetworkInterface.GetIsNetworkAvailable()) return;
            await InitializeReviews().ConfigureAwait(true);
            await InitializePlatforms().ConfigureAwait(true);
        }

        public async Task InitializeReviews()
        {
            Reviews.Clear();
            try
            {
                try
                {
                    var conn = new Games();
                    var gameFromDb = await conn.GetGame(MainGame).ConfigureAwait(true);
                    if (gameFromDb != null) { 
                        foreach (var rating in gameFromDb.Reviews)
                        {
                            using (var usersConn = new Users())
                                rating.User = await usersConn.GetUser(rating.UserId).ConfigureAwait(true);
                            Reviews.Add(rating);
                        }
                        SetAverageScore();
                    }
                }
                catch (JsonReaderException)
                {
                    GrToast.SmallToast("Could not fetch reviews.. Please check your network connection and reload page.");
                }
            }
            catch (HttpRequestException)
            {
                GrToast.SmallToast("API request does not respond, please try again later.");
            }
        }

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
            } //TODO: Different excpetion
            catch (HttpRequestException)
            {
                GrToast.SmallToast("Could not reach IGDB Servers. Please check your network connection and try again.");
            }
        }

        public static void NoConnection()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                GrToast.SmallToast("Could not reach database... Our server might be down, please try again later.");
                return;
            }
            GrToast.SmallToast("Could not connect to server... Please check your network connection.");
        }

        public async void AddReview(Review review)
        {
            Page.EnableReviewSubmitButton(false);
            if (!await new PingApi().CheckConnection().ConfigureAwait(true))
            {
                NoConnection();
                Page.EnableReviewSubmitButton(true);
                return;
            }
            if (review.Stars == -1)
            {
                Page.RatingGridBorderColor(true);
                return;
            }
            Page.RatingGridBorderColor(false);
            try
            {
                using (var game = new Games())
                {
                    var gameFromDb = await game.GetGame(MainGame).ConfigureAwait(true);
                    if (gameFromDb == null) { 
                        var result = await game.AddGame(MainGame).ConfigureAwait(true);
                        if (result.StatusCode != HttpStatusCode.Created) {
                            //TODO: FIX, not throw here
                            throw new HttpRequestException();
                        }
                    }
                }
            }
            catch (HttpRequestException)
            {
                GrToast.SmallToast("Could not connect to server. Please try again.");
                Page.RatingGridBorderColor(true);
                return;
            }

            review.date = DateTime.UtcNow;
            if (review.Id != 0)
            {
                if (!await new Reviews().UpdateReview(review).ConfigureAwait(true))
                    return;
            }
            else
            {
                if (!await new Reviews().AddReview(review).ConfigureAwait(true))
                    return;
            }
            
            ShowReviewEditor = Visibility.Collapsed;
            Page.ShowReviewBox(true);
            //TODO: Exception, could not update user
            await UserAuthenticator.SessionUserAuthenticator.UpdateUser().ConfigureAwait(true);
            UpdateRatings(review);
            Page.BringViewToReviews();
            Page.ClearReviewBox(null, null);
        }

        public async void UpdateRatings(Review newReview)
        {
            //Removes old review so new one can be loaded.
            if (newReview.Id != 0)
                Reviews.Remove(Reviews.Single(x => x.Id == newReview.Id));
            var conn = new Games();

            var gameFromDb = await conn.GetGame(MainGame);
            if (gameFromDb != null)
            {
                foreach (var review in gameFromDb.Reviews)
                {
                    using (var usersConn = new Users())
                        review.User = await usersConn.GetUser(review.UserId).ConfigureAwait(true);
                    if (Reviews.All(x => x.Id != review.Id))
                        Reviews.Insert(0, review);
                }

                SetAverageScore();
                Page.EnableReviewSubmitButton(true);
            }

            //TODO: NO INTERNET
        }

        private void SetAverageScore()
        {
            if (Reviews.Count == 0)
                return;
            var avg = 0.00;
            foreach (var review in Reviews) avg += review.Stars;
            AverageScore = Convert.ToInt32(avg / Reviews.Count);
        }

        public async void DeleteReview(Review review)
        {
            if (await new Reviews().DeleteReview(review.Id).ConfigureAwait(true))
                if (await UserAuthenticator.SessionUserAuthenticator.UpdateUser().ConfigureAwait(true))
                {
                    Reviews.Remove(review);
                    SetAverageScore();
                }

            //TODO: NO INTERNET
        }
    }
}
