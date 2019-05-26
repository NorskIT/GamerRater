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
    class Users
    {
        private readonly HttpClient _httpClient = new HttpClient();

        //Get user by ID
        public async Task<User> GetUser(int id)
        {
            using (_httpClient)
            {
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.Users + id));
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<User>(jsonCourses);
                return userResult.Id != 0 ? userResult : null;
            }
        }

        //Get user by username
        public async Task<User> GetUser(string username)
        {
            using (_httpClient)
            {
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.Username + username));
                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                    return null;
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<User>(jsonCourses);
                return userResult.Id != 0 ? userResult : null;
            }
        }

        public async Task<bool> AddUser(User user)
        {
            var payload = JsonConvert.SerializeObject(user);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(new Uri(BaseUri.Users), cont);
            return result.StatusCode == HttpStatusCode.Created;
        }
    }
}
