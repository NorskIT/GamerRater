using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    public class Game
    {
        private static readonly string ApiUri = "http://localhost:61971";
        private readonly HttpClient _httpClient;

        public Game()
        {
            _httpClient = new HttpClient();
        }
        
        public async Task<Model.Game[]> GetGamesAsync()
        {
            var resultGames = await _httpClient.GetAsync(new Uri(string.Concat(ApiUri, "/api/games")));
            var jsonGame = await resultGames.Content.ReadAsStringAsync();
            var gamesArr = JsonConvert.DeserializeObject<Model.Game[]>(jsonGame);
            return gamesArr;
        }

        public async Task<bool> DeleteGame(Model.Game game)
        {
            var result = await _httpClient.DeleteAsync(new Uri(string.Concat(ApiUri, "/api/games/", game.Id)));
            return result.IsSuccessStatusCode;
        }
    }
}
