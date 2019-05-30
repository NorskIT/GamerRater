using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    internal class Reviews : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();


        public async Task<bool> AddReview(Review review)
        {
            var payload = JsonConvert.SerializeObject(review);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(new Uri(BaseUri.Reviews), cont).ConfigureAwait(true);
            return result.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> DeleteReview(int id)
        {
            var result =
                await _httpClient.DeleteAsync(new Uri(string.Concat(BaseUri.Reviews, id))).ConfigureAwait(true);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateReview(Review review)
        {
            var payload = JsonConvert.SerializeObject(review);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result =
                await _httpClient.PutAsync(new Uri(string.Concat(BaseUri.Reviews, review.Id)), cont).ConfigureAwait(true);
            return result.IsSuccessStatusCode;
        }


        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
