using System;
using System.Collections.Generic;
using System.Linq;
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
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<UserGroup>(jsonCourses);
                if (userResult.Id != 0)
                    return userResult;
                return null;
            }
        }
    }
}
