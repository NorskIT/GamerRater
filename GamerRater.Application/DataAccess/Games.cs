using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation.Diagnostics;
using GamerRater.Application.Services;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    internal class Games : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public GameRoot ResultGame;

        //TODO: JsonException local?

        public async Task<GameRoot> GetGame(GameRoot game)
        {
            var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.Games + game.Id)).ConfigureAwait(true);
            var jsonGame = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
            ResultGame = JsonConvert.DeserializeObject<GameRoot>(jsonGame);
            return ResultGame.Id != 0 ? ResultGame : null;
        }


        public async Task<HttpResponseMessage> AddGame(GameRoot mainGame)
        {
            var payload = JsonConvert.SerializeObject(mainGame);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(new Uri(BaseUri.Games), cont).ConfigureAwait(true);
            return result;
        }

        public async Task<GameRoot> GetGameById(int id)
        {
            var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.Games + id)).ConfigureAwait(true);
            var jsonGame = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
            ResultGame = JsonConvert.DeserializeObject<GameRoot>(jsonGame);
            return ResultGame.Id != 0 ? ResultGame : null;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
