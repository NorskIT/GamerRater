using System;

using GamerRater.Application.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GamerRater.Model.IGDBModels;

namespace GamerRater.Application.Views
{
    public sealed partial class GameDetailsPage : Page
    {
        public GameDetailsViewModel ViewModel { get; } = new GameDetailsViewModel();

        public GameDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is GameRoot game)
            {
                game.GameCover.url = "https://images.igdb.com/igdb/image/upload/t_720p/" + game.GameCover.image_id + ".jpg";
                ViewModel.Initialize(game);
            }
        }
    }
}
