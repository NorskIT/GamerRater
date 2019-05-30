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

        public async Task<bool> CheckConnection()
        {
            try
            {
                try
                {
                    using (_httpClient)
                    {
                        var httpResponse = await _httpClient.GetAsync(new Uri(BaseUri.UserGroupName + "User"))
                            .ConfigureAwait(true);
                        //TODO: More specific
                        if (httpResponse.StatusCode == HttpStatusCode.RequestTimeout)
                            throw new HttpRequestException();
                        var jsonCourses = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(true);
                        var userResult = JsonConvert.DeserializeObject<UserGroup>(jsonCourses);
                        return (userResult != null);
                    }
                }
                catch (HttpRequestException)
                {

                    return false;
                }
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
