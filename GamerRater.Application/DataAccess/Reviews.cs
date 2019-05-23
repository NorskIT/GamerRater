using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    class Reviews
    {
        private readonly HttpClient _httpClient = new HttpClient();
        
        
        public async Task<bool> AddReview(Review review)
        {
            var payload = JsonConvert.SerializeObject(review);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(new Uri(BaseUri.Ratings), cont);
            return result.StatusCode == HttpStatusCode.Created;
        }
    }
}
