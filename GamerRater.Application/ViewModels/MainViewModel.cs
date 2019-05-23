using System;
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
using GamerRater.Model;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace GamerRater.Application.ViewModels
{
    public class MainViewModel : Observable
    {
        private readonly IGDBAccess _igdbCovers = new IGDBAccess();
        public ICommand _ItemClickCommand;
        public ICommand GoToLoginPageCommand;
        private AdaptiveGridView agv;
        public ObservableCollection<GameRoot> Games = new ObservableCollection<GameRoot>();

        public ICommand ItemClickCommand =>
            _ItemClickCommand ?? (_ItemClickCommand = new RelayCommand<GameRoot>(OnItemClick));

        public ICommand GoToLoginPage => new RelayCommand(() => NavigationService.Navigate<LoginPage>());

        public void UpdateTemplate(object sender, DataContextChangedEventArgs e)
        {
            agv = (AdaptiveGridView) sender;
        }

        public void SearchForGame(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter) return;
            var textField = (TextBox)sender;
            var text = textField.Text;
            GetGamesAsync(text);
        }
        
        private static void OnItemClick(GameRoot clickedItem)
        {
            if (clickedItem == null) return;
            NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
            NavigationService.Navigate<GameDetailsPage>(clickedItem);
        }

        public async void GetGamesAsync(string gameName)
        {
            Games.Clear();
            var context = new IGDBAccess();
            var games = await context.GetGamesAsync(gameName);
            var toFindCoverList = new GameRoot[5];
            var x = 0;
            if(games.Length > 4) { 
                foreach (var t in games)
                {
                    if (toFindCoverList[4] != null)
                    {
                        await context.GetCoversToGamesAsync(toFindCoverList);
                        foreach (var gamesRoot in toFindCoverList)
                        {
                            Games.Add(gamesRoot);
                        }
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
                foreach (var gamesRoot in toFindCoverList)
                {
                    Games.Add(gamesRoot);
                }
            }
        }
    }
    
}
