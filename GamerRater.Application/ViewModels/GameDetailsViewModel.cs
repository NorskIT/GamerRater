using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;

namespace GamerRater.Application.ViewModels
{
    public class GameDetailsViewModel : Observable
    {
        public GameRoot MainGame;
        public GameDetailsPage Page;
        public ObservableCollection<Platform> Platforms = new ObservableCollection<Platform>();
        public ObservableCollection<int> Stars = new ObservableCollection<int>() {1,2,3,4,5};
        private User _loggedUser;

        public User LoggedUser
        {
            get => _loggedUser;
            set => Set(ref _loggedUser, value);
        }
        public UserAuthenticator Session = UserAuthenticator.SessionUserAuthenticator;
        public ICommand CloseReviewWriter => new RelayCommand(() => ShowReviewEditor = Visibility.Collapsed);
        public ICommand OpenReviewWriter => new RelayCommand(() =>
        {
            ShowReviewEditor = Visibility.Visible;
            Page.BringViewToReviewEditBox();
        });
        public ICommand AddReviewCommand;

        private int _averageScore = -1;
        private Visibility _showReviewEditor = Visibility.Collapsed;
        public Visibility ShowReviewEditor
        {
            get => _showReviewEditor;
            set => Set(ref _showReviewEditor, value);
        }
        public int AverageScore {
            get => _averageScore;
            set => Set(ref _averageScore, value);
        }

        //TODO: Fix this focking shiet?!?!? Why the hell wont you call notifyChange when items are added :((((((((((((((((
        public ObservableCollection<Review> Reviews = new ObservableCollection<Review>();
        

        public void Initialize()
        {
            // Do no want to wait for it to fetch reviews.
#pragma warning disable 4014
            InitializeReviews(MainGame);
            InitializePlatforms(MainGame);
#pragma warning restore 4014
            MainGame.GameCover.url = "https://images.igdb.com/igdb/image/upload/t_720p/" + MainGame.GameCover.image_id + ".jpg";

            AddReviewCommand = new RelayCommand<Review>(AddReview);
        }

        public async Task InitializeReviews(GameRoot game)
        {
            var conn = new Games();
            var gameFromDb = await conn.GetGame(game);
            if (gameFromDb != null)
                foreach (var rating in gameFromDb.Reviews)
                {
                    var usersConn = new Users();
                    rating.User = await usersConn.GetUser(rating.UserId);
                    Reviews.Add(rating);
                }
            
            SetAverageScore();
        }

        public async Task InitializePlatforms(GameRoot game)
        {
            if (game.PlatformsIds.Length == 0) return;
            try
            {
                game.PlatformList = await new IgdbAccess().GetPlatformsAsync(game);
                foreach (var platform in game.PlatformList)
                {
                    Platforms.Add(platform);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public async void AddReview(Review review)
        {

            if (review.Stars == -1)
            {
                Page.RatingGridBorderColor(true);
                return;
            }
            Page.RatingGridBorderColor(false);
            var conn = new Games();
            try
            {
                var gameFromDb = await conn.GetGame(MainGame);
                if (gameFromDb == null)
                {
                    if (!await new Games().AddGame(MainGame))
                        return;
                }
            }
            catch (Exception ex)
            {
                //TODO: SOMETHING HAPPEND
            }
            try
            {
                try
                {
                    review.date = DateTime.UtcNow;
                    if (review.Id != 0) { 
                        if(!await new Reviews().UpdateReview(review))
                            return;
                    }
                    else { 
                        if (!await new Reviews().AddReview(review))
                            return;
                    }
                }
                catch (Exception ex)
                {
                    //TODO:Toast: You have internet?
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            ShowReviewEditor = Visibility.Collapsed;
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
            if (gameFromDb == null) return;
            foreach (var review in gameFromDb.Reviews)
            {
                var usersConn = new Users();
                review.User = await usersConn.GetUser(review.UserId);
                if (Reviews.All(x => x.Id != review.Id))
                    Reviews.Insert(0,review);
            }
        }

        private void SetAverageScore()
        {
            if (Reviews.Count == 0)
                return;
            var avg = 0.00;
            foreach (var review in Reviews)
            {
                avg += review.Stars;
            }
            AverageScore = Convert.ToInt32(avg / Reviews.Count);
        }

        public async void DeleteReview(Review review)
        {
            if (await new Reviews().DeleteReview(review.Id))
            {
                if (await UserAuthenticator.SessionUserAuthenticator.UpdateUser())
                    Reviews.Remove(review);
            }
            //TODO: NO INTERNET
        }
    }
}
