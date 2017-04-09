using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class ManualPriceFeeder : PriceFeeder
    {
        public ManualPriceFeeder(IMarketFeedProvider feedProvider) : base(feedProvider)
        {
        }

        public new void MinutePriceAction(TickerReference tickerReference, IPriceBar priceBar)
        {
            base.MinutePriceAction(tickerReference, priceBar);
        }
    }
}
