using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GamerRater.Application.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.Views
{
    public sealed partial class GameDetailsPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();

        //Change this to correct url.
        private readonly string uri = "http://localhost:61971";

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

        private async void AddGameButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var payload = JsonConvert.SerializeObject(ViewModel.mainGame);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(string.Concat(uri, "/api/GameRoots"), cont);
        }

        private async void AddReviewToGame_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Rating rating = new Rating()
                {date = DateTime.Now, Game = ViewModel.mainGame, Review = "Best game ever!", Stars = 5};
            var payload = JsonConvert.SerializeObject(rating);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(string.Concat(uri, "/api/Ratings"), cont);
        }
    }
}
