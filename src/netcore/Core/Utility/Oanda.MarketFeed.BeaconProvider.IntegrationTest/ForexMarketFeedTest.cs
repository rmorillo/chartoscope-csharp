using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chartoscope.Utility.RestClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chartoscope.Beacon.Common;

namespace Oanda.MarketFeed.BeaconProvider.IntegrationTest
{
    [TestClass]
    public class ForexMarketFeedTest
    {
        private const string Server = "https://api-fxpractice.oanda.com/v1/";
        private const string StreamingRatesUri = "https://stream-fxpractice.oanda.com/v1/";
        private const string AccessToken = "bfb32b137f09f9b0c12d2ef1de14def3-4da55e9d959a5dfbf3933e1b3ba336c4";
        private const int AccountId = 6184634;

        [TestMethod]
        public async Task ForexMarketFeed_WorksAsync()
        {
            //Arrange
            var restClient = new JsonRestClient(Server, AccessToken);

            var config = new Dictionary<string, string>
            {
                { "AccountId", AccountId.ToString() },
                { "StreamingRatesUri", StreamingRatesUri },
                { "AccessToken", AccessToken },
                { "SelectedInstruments", "EUR_USD,USD_JPY" }
            };
            var marketFeed = new ForexMarketFeed(restClient, config);
            var priceTick = new List<IPriceTick>();
            marketFeed.TickReceived += new DelegateDefinitions.TickDataHandler(s => priceTick.Add(s));

            marketFeed.Initialize();
            //Act
            marketFeed.Startup();

            await Task.Delay(20000);

            marketFeed.Shutdown();

            await Task.Delay(5000);

            //Assert
            Assert.IsTrue(priceTick.Count > 0);
        }
    }
}
