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

        public Users()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(4);
        }

        //Get user by ID
        public async Task<User> GetUser(int id)
        {
            try
            {
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUriString.Users + id)).ConfigureAwait(true);
                if (httpResponse.StatusCode != HttpStatusCode.OK) return null;
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
                var userResult = JsonConvert.DeserializeObject<User>(jsonCourses);
                return userResult.Id != 0 ? userResult : null;
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        //Get user by username
        public async Task<User> GetUser(string username)
        {
            try
            {
                var httpResponse =
                    await _httpClient.GetAsync(new Uri(BaseUriString.Username + username)).ConfigureAwait(true);
                if (httpResponse.StatusCode != HttpStatusCode.OK) return null;
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
                var userResult = JsonConvert.DeserializeObject<User>(jsonCourses);
                return userResult.Id != 0 ? userResult : null;
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        public async Task<bool> AddUser(User user)
        {
            var payload = JsonConvert.SerializeObject(user);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(new Uri(BaseUriString.Users), cont).ConfigureAwait(true);
            return httpResponse.StatusCode == HttpStatusCode.Created;
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
