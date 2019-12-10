using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebClinicGUI.Services
{
    public interface INetworkClient
    {
        public static Uri ServerUrl = new Uri("http://webclinicapi/api/");
        public static string ApiName = "API";
        public Task<T> SendRequestAsync<T>(HttpMethod httpMethod, string endpoint, StringContent content = null);
        public Task<T> SendRequestWithBodyAsync<T>(HttpMethod httpMethod, string endpoint, T body);
    }
}
