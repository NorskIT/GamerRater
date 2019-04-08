using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Newtonsoft.Json;

namespace GamerRater.Application.ViewModels
{
    public class MainViewModel : Observable
    {
        public ObservableCollection<Model.Game> Games = new ObservableCollection<Model.Game>();
        public ICommand _itemClickCommand;
        private readonly Game _gameDataAccess = new Game();

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Game>(OnItemClick));

        public MainViewModel()
        {

        }

        private void OnItemClick(Game clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate<ContentGridGameDetailPage>(clickedItem);
            }
        }

        public async void LoadGamesAsync(object sender, RoutedEventArgs e)
        {
            var games = await _gameDataAccess.GetGamesAsync();
            foreach (var game in games) Games.Add(game);
        }
    }

    internal class ContentGridGameDetailPage
    {
    }
}
