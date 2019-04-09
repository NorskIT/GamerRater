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
    internal class IGDBAccess
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string Url = "https://api-v3.igdb.com/";
        private readonly string _urlGames = "Games/";
        private readonly string _urlCovers = "Covers/";

        public IGDBAccess()
        {
            _httpClient.DefaultRequestHeaders.Add("user-key", "d1f0748dd028fe160ba161dfd05fe3b1");
        }

        public async Task<GameCover[]> GetCoversAsync()
        {
            using (_httpClient)
            {
                var results = await _httpClient.PostAsync(new Uri(Url + _urlCovers), new HttpStringContent(
                    "fields *;" +
                    "limit 10;",
                    UnicodeEncoding.Utf8,
                    "application/json"));
                var jsonGame = await results.Content.ReadAsStringAsync();
                var coversArr = JsonConvert.DeserializeObject<GameCover[]>(jsonGame);
                return coversArr;
            }
        }

        public async Task<GameRoot> GetGameAsync(int id)
        {
            var results = await _httpClient.PostAsync(new Uri(Url + _urlCovers), new HttpStringContent(
                "fields *;" +
                "limit "+ id + ";",
                UnicodeEncoding.Utf8,
                "application/json"));
            var jsonGame = await results.Content.ReadAsStringAsync();
            var coversArr = JsonConvert.DeserializeObject<GameRoot>(jsonGame);
            return coversArr;
        }
        
        
    }
}
