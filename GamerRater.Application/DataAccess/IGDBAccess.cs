using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    internal class IgdbAccess : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public IgdbAccess()
        {
            _httpClient.DefaultRequestHeaders.Add("user-key", "d1f0748dd028fe160ba161dfd05fe3b1");
        }

        public async Task<GameRoot[]> GetCoversToGamesAsync(GameRoot[] games)
        {
                var results = await _httpClient.PostAsync(new Uri(BaseUri.IGDBCovers), new HttpStringContent(
                    "fields *;" +
                    "where id  = (" + BuildGameCoverIdString(games) + ");",
                    UnicodeEncoding.Utf8,
                    "application/json"));
                var jsonGame = await results.Content.ReadAsStringAsync();
                var coversArr = JsonConvert.DeserializeObject<GameCover[]>(jsonGame);
                foreach (var cover in coversArr)
                {
                    cover.url = "https:" + cover.url;
                    foreach (GameRoot game in games)
                        if (cover.id == game.Cover)
                            game.GameCover = cover;
                }

                return games;
        }

        public async Task<GameRoot[]> GetGamesAsync(string gameNames)
        {
            var results = await _httpClient.PostAsync(new Uri(BaseUri.IGDBGames), new HttpStringContent(
                "fields *;" +
                "search \"" + gameNames + "\"*;" +
                "where cover != 0;" +
                "limit 50;" ,
                UnicodeEncoding.Utf8,
                "application/json"));
            var jsonGame = await results.Content.ReadAsStringAsync();
            var gamesArr = JsonConvert.DeserializeObject<GameRoot[]>(jsonGame);
            return gamesArr;
        }

        //Builds a Cover ID string compatible with the api query. Etc : (123, 432, 12994, 392)
        private static string BuildGameCoverIdString(GameRoot[] games)
        {
            var ids = "";
            var firstIterate = true;
            foreach (var game in games)
            {
                if (firstIterate)
                    ids += game.Cover;
                else
                    ids += ", " + game.Cover;
                firstIterate = false;

            }

            return ids;
        }
        //Builds a Platform ID string compatible with the api query. Etc : (123, 432, 12994, 392)
        private static string BuildGamePlatformIdString(GameRoot game)
        {
            var ids = "";
            var firstIterate = true;
            foreach (var id in game.PlatformsIds)
            {
                if (firstIterate)
                    ids += id;
                else
                    ids += ", " + id;
                firstIterate = false;

            }
            return ids;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<Platform[]> GetPlatformsAsync(GameRoot game)
        {
            var results = await _httpClient.PostAsync(new Uri(BaseUri.IGDBPlatforms), new HttpStringContent(
                "fields *;" +
                "where id = (" + BuildGamePlatformIdString(game) + ");",
                UnicodeEncoding.Utf8,
                "application/json"));
            var jsonGame = await results.Content.ReadAsStringAsync();
            var platforms = JsonConvert.DeserializeObject<Platform[]>(jsonGame);
            return platforms;
        }
    }
}
