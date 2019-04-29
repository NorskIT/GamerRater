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

        public async Task GetCoversToGamesAsync(GameRoot[] games)
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
        }

        public async Task<GameRoot[]> GetGamesAsync(string gameNames)
        {
            using (_httpClient)
            {
                var results = await _httpClient.PostAsync(new Uri(Url + _urlGames), new HttpStringContent(
                    "fields *;" +
                    "where name = \"" + gameNames + "\"*;" +
                    "sort name asc;" +
                    "limit 50;",
                    UnicodeEncoding.Utf8,
                    "application/json"));
                var jsonGame = await results.Content.ReadAsStringAsync();
                var gamesArr = JsonConvert.DeserializeObject<GameRoot[]>(jsonGame);
                return gamesArr;
            }
        }

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
    }
}
