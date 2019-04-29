using System;

using GamerRater.Application.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GamerRater.Application.Views
{
    public sealed partial class GameViewGrid : Page
    {
        public GameViewGridModel ViewModel { get; } = new GameViewGridModel();

        public GameViewGrid()
        {
            InitializeComponent();
            ViewModel.LoadGamesAsync(5);
        }
    }
}
