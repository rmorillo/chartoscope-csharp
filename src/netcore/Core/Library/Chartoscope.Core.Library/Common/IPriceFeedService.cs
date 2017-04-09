using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IPriceFeedService
    {                
        void Start();
        IPriceItemFeed<IPriceBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IPriceBar> priceAction);
        IPriceOptionFeed<PriceBarOption, double> Subscribe(TickerReference tickerReference, PriceBarOption priceOption, Action<TickerReference, long, PriceBarOption, double> priceAction);
        IPriceItemFeed<IOHLCBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IOHLCBar> priceAction);
        IPriceOptionFeed<OHLCPriceOption, double> Subscribe(TickerReference tickerReference, OHLCPriceOption priceOption, Action<TickerReference, long, OHLCPriceOption, double> priceAction);
        IPriceItemFeed<ICandlestickBar> Subscribe(TickerReference tickerReference, Action<TickerReference, ICandlestickBar> priceAction);
        IPriceOptionFeed<CandlestickPriceOption, double> Subscribe(TickerReference tickerReference, CandlestickPriceOption priceOption, Action<TickerReference, long, CandlestickPriceOption, double> priceAction);
        IPriceItemFeed<IHeikenAshiBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IHeikenAshiBar> priceAction);
        IPriceOptionFeed<HeikenAshiPriceOption, double> Subscribe(TickerReference tickerReference, HeikenAshiPriceOption priceOption, Action<TickerReference, long, HeikenAshiPriceOption, double> priceAction);
        IPriceItemFeed<IRenkoBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IRenkoBar> priceAction);
        IPriceOptionFeed<RenkoPriceOption, double> Subscribe(TickerReference tickerReference, RenkoPriceOption priceOption, Action<TickerReference, long, RenkoPriceOption, double> priceAction);
    }
}
