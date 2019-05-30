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

        /// <summary>Initializes a new instance of the <see cref="Users"/> class.</summary>
        public Users()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(4);
        }


        /// <summary>Gets the user by identifier</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>User</returns>
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


        /// <summary>Gets the user username. Flimsy and very unsecure way of doing a login..</summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
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

        /// <summary>Adds the user to database.</summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<bool> AddUser(User user)
        {
            try
            {
                var payload = JsonConvert.SerializeObject(user);
                HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PostAsync(new Uri(BaseUriString.Users), cont).ConfigureAwait(true);
                return httpResponse.StatusCode == HttpStatusCode.Created;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }

        /// <summary>Validates user property values</summary>
        /// <param name="user">The user.</param>
        /// <returns>True if validation OK, else false</returns>
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
