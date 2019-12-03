using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebClinicGUI.Services
{
    public class NetworkClient : INetworkClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public NetworkClient(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor)
        {
            _httpClient = clientFactory.CreateClient(INetworkClient.ApiName);
            _contextAccessor = contextAccessor;
        }

        public async Task<T> SendRequestAsync<T>(HttpMethod httpMethod, string endpoint, StringContent content = null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken());

            var request = new HttpRequestMessage(httpMethod, endpoint);
            if (content != null)
            {
                request.Content = content;
            }
            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            else
            {
                throw new HttpRequestException(responseString);
            }
        }

        public async Task<T> SendRequestWithBodyAsync<T>(HttpMethod httpMethod, string endpoint, T body)
        {
            var jsonObject = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            return await SendRequestAsync<T>(httpMethod, endpoint, content);
        }

        private string getToken()
        {
            return _contextAccessor.HttpContext.User.FindFirstValue("access_token");
        }
    }
}
