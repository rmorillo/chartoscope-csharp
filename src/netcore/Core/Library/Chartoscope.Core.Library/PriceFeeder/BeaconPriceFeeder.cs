using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class BeaconPriceFeeder : PriceFeeder
    {
        public BeaconPriceFeeder(MarketFeedProvider feedProvider) : base(feedProvider)
        {
        }
    }
}
