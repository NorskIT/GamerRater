using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    class Games
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<GameRoot> GetGame(GameRoot game)
        {
            using (_httpClient)
            {
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.Games + game.Id));
                var jsonGame = await httpResponse.Content.ReadAsStringAsync();
                var resultGame = JsonConvert.DeserializeObject<GameRoot>(jsonGame);
                return resultGame.Id != 0 ? resultGame : null;
            }
            
        }

        public async Task<bool> AddGame(GameRoot mainGame)
        {
            var payload = JsonConvert.SerializeObject(mainGame);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(new Uri(BaseUri.Games), cont);
            return result.StatusCode == HttpStatusCode.Created;
        }
    }
}
