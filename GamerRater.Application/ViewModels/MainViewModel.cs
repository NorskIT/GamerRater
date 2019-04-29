using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Model.IGDBModels;

namespace GamerRater.Application.ViewModels
{
    public class MainViewModel : Observable
    {
        private readonly IGDBAccess IGDBCovers = new IGDBAccess();
        public ICommand _itemClickCommand;
        public ICommand _searchButtonCommand;
        public ObservableCollection<GameRoot> Games = new ObservableCollection<GameRoot>();

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Game>(OnItemClick));

        public void SearchForGame(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            GetGamesAsync(button.DataContext.ToString());
        }

        private void OnItemClick(Game clickedItem)
        {
            /*if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate<ContentGridGameDetailPage>(clickedItem);
            }*/
        }

        public async void GetGamesAsync(string gameName)
        {
            Games.Clear();
            IGDBAccess context = new IGDBAccess();
            foreach (GameRoot game in await context.GetGamesAsync(gameName))
            {
                Games.Add(game);
            }

            FindCoversToNewlyAddedGames();

        }

        private async Task FindCoversToNewlyAddedGames()
        {
            using(IGDBAccess context = new IGDBAccess()) {
                var toFindCoverList = new GameRoot[5];
                foreach (var t in Games)
                {
                    for (var j = 0; j < 5; j++) toFindCoverList[j] = t;

                    await context.GetCoversToGamesAsync(toFindCoverList);
                    toFindCoverList = new GameRoot[5];
                }
            }
        }
    }

    internal class ContentGridGameDetailPage
    {
    }
}
