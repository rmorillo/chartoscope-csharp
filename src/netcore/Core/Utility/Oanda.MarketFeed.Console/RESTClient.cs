using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Oanda.MarketFeed.Console
{

    public class Rest
    {
        //private const string AccessToken = "73eba38ad5b44778f9a0c0fec1a66ed1-44f47f052c897b3e1e7f24196bbc071f";
        //private const string Server = "https://api-fxpractice.oanda.com/v1/";

        private const string AccessToken = "bfb32b137f09f9b0c12d2ef1de14def3-4da55e9d959a5dfbf3933e1b3ba336c4";
        private const string Server = "https://api-fxpractice.oanda.com/v1/";
        private const string StreamingRates = "https://stream-fxpractice.oanda.com/v1/";

        /// <summary>
        /// Primary (internal) request handler
        /// </summary>
        /// <typeparam name="T">The response type</typeparam>
        /// <param name="requestString">the request to make</param>
        /// <param name="method">method for the request (defaults to GET)</param>
        /// <param name="requestParams">optional parameters (note that if provided, it's assumed the requestString doesn't contain any)</param>
        /// <returns>response via type T</returns>
        private static async Task<T> MakeRequestAsync<T>(string requestString, string method = "GET", Dictionary<string, string> requestParams = null)
        {
            if (requestParams != null && requestParams.Count > 0)
            {
                var parameters = CreateParamString(requestParams);
                requestString = requestString + "?" + parameters;
            }
            HttpWebRequest request = WebRequest.CreateHttp(requestString);
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + AccessToken;
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            request.Method = method;

            try
            {
                using (WebResponse response = await request.GetResponseAsync())
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    var stream = GetResponseStream(response);
                    return (T)serializer.ReadObject(stream);
                }
            }
            catch (WebException ex)
            {
                var stream = GetResponseStream(ex.Response);
                var reader = new StreamReader(stream);
                var result = reader.ReadToEnd();
                throw new Exception(result);
            }
        }

        private static Stream GetResponseStream(WebResponse response)
        {
            var stream = response.GetResponseStream();
            if (response.Headers["Content-Encoding"] == "gzip")
            {   // if we received a gzipped response, handle that
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }
            return stream;
        }

        private static string CreateParamString(Dictionary<string, string> requestParams)
        {
            string requestBody = "";
            foreach (var pair in requestParams)
            {
                requestBody += WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value) + "&";
            }
            requestBody = requestBody.Trim('&');
            return requestBody;
        }

        /// <summary>
		/// Retrieves the list of instruments available for the given account
		/// </summary>
		/// <param name="account">the account to check</param>
		/// <param name="fields">optional - the fields to request in the response</param>
		/// <param name="instrumentNames">optional - the instruments to request details for</param>
		/// <returns>List of Instrument objects with details about each instrument</returns>
		public static async Task<List<Instrument>> GetInstrumentsAsync(int account, List<string> fields = null, List<string> instrumentNames = null)
        {
            string requestString = Server + "instruments?accountId=" + account;

            // TODO: make sure this works
            if (fields != null)
            {
                string fieldsParam = GetCommaSeparatedList(fields);
                requestString += "&fields=" + Uri.EscapeDataString(fieldsParam);
            }
            if (instrumentNames != null)
            {
                string instrumentsParam = GetCommaSeparatedList(instrumentNames);
                requestString += "&instruments=" + Uri.EscapeDataString(instrumentsParam);
            }

            InstrumentsResponse instrumentResponse = await MakeRequestAsync<InstrumentsResponse>(requestString);

            List<Instrument> instruments = new List<Instrument>();
            instruments.AddRange(instrumentResponse.instruments);

            return instruments;
        }

        private static string GetCommaSeparatedList(List<string> items)
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in items)
            {
                result.Append(item + ",");
            }
            return result.ToString().Trim(',');
        }

        public static async Task<List<Price>> GetRatesAsync(List<Instrument> instruments, string since = null)
        {
            StringBuilder requestBuilder = new StringBuilder(Server + "prices?instruments=");

            foreach (var instrument in instruments)
            {
                requestBuilder.Append(instrument.instrument + ",");
            }
            string requestString = requestBuilder.ToString().Trim(',');
            requestString = requestString.Replace(",", "%2C");

            // TODO: make sure this works
            if (!string.IsNullOrEmpty(since))
            {
                requestString += "&since=" + since;
            }

            PricesResponse pricesResponse = await MakeRequestAsync<PricesResponse>(requestString);
            List<Price> prices = new List<Price>();
            prices.AddRange(pricesResponse.prices);

            return prices;
        }

        public static async Task<List<Position>> GetPositionsAsync(int accountId)
        {
            string requestString = Server + "accounts/" + accountId + "/positions";

            var positionResponse = await MakeRequestAsync<PositionsResponse>(requestString);
            var positions = new List<Position>();
            positions.AddRange(positionResponse.positions);

            return positions;
        }

        public static async Task<PostOrderResponse> PostOrderAsync(int account, Dictionary<string, string> requestParams)
        {
            string requestString = Server + "accounts/" + account + "/orders";
            return await MakeRequestWithBody<PostOrderResponse>("POST", requestParams, requestString);
        }

        public static async Task<DeletePositionResponse> DeletePositionAsync(int accountId, string instrument)
        {
            string requestString = Server + "accounts/" + accountId + "/positions/" + instrument;

            return await MakeRequestAsync<DeletePositionResponse>(requestString, "DELETE");
        }

        private static async Task<T> MakeRequestWithBody<T>(string method, Dictionary<string, string> requestParams, string requestString)
        {
            // Create the body
            var requestBody = CreateParamString(requestParams);
            HttpWebRequest request = WebRequest.CreateHttp(requestString);
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + AccessToken;
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var writer = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                // Write the body
                await writer.WriteAsync(requestBody);
            }

            // Handle the response
            try
            {
                using (WebResponse response = await request.GetResponseAsync())
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)serializer.ReadObject(response.GetResponseStream());
                }
            }
            catch (WebException ex)
            {
                var response = (HttpWebResponse)ex.Response;
                var stream = new StreamReader(response.GetResponseStream());
                var result = stream.ReadToEnd();
                throw new Exception(result);
            }
        }

        public static async Task<Transaction> GetTransactionDetailsAsync(int accountId, long transId)
        {
            string requestString = Server + "accounts/" + accountId + "/transactions/" + transId;

            var transaction = await MakeRequestAsync<Transaction>(requestString);

            return transaction;
        }

        public static async Task<WebResponse> StartRatesSession(List<Instrument> instruments, int accountId)
        {
            string instrumentList = "";
            foreach (var instrument in instruments)
            {
                instrumentList += instrument.instrument + ",";
            }
            // Remove the extra ,
            instrumentList = instrumentList.TrimEnd(',');
            instrumentList = Uri.EscapeDataString(instrumentList);

            string requestString = StreamingRates + "prices?accountId=" + accountId + "&instruments=" + instrumentList;

            HttpWebRequest request = WebRequest.CreateHttp(requestString);
            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + AccessToken;

            try
            {
                WebResponse response = await request.GetResponseAsync();
                return response;
            }
            catch (WebException ex)
            {
                var response = (HttpWebResponse)ex.Response;
                var stream = new StreamReader(response.GetResponseStream());
                var result = stream.ReadToEnd();
                throw new Exception(result);
            }
        }

        public static async Task<WebResponse> StartEventsSession(List<int> accountId = null)
        {
            string requestString = StreamingRates + "events";
            if (accountId != null && accountId.Count > 0)
            {
                string accountIds = "";
                foreach (var account in accountId)
                {
                    accountIds += account + ",";
                }
                accountIds = accountIds.Trim(',');
                requestString += "?accountIds=" + WebUtility.UrlEncode(accountIds);
            }

            HttpWebRequest request = WebRequest.CreateHttp(requestString);
            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + AccessToken;

            try
            {
                WebResponse response = await request.GetResponseAsync();
                return response;
            }
            catch (WebException ex)
            {
                var response = (HttpWebResponse)ex.Response;
                var stream = new StreamReader(response.GetResponseStream());
                var result = stream.ReadToEnd();
                throw new Exception(result);
            }
        }
    }
}
