using Chartoscope.Core.Library.UnitTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class DualTimeframeSignalAdvisorTest
    {
        [TestMethod]
        public void DualTimeframeSignalAdvisor_GeneratesBuySignal()
        {
            //Arrange
            var forexPair = new TickerSymbol("EURUSD");
            var m1Timeframe = new TimeframeUnit(TimeframeOption.M1);
            var m5Timeframe = new TimeframeUnit(TimeframeOption.M5);

            var feedService = new PriceFeedService(new ManualPriceFeeder(new MockFeedProvider()));
            var signalTimeframe= feedService.Setup(forexPair, m1Timeframe, OHLCPriceOption.All);
            var trendTimeframe = feedService.Setup(forexPair, m5Timeframe, OHLCPriceOption.All);

            var signalAdvisor = new DualTimeframeSignalAdvisor(signalTimeframe, trendTimeframe);
            signalAdvisor.Subscribe(feedService);

            var position = PositionOption.None;

            signalAdvisor.Buy += delegate (int strength) { position = PositionOption.Buy; };
            signalAdvisor.Sell += delegate (int strength) { position = PositionOption.Sell; };
            signalAdvisor.Close += delegate () { position = PositionOption.None; };

            //Act
            feedService.OHLC.PriceAction(signalTimeframe, new OHLCBar(DateTime.Now.Ticks, 1, 2, 3, 11));

            //Assert
            Assert.AreEqual(position, PositionOption.Buy);
        }

        [TestMethod]
        public void DualTimeframeSignalAdvisor_GeneratesSellSignal()
        {
            //Arrange
            var forexPair = new TickerSymbol("EURUSD");
            var m1Timeframe = new TimeframeUnit(TimeframeOption.M1);
            var m5Timeframe = new TimeframeUnit(TimeframeOption.M5);

            var feedService = new PriceFeedService(new ManualPriceFeeder(new MockFeedProvider()));
            var signalTimeframe = feedService.Setup(forexPair, m1Timeframe, OHLCPriceOption.All);
            var trendTimeframe = feedService.Setup(forexPair, m5Timeframe, OHLCPriceOption.All);

            var signalAdvisor = new DualTimeframeSignalAdvisor(signalTimeframe, trendTimeframe);
            signalAdvisor.Subscribe(feedService);

            var position = PositionOption.None;

            signalAdvisor.Buy += delegate (int strength) { position = PositionOption.Buy; };
            signalAdvisor.Sell += delegate (int strength) { position = PositionOption.Sell; };
            signalAdvisor.Close += delegate () { position = PositionOption.None; };

            //Act
            feedService.OHLC.PriceAction(signalTimeframe, new OHLCBar(DateTime.Now.Ticks, 11, 2, 3, 1));

            //Assert
            Assert.AreEqual(position, PositionOption.Sell);

        }

        [TestMethod]
        public void DualTimeframeSignalAdvisor_GeneratesCloseSignal()
        {
            //Arrange
            var forexPair = new TickerSymbol("EURUSD");
            var m1Timeframe = new TimeframeUnit(TimeframeOption.M1);
            var m5Timeframe = new TimeframeUnit(TimeframeOption.M5);

            var feedService = new PriceFeedService(new ManualPriceFeeder(new MockFeedProvider()));
            var signalTimeframe = feedService.Setup(forexPair, m1Timeframe, OHLCPriceOption.All);
            var trendTimeframe = feedService.Setup(forexPair, m5Timeframe, OHLCPriceOption.All);

            var signalAdvisor = new DualTimeframeSignalAdvisor(signalTimeframe, trendTimeframe);
            signalAdvisor.Subscribe(feedService);

            var position = PositionOption.None;

            signalAdvisor.Buy += delegate (int strength) { position = PositionOption.Buy; };
            signalAdvisor.Sell += delegate (int strength) { position = PositionOption.Sell; };
            signalAdvisor.Close += delegate () { position = PositionOption.None; };

            //Act
            feedService.OHLC.PriceAction(signalTimeframe, new OHLCBar(DateTime.Now.Ticks, 11, 2, 3, 1));
            feedService.OHLC.PriceAction(signalTimeframe, new OHLCBar(DateTime.Now.Ticks, 1, 2, 3, 1));

            //Assert
            Assert.AreEqual(position, PositionOption.None);

        }
    }
}
