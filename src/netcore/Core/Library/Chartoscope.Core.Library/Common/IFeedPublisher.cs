using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IFeedPublisher<T>
    {
        IPriceItemFeed<T> Subscribe(TickerReference tickerReference, Action<T> priceAction);
    }
}
