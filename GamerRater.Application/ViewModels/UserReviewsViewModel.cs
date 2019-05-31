using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace GamerRater.Application.ViewModels
{
    internal class UserReviewsViewModel : Observable
    {
        public UserReviews Page { get; set; }
        public ObservableCollection<GameRoot> Games = new ObservableCollection<GameRoot>();
        public ICommand _ItemClickCommand;
        public ICommand ItemClickCommand =>
            _ItemClickCommand ?? (_ItemClickCommand = new RelayCommand<GameRoot>(OnItemClick));

        /// <summary>Initializes this instance.</summary>
        public void Initialize()
        {
            if (UserAuthenticator.SessionUserAuthenticator.User == null) return;
            if(UserAuthenticator.SessionUserAuthenticator.User.Reviews.Count > 0)
                FetchGamesRelatedToUserReviews();

        }

        /// <summary>Called when game is clicked</summary>
        /// <param name="clickedItem">The clicked item.</param>
        private static void OnItemClick(GameRoot clickedItem)
        {
            if (clickedItem == null) return;
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
            NavigationService.Navigate<GameDetailsPage>(clickedItem);
        }

        /// <summary>Fetches the games related to user reviews.</summary>
        public async void FetchGamesRelatedToUserReviews()
        {
            foreach (var review in UserAuthenticator.SessionUserAuthenticator.User.Reviews)
            {
                var game = await new Games().GetGameById(review.GameRootId).ConfigureAwait(true);
                if (game == null)
                {
                    GrToast.SmallToast(GrToast.Errors.ApiError);
                    return;
                }
                if(Games.All(x => x.Id != game.Id))
                    Games.Add(game);
            }
        }
    }
}
