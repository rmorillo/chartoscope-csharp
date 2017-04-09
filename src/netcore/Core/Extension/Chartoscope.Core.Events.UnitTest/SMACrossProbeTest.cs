using Chartoscope.Core.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Events.UnitTest
{
    [TestClass]
    public class SMACrossProbeTest
    {
        [TestMethod]
        public void SMACrossProbe_OHLCFeedTriggersAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, OHLCPriceOption.All);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 2, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            var priceSwing = MockDataFactory.ExtractOHLCPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);
            
            //Act
            priceSwing.ForEach((s) => feedService.OHLC.PriceAction(ticker, s));

            //Assert
            Assert.IsTrue(isTriggered);
            Assert.IsTrue(smaCross.Current.Status==EventStateOption.On);
        }

        [TestMethod]
        public void SMACrossProbe_PriceBarItemFeedTriggersAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, PriceBarOption.All);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 2, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            feedService.Start();

            var priceSwing = MockDataFactory.ExtractPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);

            //Act
            priceSwing.ForEach((s) => feedService.PriceBar.PriceAction(ticker, s));

            //Assert
            Assert.IsTrue(isTriggered);
            Assert.IsTrue(smaCross.Current.Status == EventStateOption.On);
        }

        [TestMethod]
        public void SMACrossProbe_PriceBarOptionFeedTriggersAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, PriceBarOption.Close);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 2, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            feedService.Start();

            var priceSwing = MockDataFactory.ExtractPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);

            //Act
            priceSwing.ForEach((s) => feedService.PriceBar.PriceAction(ticker, s.Timestamp, PriceBarOption.Close, s.Close));

            //Assert
            Assert.IsTrue(isTriggered);
            Assert.IsTrue(smaCross.Current.Status == EventStateOption.On);
        }
        [TestMethod]
        public void SMACrossProbe_OHLCFeed_DoesNotTriggerAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, OHLCPriceOption.All);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 3, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            var priceSwing = MockDataFactory.ExtractOHLCPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker, -1);

            //Act
            priceSwing.ForEach((s) => feedService.OHLC.PriceAction(ticker, s));

            //Assert
            Assert.IsFalse(isTriggered);
            Assert.AreEqual(0, smaCross.Length);
        }

        [TestMethod]
        public void SMACrossProbe_CandlestickItemFeedTriggersAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, CandlestickPriceOption.All);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 2, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            feedService.Start();

            var priceSwing = MockDataFactory.ExtractCandlestickPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);

            //Act
            priceSwing.ForEach((s) => feedService.Candlesticks.PriceAction(ticker, s));

            //Assert
            Assert.IsTrue(isTriggered);
            Assert.IsTrue(smaCross.Current.Status == EventStateOption.On);
        }

        [TestMethod]
        public void SMACrossProbe_CandlestickOptionFeedTriggersAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, CandlestickPriceOption.Close);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 2, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            feedService.Start();

            var priceSwing = MockDataFactory.ExtractPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);

            //Act
            priceSwing.ForEach((s) => feedService.Candlesticks.PriceAction(ticker, s.Timestamp, CandlestickPriceOption.Close, s.Close));

            //Assert
            Assert.IsTrue(isTriggered);
            Assert.IsTrue(smaCross.Current.Status == EventStateOption.On);
        }

        [TestMethod]
        public void SMACrossProbe_HeikenAshiItemFeedTriggersAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, HeikenAshiPriceOption.All);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 2, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            feedService.Start();

            var priceSwing = MockDataFactory.ExtractHeikenAshiPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);

            //Act
            priceSwing.ForEach((s) => feedService.HeikenAshi.PriceAction(ticker, s));

            //Assert
            Assert.IsTrue(isTriggered);
            Assert.IsTrue(smaCross.Current.Status == EventStateOption.On);
        }

        [TestMethod]
        public void SMACrossProbe_HeikenAshiOptionFeedTriggersAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, HeikenAshiPriceOption.Close);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 2, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            feedService.Start();

            var priceSwing = MockDataFactory.ExtractHeikenAshiPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);

            //Act
            priceSwing.ForEach((s) => feedService.HeikenAshi.PriceAction(ticker, s.Timestamp, HeikenAshiPriceOption.Close, s.Close));

            //Assert
            Assert.IsTrue(isTriggered);
            Assert.IsTrue(smaCross.Current.Status == EventStateOption.On);
        }

        public void SMACrossProbe_RenkoItemFeedTriggersAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, RenkoPriceOption.All);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 2, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            feedService.Start();

            var priceSwing = MockDataFactory.ExtractRenkoPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);

            //Act
            priceSwing.ForEach((s) => feedService.Renko.PriceAction(ticker, s));

            //Assert
            Assert.IsTrue(isTriggered);
            Assert.IsTrue(smaCross.Current.Status == EventStateOption.On);
        }

        [TestMethod]
        public void SMACrossProbe_RenkoOptionFeedTriggersAnEvent()
        {
            //Arrange

            var forexPair = MockObjectFactory.MockTickerSymbol(MockObjectFactory.EURUSD);
            var timeFrame = new TimeframeUnit(TimeframeOption.M1);

            var feedService = MockObjectFactory.MockManualPriceFeedService();
            var ticker = feedService.Setup(forexPair, timeFrame, RenkoPriceOption.Low);

            var isTriggered = false;

            var smaCross = new SMACrossProbe(ticker, 2, 5, () => isTriggered = true);
            smaCross.Subscribe(feedService);

            feedService.Start();

            var priceSwing = MockDataFactory.ExtractRenkoPrices(MockDataFactory.PriceSwingBeginMarker, MockDataFactory.PriceSwingEndMarker);

            //Act
            priceSwing.ForEach((s) => feedService.Renko.PriceAction(ticker, s.Timestamp, RenkoPriceOption.Low, s.Close));

            //Assert
            Assert.IsTrue(isTriggered);
            Assert.IsTrue(smaCross.Current.Status == EventStateOption.On);
        }
    }
}
