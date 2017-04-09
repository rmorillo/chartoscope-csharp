using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class PriceFeeder: IPriceFeeder
    {
        public PriceFeeder(IMarketFeedProvider feedProvider)
        {
        }

        public event Delegates.PriceBarFeedEventHandler MinutePriceBarFeed;
        public event Delegates.QuoteFeedEventHandler QuoteFeed;        

        protected void MinutePriceAction(TickerReference tickerReference, IPriceBar priceBar)
        {
            if (MinutePriceBarFeed != null)
            {
                MinutePriceBarFeed(tickerReference, priceBar);
            }
        }

        protected void QuotePriceAction(TickerReference tickerReference, long timestamp, double bid, double ask)
        {
            if (MinutePriceBarFeed != null)
            {
                QuoteFeed(timestamp, bid, ask);
            }
        }
    }
}
