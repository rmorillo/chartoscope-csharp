using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class RenkoFeed : IPriceFeed<IRenkoBar, RenkoPriceOption>
    {
        public FeedRegistry<IRenkoBar, RenkoPriceOption> Registry { get; private set; }

        public RenkoAggregator Aggregator { get; private set; }

        public RenkoFeed()
        {
            Registry = new FeedRegistry<IRenkoBar, RenkoPriceOption>();
            Aggregator = new RenkoAggregator();
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, RenkoPriceOption RenkoPriceOption)
        {

            if (RenkoPriceOption == RenkoPriceOption.All)
            {
                Registry.ItemSetup(tickerSymbol.Id, interval.UnitId);
            }
            else
            {
                Registry.OptionSetup(tickerSymbol.Id, interval.UnitId);
            }

            var tickerReference = new TickerReference(Guid.NewGuid(), tickerSymbol, interval, typeof(RenkoPriceOption), (int)RenkoPriceOption);

            Aggregator.Setup(tickerReference);

            return tickerReference;
        }

        public IPriceItemFeed<IRenkoBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IRenkoBar> priceAction)
        {
            return Registry.Assign(tickerReference, priceAction);
        }

        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, IRenkoBar> priceBarAction)
        {
            if (ticker.PriceFeedType == typeof(RenkoPriceOption) && ((RenkoPriceOption)ticker.PriceFeedOption) == RenkoPriceOption.All)
            {
                priceFeedService.Subscribe(ticker, priceBarAction);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, long, RenkoPriceOption, double> priceOptionAction)
        {
            if (ticker.PriceFeedType == typeof(RenkoPriceOption))
            {
                priceFeedService.Subscribe(ticker, (RenkoPriceOption)ticker.PriceFeedOption, priceOptionAction);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IPriceOptionFeed<RenkoPriceOption, double> Subscribe(TickerReference tickerReference, RenkoPriceOption priceOption, Action<TickerReference, long, RenkoPriceOption, double> priceAction)
        {
            return Registry.Assign(tickerReference, priceOption, priceAction);
        }

        public void PriceAction(TickerReference tickerReference, IRenkoBar priceBar)
        {
            Registry.PriceAction(tickerReference, priceBar);
        }

        public void PriceAction(TickerReference tickerReference, long timestamp, RenkoPriceOption priceOption, double value)
        {
            Registry.PriceAction(tickerReference, timestamp, priceOption, value);
        }

        public void Wireup()
        {
            Aggregator.Wireup(Registry);
        }
    }
}
