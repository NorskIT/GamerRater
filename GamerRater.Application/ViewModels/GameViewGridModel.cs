using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Model.IGDBModels;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace GamerRater.Application.ViewModels
{
    public class GameViewGridModel : Page
    {
        private readonly IGDBAccess IGDBCovers = new IGDBAccess();
        private readonly TimeSpan MinInterval = TimeSpan.FromSeconds(0.5);
        private readonly Stopwatch stopwatch = new Stopwatch(); // Stopped initially
        public ICommand _itemClickCommand;
        public ObservableCollection<GameRoot> Games = new ObservableCollection<GameRoot>();

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<GameRoot>(OnItemClick));

        private void OnItemClick(GameRoot clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate<GameViewGridDetailModel>(clickedItem);
            }
        }

        public async void LoadGamesAsync(int loads)
        {
            for (var i = 0; i < loads; i++)
                foreach (var game in await IGDBCovers.GetGamesAndCoversAsync())
                    if (!Games.Contains(game) && game.id != 0)
                        Games.Add(game);
        }

        public void OnScroll(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var viewer = (ScrollViewer) sender;
            if (Math.Abs(viewer.VerticalOffset - viewer.ScrollableHeight) < 15)
            {
                if (stopwatch.IsRunning && stopwatch.Elapsed < MinInterval)
                    return;

                LoadGamesAsync(1);
                stopwatch.Restart();
            }
        }
    }
}
