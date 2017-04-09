using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IPriceFeed<TBar, TOption>
    {
        FeedRegistry<TBar, TOption> Registry { get; }
        IPriceItemFeed<TBar> Subscribe(TickerReference tickerReference, Action<TickerReference, TBar> priceAction);
        IPriceOptionFeed<TOption, double> Subscribe(TickerReference tickerReference, TOption priceOption, Action<TickerReference, long, TOption, double> priceAction);
        void PriceAction(TickerReference tickerReference, TBar priceBar);
        void PriceAction(TickerReference tickerReference, long timestamp, TOption priceOption, double value);        
        void Wireup();
        TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, TOption ohlcPriceOption);
    }
}
