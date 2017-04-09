using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class OHLCFeed : IPriceFeed<IOHLCBar, OHLCPriceOption>
    {
        public FeedRegistry<IOHLCBar, OHLCPriceOption> Registry { get; private set; }
        
        public OHLCAggregator Aggregator { get; private set; }

        public OHLCFeed()
        {
            Registry = new FeedRegistry<IOHLCBar, OHLCPriceOption>();            
            Aggregator = new OHLCAggregator();
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, OHLCPriceOption ohlcPriceOption)
        {
            
            if (ohlcPriceOption == OHLCPriceOption.All)
            {
                Registry.ItemSetup(tickerSymbol.Id, interval.UnitId);
            }
            else
            {
                Registry.OptionSetup(tickerSymbol.Id, interval.UnitId);
            }

            var tickerReference = new TickerReference(Guid.NewGuid(), tickerSymbol, interval, typeof(OHLCPriceOption), (int)ohlcPriceOption);

            Aggregator.Setup(tickerReference);

            return tickerReference;
        }        

        public IPriceItemFeed<IOHLCBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IOHLCBar> priceAction)
        {
            return Registry.Assign(tickerReference, priceAction);
        }

        public IPriceOptionFeed<OHLCPriceOption, double> Subscribe(TickerReference tickerReference, OHLCPriceOption priceOption, Action<TickerReference, long, OHLCPriceOption, double> priceAction)
        {
            return Registry.Assign(tickerReference, priceOption, priceAction);
        }

        public void PriceAction(TickerReference tickerReference, IOHLCBar priceBar)
        {
            Registry.PriceAction(tickerReference, priceBar);
        }

        public void PriceAction(TickerReference tickerReference, long timestamp, OHLCPriceOption priceOption, double value)
        {
            Registry.PriceAction(tickerReference, timestamp, priceOption, value);
        }

        public void Wireup()
        {
            Aggregator.Wireup(Registry);
        }
    }
}
