using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class CandlestickFeed : IPriceFeed<ICandlestickBar, CandlestickPriceOption>
    {
        public FeedRegistry<ICandlestickBar, CandlestickPriceOption> Registry { get; private set; }

        public CandlestickAggregator Aggregator { get; private set; }

        public CandlestickFeed()
        {
            Registry = new FeedRegistry<ICandlestickBar, CandlestickPriceOption>();
            Aggregator = new CandlestickAggregator();
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, CandlestickPriceOption CandlestickPriceOption)
        {

            if (CandlestickPriceOption == CandlestickPriceOption.All)
            {
                Registry.ItemSetup(tickerSymbol.Id, interval.UnitId);
            }
            else
            {
                Registry.OptionSetup(tickerSymbol.Id, interval.UnitId);
            }

            var tickerReference = new TickerReference(Guid.NewGuid(), tickerSymbol, interval, typeof(CandlestickPriceOption), (int)CandlestickPriceOption);

            Aggregator.Setup(tickerReference);

            return tickerReference;
        }

        public IPriceItemFeed<ICandlestickBar> Subscribe(TickerReference tickerReference, Action<TickerReference, ICandlestickBar> priceAction)
        {
            return Registry.Assign(tickerReference, priceAction);
        }

        public IPriceOptionFeed<CandlestickPriceOption, double> Subscribe(TickerReference tickerReference, CandlestickPriceOption priceOption, Action<TickerReference, long, CandlestickPriceOption, double> priceAction)
        {
            return Registry.Assign(tickerReference, priceOption, priceAction);
        }

        public void PriceAction(TickerReference tickerReference, ICandlestickBar priceBar)
        {
            Registry.PriceAction(tickerReference, priceBar);
        }

        public void PriceAction(TickerReference tickerReference, long timestamp, CandlestickPriceOption priceOption, double value)
        {
            Registry.PriceAction(tickerReference, timestamp, priceOption, value);
        }

        public void Wireup()
        {
            Aggregator.Wireup(Registry);
        }
    }
}
