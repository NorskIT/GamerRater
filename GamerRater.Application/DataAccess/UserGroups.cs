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
    class UserGroups: IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        /// <summary>Gets the user group by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<UserGroup> GetUserGroup(int id)
        {
            var httpResponse = await _httpClient.GetAsync(new Uri(BaseUriString.UserGroups + id)).ConfigureAwait(true);
            if (httpResponse.StatusCode != HttpStatusCode.OK) return null;
            var jsonCourses = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
            var userResult = JsonConvert.DeserializeObject<UserGroup>(jsonCourses);
            return userResult.Id != 0 ? userResult : null;
        }


        /// <summary>Gets the user group by group name</summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        public async Task<UserGroup> GetUserGroup(string groupName)
        {
            var httpResponse = await _httpClient.GetAsync(new Uri(BaseUriString.UserGroupName + groupName)).ConfigureAwait(true);
            if (httpResponse.StatusCode != HttpStatusCode.OK) return null;
            var jsonCourses = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
            var userResult = JsonConvert.DeserializeObject<UserGroup>(jsonCourses);
            return userResult.Id != 0 ? userResult : null;
        }

        /// <summary>Adds the user group to database</summary>
        /// <param name="userGroup">The user group.</param>
        /// <returns></returns>
        public async Task<HttpStatusCode> AddUserGroup(UserGroup userGroup)
        {
            var payload = JsonConvert.SerializeObject(userGroup);
            HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(new Uri(BaseUriString.UserGroups), cont).ConfigureAwait(true);
            return result.StatusCode;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
