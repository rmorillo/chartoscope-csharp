using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IRenkoSubscriber
    {
        void RenkoPriceActionItem(TickerReference tickerReference, IRenkoBar renkoBar);
        void RenkoPriceActionOption(TickerReference tickerReference, long timestamp, RenkoPriceOption priceOption, double value);
    }
}
