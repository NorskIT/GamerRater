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
    class UserGroups
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<UserGroup> GetUserGroup(int id)
        {
            using (_httpClient)
            {
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.UserGroups + id));
                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                    return null;
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<UserGroup>(jsonCourses);
                return userResult.Id != 0 ? userResult : null;
            }
        }
        
        //Get UserGroup by username
        public async Task<UserGroup> GetUserGroup(string username)
        {
            using (_httpClient)
            {
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.UserGroups + username));
                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                    return null;
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<UserGroup>(jsonCourses);
                return userResult.Id != 0 ? userResult : null;
            }
        }

        public async Task<HttpStatusCode> AddUserGroup(UserGroup userGroup)
        {
            using (_httpClient)
            {
                var payload = JsonConvert.SerializeObject(userGroup);
                HttpContent cont = new StringContent(payload, Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync(new Uri(BaseUri.UserGroups), cont);
                return result.StatusCode;
            }
        }
    }
}
