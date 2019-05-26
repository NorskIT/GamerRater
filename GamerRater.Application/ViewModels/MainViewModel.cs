using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace GamerRater.Application.ViewModels
{
    public class MainViewModel : Observable
    {
        public ICommand _ItemClickCommand;
        public static ObservableCollection<GameRoot> lastGamesList = new ObservableCollection<GameRoot>();
        public ObservableCollection<GameRoot> Games = new ObservableCollection<GameRoot>();

        public ICommand ItemClickCommand =>
            _ItemClickCommand ?? (_ItemClickCommand = new RelayCommand<GameRoot>(OnItemClick));

        public void SubmitSearch(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            sender.IsFocusEngaged = false;
            GetGamesAsync(args.QueryText);
        }
        

        private static void OnItemClick(GameRoot clickedItem)
        {
            if (clickedItem == null) return;
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
            NavigationService.Navigate<GameDetailsPage>(clickedItem);
        }


        /**
            Searches for any games close to @param gameName via IGDB Api.
            We receive an array with games which we then need to locate their covers(Images).
            After covers are found, we bind them togheter and then show them in the list.
         */
        public async void GetGamesAsync(string gameName)
        {
            Games.Clear();
            var context = new IGDBAccess();
            var games = await context.GetGamesAsync(gameName);
            var toFindCoverList = new GameRoot[5];
            var x = 0;
            if (games.Length > 4)
            {
                foreach (var t in games)
                {
                    if (toFindCoverList[4] != null)
                    {
                        await context.GetCoversToGamesAsync(toFindCoverList);
                        foreach (var gamesRoot in toFindCoverList) Games.Add(gamesRoot);
                        toFindCoverList = new GameRoot[5];
                        x = 0;
                    }

                    toFindCoverList[x] = t;
                    x++;
                }
            }
            else
            {
                toFindCoverList = new GameRoot[games.Length];
                foreach (var t in games)
                {
                    toFindCoverList[x] = t;
                    x++;
                }

                await context.GetCoversToGamesAsync(toFindCoverList);
                foreach (var gamesRoot in toFindCoverList) Games.Add(gamesRoot);
            }

            lastGamesList = Games;
        }

        public void CheckCache()
        {
            if (lastGamesList.Count != 0)
            {
                Games = lastGamesList;
            }
        }
    }
}
