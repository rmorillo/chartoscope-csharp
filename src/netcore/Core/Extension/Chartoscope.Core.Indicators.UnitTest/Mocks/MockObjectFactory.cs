using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators.UnitTest
{
    public class MockObjectFactory
    {
        public const string EURUSD = "EURUSD";
        public static MockFeedProvider MockFeedProvider() { return new MockFeedProvider(); }

        public static TickerSymbol MockTickerSymbol(string tickerSymbol) { return new TickerSymbol(tickerSymbol); } 

        public static PriceFeedService MockManualPriceFeedService()
        {
            var mockFeedProvider = MockObjectFactory.MockFeedProvider();            

            var manualFeed = new ManualPriceFeeder(mockFeedProvider);

            return new PriceFeedService(manualFeed);
        }
    }
}
