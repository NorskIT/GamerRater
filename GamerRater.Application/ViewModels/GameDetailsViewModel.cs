using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;

namespace GamerRater.Application.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameRoot MainGame;

        public ObservableCollection<Review> Reviews = new ObservableCollection<Review>();
        public UserAuthenticator Session = UserAuthenticator.SessionUserAuthenticator;

        public ICommand GoToLoginPage => new RelayCommand(() => NavigationService.Navigate<LoginPage>());
        public ICommand LogOutCommand => new RelayCommand(() => UserAuthenticator.SessionUserAuthenticator.LogOut());
        public ICommand CloseReviewWriter => new RelayCommand(() => ShowReviewEditor = false);
        public ICommand OpenReviewWriter => new RelayCommand(() => ShowReviewEditor = true);
        public bool ShowReviewEditor { get; set; }

        public void Initialize(GameRoot game)
        {
            // Do no want to wait for it to fetch reviews.
#pragma warning disable 4014
            InitializeReviews(game);
#pragma warning restore 4014
            game.GameCover.url = "https://images.igdb.com/igdb/image/upload/t_720p/" + game.GameCover.image_id + ".jpg";
            MainGame = game;
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
        }

        public async Task<bool> AddReview()
        {
            //TODO:Get logged in user. Get custom reviewText
            var newReview = new Review
            {
                date = DateTime.Now,
                GameRootId = MainGame.Id,
                ReviewText = "Best game ever!",
                Stars = 5,
                User = UserAuthenticator.LoggedInUser
            };

            //If review was successfully added to db, return true.
            if (!await new Reviews().AddReview(newReview)) return false;
            UpdateRatings();
            return true;
        }

        public async void UpdateRatings()
        {
            var conn = new Games();

            var gameFromDb = await conn.GetGame(MainGame);
            if (gameFromDb == null) return;
            foreach (var review in gameFromDb.Reviews)
            {
                var usersConn = new Users();
                review.User = await usersConn.GetUser(review.UserId);
                if (Reviews.All(x => x.Id != review.Id))
                    Reviews.Add(review);
            }
        }

        public async void AddGame(GameRoot viewModelMainGame)
        {
            if (await new Games().AddGame(MainGame))
                return;
            //TODO: HANDLE ERROR
        }
    }
}
