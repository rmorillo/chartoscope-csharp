using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Chartoscope.Utility.RestClient
{
    public class JsonRestClient: IRestClient
    {
        private HttpClient _client;
        private string _uri;
        private string _accessToken;
        public JsonRestClient(string uri, string accessToken)
        {
            _uri = uri;
            _accessToken = accessToken;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_uri);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        public async Task<T> Get<T>(IGetRequest<T> method)
        {
            HttpResponseMessage response = await _client.GetAsync(_uri + method.Path);
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    return method.Deserialize(stream);
                }                 
            }
            else
            {
                return default(T);
            }
        }      
    }
}
