using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        public async Task<T> SendRequestAsync<T>(HttpMethod httpMethod, string endpoint, StringContent content = null)
        {
            var request = new HttpRequestMessage(httpMethod, endpoint);
            if (content != null)
            {
                request.Content = content;
            }
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

        public async Task<T> SendRequestWithBodyAsync<T>(HttpMethod httpMethod, string endpoint, T body)
        {
            var jsonObject = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            return await SendRequestAsync<T>(httpMethod, endpoint, content);
        }
    }
}
