using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Chartoscope.Utility.RestClient
{
    public class JsonWebSession
    {
        private HttpClient _client;
        private string _uri;
        private string _accessToken;
        public JsonWebSession(string uri, string accessToken)
        {
            _uri = uri;
            _accessToken = accessToken;
            _client = new HttpClient()
            {
                BaseAddress = new Uri(_uri)
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        public async Task<WebResponse> GetWebResponseAsync(string path)
        {
            WebResponse response = null;

            HttpWebRequest request = WebRequest.CreateHttp(_uri + path);
            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + _accessToken;

            try
            {                
                response = await request.GetResponseAsync();
                return response;
            }
            catch(Exception)
            {

            }

            return response;
        }
    }    
}
