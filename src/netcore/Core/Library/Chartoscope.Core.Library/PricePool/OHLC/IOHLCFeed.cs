using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IOHLCFeed
    {
        Dictionary<Guid, Dictionary<int, PriceItemFeed<TickerReference, IOHLCBar>>> BarFeed { get; }
        Dictionary<Guid, Dictionary<int, PriceOptionFeed<TickerReference, OHLCPriceOption, double>>> PriceOptionFeed { get; }
        IPriceItemFeed<IOHLCBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IOHLCBar> priceAction);
        IPriceOptionFeed<OHLCPriceOption, double> Subscribe(TickerReference tickerReference, OHLCPriceOption priceOption, Action<TickerReference, long, OHLCPriceOption, double> priceAction);
        void PriceAction(TickerReference tickerReference, IOHLCBar priceBar);
        void PriceAction(TickerReference tickerReference, long timestamp, OHLCPriceOption priceOption, double value);

        OHLCAggregator Aggregator { get; }

        void Wireup();

        TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, OHLCPriceOption ohlcPriceOption);
    }
}
