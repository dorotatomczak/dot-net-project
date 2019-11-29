using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebClinicGUI.Services
{
    public interface INetworkClient
    {
        public static Uri ServerUrl = new Uri("http://localhost:51210/api/");
        public static string ApiName = "API";
        public Task<T> SendRequestAsync<T>(HttpMethod httpMethod, string endpoint, StringContent content = null);
        public Task<T> SendRequestWithBodyAsync<T>(HttpMethod httpMethod, string endpoint, T body);
    }
}
