using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Model;
using Newtonsoft.Json;
using HttpClient = Windows.Web.Http.HttpClient;

namespace GamerRater.Application.DataAccess
{
    internal class IgdbAccess : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public IgdbAccess()
        {
            _httpClient.DefaultRequestHeaders.Add("user-key",
                "d1f0748dd028fe160ba161dfd05fe3b1"); //Free key, no worries
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        /// <summary>Gets the covers related to games asynchronous.</summary>
        /// <param name="games">The games.</param>
        /// <returns></returns>
        public async Task<GameRoot[]> GetCoversToGamesAsync(GameRoot[] games)
        {
            try
            {
                var results = await _httpClient.PostAsync(new Uri(BaseUriString.IGDBCovers), new HttpStringContent(
                    "fields *;" +
                    "where id  = (" + IdStringBuilder.GameIds(games) + ");",
                    UnicodeEncoding.Utf8,
                    "application/json"));
                var jsonGame = await results.Content.ReadAsStringAsync();
                var coversArr = JsonConvert.DeserializeObject<GameCover[]>(jsonGame);
                foreach (var cover in coversArr)
                {
                    cover.url = "https:" + cover.url;
                    foreach (var game in games)
                        if (cover.id == game.Cover)
                            game.GameCover = cover;
                }

                return games;
            }
            catch (HttpRequestException e)
            {
                GrToast.SmallToast(GrToast.Errors.IgdbError);
                await Log.WriteMessage(this + " ; " + e.Message + " : " + e.StackTrace).ConfigureAwait(true);
                return null;
            }
        }

        /// <summary>Gets the games asynchronous.</summary>
        /// <param name="gameNames">The game names.</param>
        /// <returns></returns>
        public async Task<GameRoot[]> GetGamesAsync(string gameNames)
        {
            try
            {
                var results = await _httpClient.PostAsync(new Uri(BaseUriString.IGDBGames), new HttpStringContent(
                    "fields *;" +
                    "search \"" + gameNames + "\"*;" +
                    "where cover != 0;" +
                    "limit 50;",
                    UnicodeEncoding.Utf8,
                    "application/json"));
                var jsonGame = await results.Content.ReadAsStringAsync();
                if (!results.IsSuccessStatusCode) return null;
                var gamesArr = JsonConvert.DeserializeObject<GameRoot[]>(jsonGame);
                return gamesArr;
            }
            catch (HttpRequestException e)
            {
                await Log.WriteMessage(this + " ; " + e.Message + " : " + e.StackTrace).ConfigureAwait(true);
                GrToast.SmallToast(GrToast.Errors.IgdbError);
                return null;
            }
        }

        /// <summary>Gets the platforms related to game asynchronous.</summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public async Task<Platform[]> GetPlatformsAsync(GameRoot game)
        {
            try
            {
                var results = await _httpClient.PostAsync(new Uri(BaseUriString.IGDBPlatforms), new HttpStringContent(
                    "fields *;" +
                    "where id = (" + IdStringBuilder.PlatformIds(game) + ");",
                    UnicodeEncoding.Utf8,
                    "application/json"));
                var jsonGame = await results.Content.ReadAsStringAsync();
                var platforms = JsonConvert.DeserializeObject<Platform[]>(jsonGame);
                return platforms;
            }
            catch (HttpRequestException e)
            {
                await Log.WriteMessage(this + " ; " + e.Message + " : " + e.StackTrace).ConfigureAwait(true);
                GrToast.SmallToast(GrToast.Errors.IgdbError);
                return null;
            }
        }
    }
}
