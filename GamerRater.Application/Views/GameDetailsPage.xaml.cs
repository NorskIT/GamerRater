using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GamerRater.Application.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GamerRater.Application.DataAccess;
using GamerRater.Model;
using Newtonsoft.Json;

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
                ViewModel.Initialize(game);
            }
        }

        private void AddGameButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.AddGame(ViewModel.MainGame);
        }

        private async void AddReviewToGame_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (await ViewModel.AddReview())
            {
                //TODO:Tell user review was added.
                return;
            }
            //TODO:Tell user it failed.
        }
    }
}
