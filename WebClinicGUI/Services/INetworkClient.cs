using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebClinicGUI.Services
{
    public interface INetworkClient
    {
        public static Uri ServerUrl = new Uri("http://localhost:65145/api/");
        public static string ApiName = "API";
        public Task<T> SendRequestAsync<T>(HttpMethod httpMethod, string endpoint);
    }
}
