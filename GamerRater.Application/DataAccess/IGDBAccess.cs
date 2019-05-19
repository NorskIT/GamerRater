using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using GamerRater.Model.IGDBModels;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    internal class IGDBAccess : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string Url = "https://api-v3.igdb.com/";
        private readonly string _urlGames = "Games/";
        private readonly string _urlCovers = "Covers/";

        public IGDBAccess()
        {
            _httpClient.DefaultRequestHeaders.Add("user-key", "d1f0748dd028fe160ba161dfd05fe3b1");
        }

        public async Task<GameRoot[]> GetCoversToGamesAsync(GameRoot[] games)
        {
                var results = await _httpClient.PostAsync(new Uri(Url + _urlCovers), new HttpStringContent(
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
                        if (cover.id == game.cover)
                            game.GameCover = cover;
                }

                return games;
        }

        public async Task<GameRoot[]> GetGamesAsync(string gameNames)
        {
            var results = await _httpClient.PostAsync(new Uri(Url + _urlGames), new HttpStringContent(
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

        //Builds a string compatible with the api query. Etc : (123, 432, 12994, 392)
        private string BuildGameCoverIdString(GameRoot[] games)
        {
            string ids = "";
            bool firstIterate = true;
            foreach (GameRoot game in games)
            {
                if (firstIterate)
                    ids += game.cover;
                else
                    ids += ", " + game.cover;
                firstIterate = false;

            }

            return ids;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /*public async Task<GameRoot[]> GetHqCoverToGameAsync(GameRoot game)
        {
            var results = await _httpClient.PostAsync(new Uri(Url + _urlCovers), new HttpStringContent(
                "fields *;" +
                "where id  = (" + game.cover + ");",
                UnicodeEncoding.Utf8,
                "application/json"));
            var jsonGame = await results.Content.ReadAsStringAsync();
            var coversArr = JsonConvert.DeserializeObject<GameCover[]>(jsonGame);
            foreach (var cover in coversArr)
            {
                cover.url = "https:" + cover.url;
                foreach (GameRoot game in games)
                    if (cover.id == game.cover)
                        game.GameCover = cover;
            }

            return games;
        }*/
    }
}
