using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class PriceBarFeed : IPriceFeed<IPriceBar, PriceBarOption>
    {
        public FeedRegistry<IPriceBar, PriceBarOption> Registry { get; private set; }

        public PriceBarAggregator Aggregator { get; private set; }

        public PriceBarFeed()
        {
            Registry = new FeedRegistry<IPriceBar, PriceBarOption>();
            Aggregator = new PriceBarAggregator();
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, PriceBarOption ohlcPriceOption)
        {

            if (ohlcPriceOption == PriceBarOption.All)
            {
                Registry.ItemSetup(tickerSymbol.Id, interval.UnitId);
            }
            else
            {
                Registry.OptionSetup(tickerSymbol.Id, interval.UnitId);
            }

            var tickerReference = new TickerReference(Guid.NewGuid(), tickerSymbol, interval, typeof(PriceBarOption), (int)ohlcPriceOption);

            Aggregator.Setup(tickerReference);

            return tickerReference;
        }

        public IPriceItemFeed<IPriceBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IPriceBar> priceAction)
        {
            return Registry.Assign(tickerReference, priceAction);
        }

        public IPriceOptionFeed<PriceBarOption, double> Subscribe(TickerReference tickerReference, PriceBarOption priceOption, Action<TickerReference, long, PriceBarOption, double> priceAction)
        {
            return Registry.Assign(tickerReference, priceOption, priceAction);
        }

        public void PriceAction(TickerReference tickerReference, IPriceBar priceBar)
        {
            Registry.PriceAction(tickerReference, priceBar);
        }

        public void PriceAction(TickerReference tickerReference, long timestamp, PriceBarOption priceOption, double value)
        {
            Registry.PriceAction(tickerReference, timestamp, priceOption, value);
        }

        public void Wireup()
        {
            Aggregator.Wireup(Registry);
        }
    }
}
