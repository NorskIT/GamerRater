using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
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
        public ObservableCollection<GameRoot> Games { get; set; }
        public MainPage Page { get; set; }
        public ICommand ItemClickCommand =>
            _ItemClickCommand ?? (_ItemClickCommand = new RelayCommand<GameRoot>(OnItemClick));

        public MainViewModel()
        {
            Games = new ObservableCollection<GameRoot>();
        }
        /// <summary>Executes initializing of search based on Search bar query text </summary>
        /// <param name="sender">Search bar</param>
        /// <param name="args">The <see cref="SearchBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        public void SubmitSearch(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if(!NetworkInterface.GetIsNetworkAvailable())
            {
                GrToast.SmallToast(GrToast.Errors.NetworkError);
                return;
            }
            sender.IsFocusEngaged = false;
            InitializeGameSearchAsync(args.QueryText);
        }

        /// <summary>Called when Game has been clicked. Sends user to GameDetailsPage</summary>
        /// <param name="clickedItem">The clicked item.</param>
        private static void OnItemClick(GameRoot clickedItem)
        {
            if (clickedItem == null) return;
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
            NavigationService.Navigate<GameDetailsPage>(clickedItem);
        }

        /// <summary>  Initialize a search towards IGDB based on parameter</summary>
        /// <param name="gameName">Name of the game.</param>
        public async void InitializeGameSearchAsync(string gameName)
        {
            Page.WaitVisual(true);
            Games.Clear();
            var context = new IgdbAccess();
            using(context) {
                try
                {
                    var games = await context.GetGamesAsync(gameName).ConfigureAwait(true);
                    if (games.Length != 0)
                    {
                        await InitializeCoversToGameAsync(games, context).ConfigureAwait(true);
                        Page.WaitVisual(false);
                        PreviousGamesObservableCollection = Games;
                        return;
                    }
                }
                catch (HttpRequestException)
                {
                    GrToast.SmallToast(GrToast.Errors.NetworkError);
                    Page.WaitVisual(false);
                    return;
                }
            }
            Page.WaitVisual(false);
            GrToast.SmallToast("No game found");
            
        }

        /// <summary>Initializes process of fetching covers related to game asynchronous.</summary>
        /// <param name="games">The games.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private async Task InitializeCoversToGameAsync(GameRoot[] games, IgdbAccess context)
        {
           
            const int searchLimit = 5;
            var tempArr = new GameRoot[searchLimit];
            const int x = 0;
            if (games.Length > 4)
            {
                await BindCoversToGameLargeAsync(games, context, tempArr, searchLimit, x).ConfigureAwait(true);
            }
            else
            {
                await BindCoversToGameSmallAsync(games, context, x).ConfigureAwait(true);
                }
        }

        /// <summary>Binds the covers to game asynchronous.
        /// Used if less than 5 covers</summary>
        /// <param name="games">The games.</param>
        /// <param name="context">The context.</param>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private async Task BindCoversToGameSmallAsync(GameRoot[] games, IgdbAccess context, int x)
        {
            var toFindCoverList = new GameRoot[games.Length];
            foreach (var t in games)
            {
                toFindCoverList[x] = t;
                x++;
            }

            Page.WaitVisual(false);
            await context.GetCoversToGamesAsync(toFindCoverList).ConfigureAwait(true);
            foreach (var gamesRoot in toFindCoverList) Games.Add(gamesRoot);
        }

        /// <summary>Binds the covers to game asynchronous.
        /// Used if more than 4 covers</summary>
        /// <param name="games">The games.</param>
        /// <param name="context">The context.</param>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        private async Task BindCoversToGameLargeAsync(GameRoot[] games, IgdbAccess context, GameRoot[] toFindCoverList, int searchLimit, int x)
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

        /// <summary>Checks if there has been a previews search, if true then Games will be set to cached version</summary>
        public void CheckCache()
        {
            if (PreviousGamesObservableCollection.Count != 0)
            {
                Games = PreviousGamesObservableCollection;
            }
        }
    }
}
