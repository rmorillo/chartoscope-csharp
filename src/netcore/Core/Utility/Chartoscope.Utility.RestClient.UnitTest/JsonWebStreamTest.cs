using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oanda.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chartoscope.Utility.RestClient.UnitTest
{
    [TestClass]
    public class JsonWebStreamTest
    {
        private const string StreamingRatesUri = "https://stream-fxpractice.oanda.com/v1/";
        private const string AccessToken = "bfb32b137f09f9b0c12d2ef1de14def3-4da55e9d959a5dfbf3933e1b3ba336c4";
        private const int AccountId = 6184634;

        [TestMethod]
        public async Task JsonWebStream_WorksAsync()
        {
            //Arrange
            var session = new JsonWebSession(StreamingRatesUri, AccessToken);
            var path = "prices?accountId=" + AccountId + "&instruments=" + Uri.EscapeDataString("EUR_USD,USD_CAD");
            var webStream = new JsonWebStream<RateStreamResponse>(path, session);
            var ticks = new List<Price>();
            webStream.DataReceived += new JsonWebStream<RateStreamResponse>.DataHandler((s) => ticks.Add(s.tick));
            //Act
            await webStream.BeginReceiveStreamAsync();
        }
    }
}
