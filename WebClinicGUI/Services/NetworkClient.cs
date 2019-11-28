using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebClinicGUI.Services
{
    public class NetworkClient : INetworkClient
    {
        private readonly HttpClient _httpClient;

        public NetworkClient(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient(INetworkClient.ApiName);
        }

        public async Task<T> SendRequestAsync<T>(HttpMethod httpMethod, string endpoint)
        {
            var request = new HttpRequestMessage(httpMethod, endpoint);
            var response = await _httpClient.SendAsync(request); var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            else
            {
                throw new HttpRequestException();
            }
        }
    }
}
