using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model.IGDBModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace GamerRater.Application.ViewModels
{
    public class MainViewModel : Observable
    {
        private readonly IGDBAccess IGDBCovers = new IGDBAccess();
        public ICommand _itemClickCommand;
        private AdaptiveGridView agv;
        public ObservableCollection<GameRoot> Games = new ObservableCollection<GameRoot>();

        public ICommand ItemClickCommand =>
            _itemClickCommand ?? (_itemClickCommand = new RelayCommand<GameRoot>(OnItemClick));

        public void UpdateTemplate(object sender, DataContextChangedEventArgs e)
        {
            agv = (AdaptiveGridView) sender;
        }

        public void SearchForGame(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var textFIeld = (TextBox)sender;
                string text = textFIeld.Text;
                GetGamesAsync(text);
            }
        }

        private void OnItemClick(GameRoot clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate<GameDetailsPage>(clickedItem);
            }
        }

        public async void GetGamesAsync(string gameName)
        {
            Games.Clear();
            var context = new IGDBAccess();
            var games = await context.GetGamesAsync(gameName);
            var toFindCoverList = new GameRoot[5];
            int x = 0;
            for (var index = 0; index < games.Length; index++)
            {
                if (toFindCoverList[4] != null)
                {
                    await context.GetCoversToGamesAsync(toFindCoverList);
                    foreach (GameRoot gamesRoot in toFindCoverList)
                    {
                        Games.Add(gamesRoot);
                    }
                    toFindCoverList = new GameRoot[5];
                    x = 0;
                }
                toFindCoverList[x] = games[index];
                x++;
            }
        }

        private async Task FindCoversToNewlyAddedGames()
        {
            using (var context = new IGDBAccess())
            {
                var toFindCoverList = new GameRoot[5];
                foreach (var t in Games)
                {
                    for (var j = 0; j < 5; j++) toFindCoverList[j] = t;

                    await context.GetCoversToGamesAsync(toFindCoverList);
                    toFindCoverList = new GameRoot[5];
                }

                foreach (var gameRoot in Games) Games.Add(gameRoot);
            }
        }
    }
    
}
