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
    class Users
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<User> GetUser(int id)
        {
            using (_httpClient)
            {
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.Users + id));
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<User>(jsonCourses);
                if (userResult.Id != 0)
                    return userResult;
                return null;
            }

        }
    }
}
