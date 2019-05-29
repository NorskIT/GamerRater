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
        public void Initialize()
        {
            if (UserAuthenticator.SessionUserAuthenticator.User != null)
                if(UserAuthenticator.SessionUserAuthenticator.User.Reviews.Count > 0)
                    FetchGames();
            
        }

        private static void OnItemClick(GameRoot clickedItem)
        {
            if (clickedItem == null) return;
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
            NavigationService.Navigate<GameDetailsPage>(clickedItem);
        }

        public async void FetchGames()
        {
            
            foreach (var review in UserAuthenticator.SessionUserAuthenticator.User.Reviews)
            {
                var game = await new Games().GetGameById(review.GameRootId);
                if(Games.All(x => x.Id != game.Id))
                    Games.Add(game);
            }
        }
    }
}
