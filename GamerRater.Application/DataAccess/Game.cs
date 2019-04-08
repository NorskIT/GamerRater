using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    internal class Game
    {
        private static readonly Uri ApiUri = new Uri("http://localhost:6289/");
        private readonly HttpClient _httpClient;

        public Game(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Game()
        {
        }

        public async Task<Model.Game[]> GetGamesAsync()
        {
            var resultGames = await _httpClient.GetAsync(new Uri(string.Concat(ApiUri, "/api/game")));
            var jsonGame = await resultGames.Content.ReadAsStringAsync();
            var gamesArr = JsonConvert.DeserializeObject<Model.Game[]>(jsonGame);
            return gamesArr;
        }

        public async Task<bool> DeleteGame(Model.Game game)
        {
            var result = await _httpClient.DeleteAsync(new Uri(string.Concat(ApiUri, "/api/game/", game.Id)));
            return result.IsSuccessStatusCode;
        }
    }
}
