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


        public void Dispose()
        {
            _httpClient?.Dispose();
        }


        /// <summary>Adds the review to database</summary>
        /// <param name="review">The review.</param>
        /// <returns></returns>
        public async Task<bool> AddReview(Review review)
        {
            try
            {
                var payload = JsonConvert.SerializeObject(review);
                HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync(new Uri(BaseUriString.Reviews), cont).ConfigureAwait(true);
                return result.StatusCode == HttpStatusCode.Created;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        /// <summary>Deletes the review from database</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteReview(int id)
        {
            try
            {
                try
                {
                    _httpClient.Timeout = TimeSpan.FromSeconds(4);
                    var result =
                        await _httpClient.DeleteAsync(new Uri(string.Concat(BaseUriString.Reviews, id)))
                            .ConfigureAwait(true);
                    return result.IsSuccessStatusCode;
                }
                catch (HttpRequestException)
                {
                    return false;
                }
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }

        /// <summary>Updates the review.</summary>
        /// <param name="review">The review.</param>
        /// <returns></returns>
        public async Task<bool> UpdateReview(Review review)
        {
            try { 
            var payload = JsonConvert.SerializeObject(review);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result =
                await _httpClient.PutAsync(new Uri(string.Concat(BaseUriString.Reviews, review.Id)), cont)
                    .ConfigureAwait(true);
            return result.IsSuccessStatusCode;

            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
