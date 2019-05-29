using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
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
            Page.WaitVisual(false);
            Games.Clear();
            var context = new IgdbAccess();
            GameRoot[] games = null;
            using(context) { 
                try
                {
                    games = await context.GetGamesAsync(gameName);
                }
                catch (Exception ex)
                {
                    return;
                    //TODO: NO INTERNET. Could not retireve games.
                }
                try
                {
                    
                    const int searchLimit = 5;
                    var toFindCoverList = new GameRoot[searchLimit];
                    var x = 0;
                    if (games.Length > 4)
                    {
                        foreach (var t in games)
                        {
                            if (toFindCoverList[searchLimit - 1] != null)
                            {
                                await context.GetCoversToGamesAsync(toFindCoverList);
                                foreach (var gamesRoot in toFindCoverList) Games.Add(gamesRoot);
                                toFindCoverList = new GameRoot[searchLimit];
                                x = 0;
                                Page.WaitVisual(true);
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

                        Page.WaitVisual(true);
                        await context.GetCoversToGamesAsync(toFindCoverList);
                        foreach (var gamesRoot in toFindCoverList) Games.Add(gamesRoot);
                    }
                }
                catch (Exception ex)
                {
                    //TODO: Could not fetch covers. Check you internet
                    //Page.WaitVisual(true);
                    //foreach (var game in games) Games.Add(game);
                }
            }
            Page.WaitVisual(true);
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

        public void CheckCache()
        {
            if (PreviousGamesObservableCollection.Count != 0)
            {
                Games = PreviousGamesObservableCollection;
            }
        }
    }
}
