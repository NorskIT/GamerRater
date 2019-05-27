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
        
        public ObservableCollection<Platform> Platforms = new ObservableCollection<Platform>();

        public ObservableCollection<int> Stars = new ObservableCollection<int>();
        public User LoggedUser = UserAuthenticator.LoggedInUser;
        public UserAuthenticator Session = UserAuthenticator.SessionUserAuthenticator;
        private GameDetailsPage Page { get; set; }
        public ICommand CloseReviewWriter => new RelayCommand(() => ShowReviewEditor = Visibility.Collapsed);
        public ICommand OpenReviewWriter => new RelayCommand(() => ShowReviewEditor = Visibility.Visible);
        public ICommand AddReviewCommand;
        private int SelectedStar { get; set; }
        private int _averageScore;
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
        

        public void Initialize(GameRoot game, GameDetailsPage page)
        {
            this.Page = page;
            for(var x = 1; x<=5;x++)
                Stars.Add(x);
            // Do no want to wait for it to fetch reviews.
#pragma warning disable 4014
            InitializeReviews(game);
            InitializePlatforms(game);
#pragma warning restore 4014
            game.GameCover.url = "https://images.igdb.com/igdb/image/upload/t_720p/" + game.GameCover.image_id + ".jpg";
            MainGame = game;

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

            if (SelectedStar == 0)
            {
                Page.ComboBoxBorderColor(true);
                return;
            }
            Page.ComboBoxBorderColor(false);
            var conn = new Games();

            var gameFromDb = await conn.GetGame(MainGame);
            if (gameFromDb == null)
            {
                if (!await new Games().AddGame(MainGame))
                    return;
            }
            review.date = DateTime.UtcNow;
            review.GameRootId = MainGame.Id;
            review.UserId = LoggedUser.Id;
            review.Stars = SelectedStar;


            try
            {
                try
                {
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
            
            UpdateRatings(review);
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

        public void StarsUpdate(object sender, SelectionChangedEventArgs e)
        {
            var x = (ComboBox) sender;
            SelectedStar = int.Parse(x.SelectedItem?.ToString());
        }

        private void SetAverageScore()
        {
            if (Reviews.Count == 0)
                return;
            
            var avg = 0;
            foreach (var review in Reviews)
            {
                avg += review.Stars;
            }

            AverageScore = (avg / Reviews.Count);


        }
    }
}
