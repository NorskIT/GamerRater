using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
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
        public static ObservableCollection<GameRoot> PreviousGamesObservableCollection = new ObservableCollection<GameRoot>();
        public ObservableCollection<GameRoot> Games = new ObservableCollection<GameRoot>();
        public MainPage Page;

        public ICommand ItemClickCommand =>
            _ItemClickCommand ?? (_ItemClickCommand = new RelayCommand<GameRoot>(OnItemClick));

        //TODO: COmmand?
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
        //TODO: IntializeGameSearch?
        public async void GetGamesAsync(string gameName)
        {
            Page.WaitVisual(true);
            Games.Clear();
            var context = new IgdbAccess();
            using(context) {
                GameRoot[] games;
                try
                {
                    games = await context.GetGamesAsync(gameName).ConfigureAwait(true);
                }
                catch (HttpRequestException)
                {
                    GrToast.SmallToast("Connection to database failed.. Please check your network connection and try again.");
                    Page.WaitVisual(false);
                    return;
                }
                //TODO: NAMES
                await NewMethod(games, context).ConfigureAwait(true);
            }
            Page.WaitVisual(false);
            if(Games.Count == 0) { 
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    //TODO:NO INTERNET
                }
                //TODO:NO RESULT
            }
            //foreach (var game in games) Games.Add(game); //DEBUG REMOVE!!!
            PreviousGamesObservableCollection = Games;
        }
        //TODO: NAMES!!
        private async Task NewMethod(GameRoot[] games, IgdbAccess context)
        {
            try
            {
                const int searchLimit = 5;
                var asd = new GameRoot[searchLimit];
                var x = 0;
                if (games.Length > 4)
                {
                    await NewMethod1(games, context, asd, searchLimit, x);
                }
                else
                {
                    await NewMethod2(games, context, x);
                }
            }
            catch (HttpRequestException)
            {
                //TODO: Could not fetch covers. Check you internet
                //Page.WaitVisual(true);
                //foreach (var game in games) Games.Add(game);
            }
        }

        private async Task NewMethod2(GameRoot[] games, IgdbAccess context, int x)
        {
            GameRoot[] toFindCoverList;
            toFindCoverList = new GameRoot[games.Length];
            foreach (var t in games)
            {
                toFindCoverList[x] = t;
                x++;
            }

            Page.WaitVisual(false);
            await context.GetCoversToGamesAsync(toFindCoverList);
            foreach (var gamesRoot in toFindCoverList) Games.Add(gamesRoot);
        }

        private async Task NewMethod1(GameRoot[] games, IgdbAccess context, GameRoot[] toFindCoverList, int searchLimit, int x)
        {
            foreach (var t in games)
            {
                if (toFindCoverList[searchLimit - 1] != null)
                {
                    await context.GetCoversToGamesAsync(toFindCoverList).ConfigureAwait(true);
                    foreach (var gamesRoot in toFindCoverList) Games.Add(gamesRoot);
                    toFindCoverList = new GameRoot[searchLimit];
                    x = 0;
                    Page.WaitVisual(false);
                }

                toFindCoverList[x] = t;
                x++;
            }
        }

        public void CheckCache()
        {
            if (PreviousGamesObservableCollection.Count != 0)
            {
                Games = PreviousGamesObservableCollection;
            }
        }
    }
}
