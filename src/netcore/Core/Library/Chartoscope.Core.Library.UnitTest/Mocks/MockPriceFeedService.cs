using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest.Mocks
{
    public class MockPriceFeedService : IPriceFeedService
    {
        public IOHLCFeed OHLC
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void PriceAction(TickerReference tickerReference, IPriceBar priceBar)
        {
            throw new NotImplementedException();
        }

        public void QuoteAction(DateTime timestamp, double bid, double ask)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public IPriceItemFeed<IHeikenAshiBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IHeikenAshiBar> priceAction)
        {
            throw new NotImplementedException();
        }

        public IPriceItemFeed<IRenkoBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IRenkoBar> priceAction)
        {
            throw new NotImplementedException();
        }

        public IPriceItemFeed<IPriceBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IPriceBar> priceAction)
        {
            throw new NotImplementedException();
        }

        public IPriceItemFeed<ICandlestickBar> Subscribe(TickerReference tickerReference, Action<TickerReference, ICandlestickBar> priceAction)
        {
            throw new NotImplementedException();
        }

        public IPriceItemFeed<IOHLCBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IOHLCBar> priceAction)
        {
            return null;
        }

        public IPriceOptionFeed<RenkoPriceOption, double> Subscribe(TickerReference tickerReference, RenkoPriceOption priceOption, Action<TickerReference, long, RenkoPriceOption, double> priceAction)
        {
            throw new NotImplementedException();
        }

        public IPriceOptionFeed<CandlestickPriceOption, double> Subscribe(TickerReference tickerReference, CandlestickPriceOption priceOption, Action<TickerReference, long, CandlestickPriceOption, double> priceAction)
        {
            throw new NotImplementedException();
        }

        public IPriceOptionFeed<HeikenAshiPriceOption, double> Subscribe(TickerReference tickerReference, HeikenAshiPriceOption priceOption, Action<TickerReference, long, HeikenAshiPriceOption, double> priceAction)
        {
            throw new NotImplementedException();
        }

        public IPriceOptionFeed<PriceBarOption, double> Subscribe(TickerReference tickerReference, PriceBarOption priceOption, Action<TickerReference, long, PriceBarOption, double> priceAction)
        {
            throw new NotImplementedException();
        }

        public IPriceOptionFeed<OHLCPriceOption, double> Subscribe(TickerReference tickerReference, OHLCPriceOption priceOption, Action<TickerReference, long, OHLCPriceOption, double> priceAction)
        {
            return null;
        }
    }
}
