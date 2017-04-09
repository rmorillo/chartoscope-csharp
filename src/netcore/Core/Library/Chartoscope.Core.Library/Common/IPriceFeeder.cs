using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IPriceFeeder
    {
        event Delegates.PriceBarFeedEventHandler MinutePriceBarFeed;
        event Delegates.QuoteFeedEventHandler QuoteFeed;
    }
}
