using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Model.IGDBModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Newtonsoft.Json;

namespace GamerRater.Application.ViewModels
{
    public class MainViewModel : Observable
    {
        public ObservableCollection<GameCover> Games = new ObservableCollection<GameCover>();
        public ICommand _itemClickCommand;
        private readonly IGDBAccess IGDBCovers = new IGDBAccess();

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Game>(OnItemClick));

        public MainViewModel()
        {
            
        }

        private void OnItemClick(Game clickedItem)
        {
            /*if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate<ContentGridGameDetailPage>(clickedItem);
            }*/
        }

        public async void LoadGamesAsync(object sender, RoutedEventArgs e)
        {
            var covers = await IGDBCovers.GetCoversAsync();
            foreach (var cover in covers) Games.Add(cover);
        }
    }

    internal class ContentGridGameDetailPage
    {
    }
}
