using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class HeikenAshiFeed: IPriceFeed<IHeikenAshiBar, HeikenAshiPriceOption>
    {
        public FeedRegistry<IHeikenAshiBar, HeikenAshiPriceOption> Registry { get; private set; }

        public HeikenAshiAggregator Aggregator { get; private set; }

        public HeikenAshiFeed()
        {
            Registry = new FeedRegistry<IHeikenAshiBar, HeikenAshiPriceOption>();
            Aggregator = new HeikenAshiAggregator();
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, HeikenAshiPriceOption ohlcPriceOption)
        {

            if (ohlcPriceOption == HeikenAshiPriceOption.All)
            {
                Registry.ItemSetup(tickerSymbol.Id, interval.UnitId);
            }
            else
            {
                Registry.OptionSetup(tickerSymbol.Id, interval.UnitId);
            }

            var tickerReference = new TickerReference(Guid.NewGuid(), tickerSymbol, interval, typeof(HeikenAshiPriceOption), (int)ohlcPriceOption);

            Aggregator.Setup(tickerReference);

            return tickerReference;
        }

        public IPriceItemFeed<IHeikenAshiBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IHeikenAshiBar> priceAction)
        {
            return Registry.Assign(tickerReference, priceAction);
        }

        public IPriceOptionFeed<HeikenAshiPriceOption, double> Subscribe(TickerReference tickerReference, HeikenAshiPriceOption priceOption, Action<TickerReference, long, HeikenAshiPriceOption, double> priceAction)
        {
            return Registry.Assign(tickerReference, priceOption, priceAction);
        }

        public void PriceAction(TickerReference tickerReference, IHeikenAshiBar priceBar)
        {
            Registry.PriceAction(tickerReference, priceBar);
        }

        public void PriceAction(TickerReference tickerReference, long timestamp, HeikenAshiPriceOption priceOption, double value)
        {
            Registry.PriceAction(tickerReference, timestamp, priceOption, value);
        }

        public void Wireup()
        {
            //Aggregator.Wireup(Registry);
        }
    }
}
