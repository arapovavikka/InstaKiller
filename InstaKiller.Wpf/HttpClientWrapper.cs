using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using InstaKiller.Model;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace InstaKiller.Wpf
{
    class HttpClientWrapper
    {
        private readonly string _connectionString;
        private readonly HttpClient _client;

        public HttpClientWrapper(string connectionString)
        {
            _connectionString = connectionString;
            _client = new HttpClient
            {
                BaseAddress = new Uri(connectionString)
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Person GetUserByEmail(string email)
        {
            var result = _client.GetAsync(string.Format("{0}api/v1/user/email/{1}", _connectionString, email)).Result;
            return result.Content.ReadAsAsync<Person>().Result;
        }
    }
}
