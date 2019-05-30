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

        public async Task<UserGroup> GetUserGroup(int id)
        {
            var httpResponse = await _httpClient.GetAsync(new Uri(BaseUriString.UserGroups + id)).ConfigureAwait(true);
            if (httpResponse.StatusCode != HttpStatusCode.OK) return null;
            var jsonCourses = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
            var userResult = JsonConvert.DeserializeObject<UserGroup>(jsonCourses);
            return userResult.Id != 0 ? userResult : null;
        }
        
        //Get UserGroup by username
        public async Task<UserGroup> GetUserGroup(string groupName)
        {
            var httpResponse = await _httpClient.GetAsync(new Uri(BaseUriString.UserGroupName + groupName)).ConfigureAwait(true);
            if (httpResponse.StatusCode != HttpStatusCode.OK) return null;
            var jsonCourses = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
            var userResult = JsonConvert.DeserializeObject<UserGroup>(jsonCourses);
            return userResult.Id != 0 ? userResult : null;
        }

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
