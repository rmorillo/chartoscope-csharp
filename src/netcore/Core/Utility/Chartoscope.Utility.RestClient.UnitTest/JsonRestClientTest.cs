using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oanda.Common.Data;

namespace Chartoscope.Utility.RestClient.UnitTest
{
    [TestClass]
    public class JsonRestClientTest
    {
        private const string Server = "https://api-fxpractice.oanda.com/v1/";
        private const string AccessToken = "bfb32b137f09f9b0c12d2ef1de14def3-4da55e9d959a5dfbf3933e1b3ba336c4";
        private const int AccountId = 6184634;

        [TestMethod]
        public async Task JsonRestClient_WorksAsync()
        {
            var client = new JsonRestClient(Server, AccessToken);            

            var request = new JsonGetRequest<Instruments>("instruments?accountId=" + 6184634 + "&instruments=" + Uri.EscapeDataString("EUR_USD,USD_CAD"));
             
            var result= await client.Get<Instruments>(request);
            
        }
    }
}
