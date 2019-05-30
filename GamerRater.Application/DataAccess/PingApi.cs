using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using GamerRater.Application.Services;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.DataAccess
{
    public class PingApi : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        //Primitive way of checking if we can reach the database
        public async Task<bool> CheckConnection()
        {

            using (_httpClient)
            {
                if (!NetworkInterface.GetIsNetworkAvailable()) return false;
                var httpResponse = await _httpClient.GetAsync(new Uri(BaseUriString.UserGroupName + "User")).ConfigureAwait(true);
                if (httpResponse.StatusCode != HttpStatusCode.OK) return false;
                var jsonCourses = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
                var userResult = JsonConvert.DeserializeObject<UserGroup>(jsonCourses);
                return (userResult != null);
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
