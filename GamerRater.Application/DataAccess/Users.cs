using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Media.Core;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    internal class Users : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        //Get user by ID
        public async Task<User> GetUser(int id)
        {
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.Users + id)).ConfigureAwait(true);
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<User>(jsonCourses);
                return userResult.Id != 0 ? userResult : null;
        }

        //Get user by username
        public async Task<User> GetUser(string username)
        {
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.Username + username)).ConfigureAwait(true);
                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                    return null;
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<User>(jsonCourses);
                return userResult.Id != 0 ? userResult : null;
        }

        public async Task<bool> AddUser(User user)
        {
            var payload = JsonConvert.SerializeObject(user);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(new Uri(BaseUri.Users), cont).ConfigureAwait(true);
            return result.StatusCode == HttpStatusCode.Created;
        }

        public static bool UserDataValidator(User user)
        {
            var reg = new Regex("^[a-zA-Z0-9]*$");

            if (!reg.IsMatch(user.FirstName))
                    return false;
            if (!reg.IsMatch(user.LastName))
                    return false;
            if (!reg.IsMatch(user.Email))
                    return false;
            if (!reg.IsMatch(user.Username))
                    return false;
            if (!reg.IsMatch(user.Password))
                    return false;
            return true;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
