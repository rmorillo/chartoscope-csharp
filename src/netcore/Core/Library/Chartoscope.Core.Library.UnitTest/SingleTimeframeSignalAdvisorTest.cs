using Chartoscope.Core.Library.UnitTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class SingleTimeframeSignalAdvisorTest
    {
        [TestMethod]
        public void SingleTimeframeSignalAdvisor_GeneratesBuySignal()
        {
            //Arrange
            var feeder = new ManualPriceFeeder(new MockFeedProvider());
            var forexPair = new TickerSymbol("EURUSD");
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = new PriceFeedService(feeder);
            var signalTimeframe = feedService.Setup(forexPair, timeFrame, OHLCPriceOption.All);

            var signalAdvisor = new SingleTimeframeSignalAdvisor(signalTimeframe);
            signalAdvisor.Subscribe(feedService);

            feedService.Start();

            var position = PositionOption.None;

            signalAdvisor.Buy += delegate (int strength) { position = PositionOption.Buy; };
            signalAdvisor.Sell += delegate (int strength) { position = PositionOption.Sell; };
            signalAdvisor.Close += delegate () { position = PositionOption.None; };

            //Act
            feeder.MinutePriceAction(signalTimeframe, new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 11));

            //Assert
            Assert.AreEqual(PositionOption.Buy, position);
            Assert.AreEqual(signalAdvisor.Position, PositionOption.Buy);
            Assert.AreEqual(100, signalAdvisor.PositionStrength);
        }

        [TestMethod]
        public void SingleTimeframeSignalAdvisor_GeneratesSellSignal()
        {
            //Arrange
            var forexPair = new TickerSymbol("EURUSD");
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);
            var feedService = new PriceFeedService(new ManualPriceFeeder(new MockFeedProvider()));
            var signalTimeframe = feedService.Setup(forexPair, timeFrame, OHLCPriceOption.All);

            var signalAdvisor = new SingleTimeframeSignalAdvisor(signalTimeframe);
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
        public void SingleTimeframeSignalAdvisor_GeneratesCloseSignal()
        {
            //Arrange
            var forexPair = new TickerSymbol("EURUSD");
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = new PriceFeedService(new ManualPriceFeeder(new MockFeedProvider()));
            var signalTimeframe = feedService.Setup(forexPair, timeFrame, OHLCPriceOption.All);

            var signalAdvisor = new SingleTimeframeSignalAdvisor(signalTimeframe);
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

        [TestMethod]
        public void SingleTimeframeSignalAdvisor_ClosesSellPositionOnReversal()
        {
            //Arrange
            var forexPair = new TickerSymbol("EURUSD");
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = new PriceFeedService(new ManualPriceFeeder(new MockFeedProvider()));
            var signalTimeframe = feedService.Setup(forexPair, timeFrame, OHLCPriceOption.All);

            var signalAdvisor = new SingleTimeframeSignalAdvisor(signalTimeframe);
            signalAdvisor.Subscribe(feedService);

            var position = PositionOption.None;

            var closed = false;

            signalAdvisor.Buy += delegate (int strength) { position = PositionOption.Buy; };
            signalAdvisor.Sell += delegate (int strength) { position = PositionOption.Sell; };
            signalAdvisor.Close += delegate () 
                {
                    position = PositionOption.None;
                    closed = true;
                };

            //Act
            feedService.OHLC.PriceAction(signalTimeframe, new OHLCBar(DateTime.Now.Ticks, 11, 2, 3, 1));
            feedService.OHLC.PriceAction(signalTimeframe, new OHLCBar(DateTime.Now.Ticks, 1, 2, 3, 11));

            //Assert
            Assert.AreEqual(position, PositionOption.Buy);
            Assert.IsTrue(closed);

        }

        [TestMethod]
        public void SingleTimeframeSignalAdvisor_ClosesBuyPositionOnReversal()
        {
            //Arrange
            var forexPair = new TickerSymbol("EURUSD");
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = new PriceFeedService(new ManualPriceFeeder(new MockFeedProvider()));
            var signalTimeframe = feedService.Setup(forexPair, timeFrame, OHLCPriceOption.All);

            var signalAdvisor = new SingleTimeframeSignalAdvisor(signalTimeframe);
            signalAdvisor.Subscribe(feedService);

            var position = PositionOption.None;

            var closed = false;

            signalAdvisor.Buy += delegate (int strength) { position = PositionOption.Buy; };
            signalAdvisor.Sell += delegate (int strength) { position = PositionOption.Sell; };
            signalAdvisor.Close += delegate ()
            {
                position = PositionOption.None;
                closed = true;
            };

            //Act
            feedService.OHLC.PriceAction(signalTimeframe, new OHLCBar(DateTime.Now.Ticks, 1, 2, 3, 11));
            feedService.OHLC.PriceAction(signalTimeframe, new OHLCBar(DateTime.Now.Ticks, 11, 2, 3, 1));            

            //Assert
            Assert.AreEqual(position, PositionOption.Sell);
            Assert.IsTrue(closed);

        }
    }
}
