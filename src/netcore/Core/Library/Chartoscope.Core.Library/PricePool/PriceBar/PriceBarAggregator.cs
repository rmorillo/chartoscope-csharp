using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class PriceBarAggregator : PriceAggregator<PriceBars, IPriceBar, PriceBarOption>
    {
        private Dictionary<Guid, PriceBars> _bars;

        public PriceBarAggregator()
        {
            _bars = new Dictionary<Guid, PriceBars>();
        }

        public override PriceBars CreatePriceBars(TickerReference tickerReference)
        {
            var bars = new PriceBars(PricePoolSizeConfig.GetPoolSize(typeof(PriceBars)));
            _bars.Add(tickerReference.Id, bars);
            return bars;
        }

        public override void UpdatePrice(PriceBars bars, TickerReference tickerReference, IPriceBar priceBar)
        {
            bars.Write(priceBar.Timestamp, priceBar.Open, priceBar.High, priceBar.Low, priceBar.Close, priceBar.Volume);
        }

        protected override void Wireup(PriceBars bars, PriceOptionFeed<TickerReference, PriceBarOption, double> priceOptionFeed)
        {
            bars.PriceUpdated += priceOptionFeed.PriceAction;
        }

        protected override void Wireup(PriceBars bars, PriceItemFeed<TickerReference, IPriceBar> priceItemFeed)
        {
            bars.BarUpdated += priceItemFeed.PriceAction;
        }
    }
}
