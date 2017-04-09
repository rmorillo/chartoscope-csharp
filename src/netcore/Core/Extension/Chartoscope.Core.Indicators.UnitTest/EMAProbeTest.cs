using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators.UnitTest
{
    [TestClass]
    public class EMAProbeTest
    {
        [TestMethod]
        public void EMAProbe_Constructor_DoesNotRaiseException()
        {
            new EMAProbe(null, 3);
        }

        [TestMethod]
        public void EMAProbe_ReactsToPriceFeed()
        {
            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, OHLCPriceOption.All);

            var probe= new EMAProbe(ticker,3);
            probe.Subscribe(feedService);

            var priceSwing = MockDataFactory.ExtractOHLCPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);

            //Act
            priceSwing.ForEach((s) => feedService.OHLC.PriceAction(ticker, s));

            //Assert
            Assert.IsTrue(probe.Length > 0);            
        }
    }
}
