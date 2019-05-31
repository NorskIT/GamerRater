using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GamerRater.Application.Services;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    internal class Games : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public GameRoot ResultGame;

        public Games()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(4);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        /// <summary>Gets the GameRater version GameRoot</summary>
        /// <param name="game">  IGDB GameRoot</param>
        /// <returns>Original GameRoot</returns>
        public async Task<GameRoot> GetGame(GameRoot game)
        {
            try
            {
                try
                {
                    var httpResponse = await _httpClient.GetAsync(new Uri(BaseUriString.Games + game.Id))
                        .ConfigureAwait(true);
                    if (httpResponse.StatusCode != HttpStatusCode.OK) return null;
                    var jsonGame = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
                    ResultGame = JsonConvert.DeserializeObject<GameRoot>(jsonGame);
                    return ResultGame.Id != 0 ? ResultGame : null;
                }
                catch (TaskCanceledException)
                {
                    GrToast.SmallToast(GrToast.Errors.ApiError);
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                GrToast.SmallToast(GrToast.Errors.ApiError);
                return null;
            }
        }

        /// <summary>Gets the game by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<GameRoot> GetGameById(int id)
        {
            try
            {
                try
                {
                    var httpResponse =
                        await _httpClient.GetAsync(new Uri(BaseUriString.Games + id)).ConfigureAwait(true);
                    if (httpResponse.StatusCode != HttpStatusCode.OK) return null;
                    var jsonGame = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
                    ResultGame = JsonConvert.DeserializeObject<GameRoot>(jsonGame);
                    return ResultGame.Id != 0 ? ResultGame : null;
                }
                catch (TaskCanceledException)
                {
                    GrToast.SmallToast(GrToast.Errors.ApiError);
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                GrToast.SmallToast(GrToast.Errors.ApiError);
                return null;
            }
        }

        /// <summary>Adds the game.</summary>
        /// <param name="mainGame">The main game.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> AddGame(GameRoot mainGame)
        {
            try
            {
                try
                {
                    var payload = JsonConvert.SerializeObject(mainGame);
                    HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
                    var result = await _httpClient.PostAsync(new Uri(BaseUriString.Games), cont).ConfigureAwait(true);
                    return result;
                }
                catch (TaskCanceledException)
                {
                    return new HttpResponseMessage {StatusCode = HttpStatusCode.RequestTimeout};
                    ;
                }
            }
            catch (HttpRequestException)
            {
                GrToast.SmallToast(GrToast.Errors.ApiError);
                return null;
            }
        }
    }
}
