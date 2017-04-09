using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface ICandlestickFeed
    {
        Dictionary<Guid, Dictionary<int, PriceItemFeed<TickerReference, ICandlestickBar>>> BarFeed { get; }
        Dictionary<Guid, Dictionary<int, PriceOptionFeed<TickerReference, CandlestickPriceOption, double>>> PriceOptionFeed { get; }
        TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, CandlestickPriceOption candlestickPriceOption);
    }
}
