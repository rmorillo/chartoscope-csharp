using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class HeikenAshiAggregator : PriceAggregator<HeikenAshiBars, IHeikenAshiBar, HeikenAshiPriceOption>
    {
        private Dictionary<Guid, HeikenAshiBars> _bars;

        public HeikenAshiAggregator()
        {
            _bars = new Dictionary<Guid, HeikenAshiBars>();
        }

        public override HeikenAshiBars CreatePriceBars(TickerReference tickerReference)
        {
            var bars = new HeikenAshiBars(PricePoolSizeConfig.GetPoolSize(typeof(HeikenAshiBars)));
            _bars.Add(tickerReference.Id, bars);
            return bars;
        }

        public override void UpdatePrice(HeikenAshiBars bars, TickerReference tickerReference, IPriceBar priceBar)
        {
            bars.Write(priceBar);
        }

        protected override void Wireup(HeikenAshiBars bars, PriceOptionFeed<TickerReference, HeikenAshiPriceOption, double> priceOptionFeed)
        {
            bars.PriceUpdated += priceOptionFeed.PriceAction;
        }

        protected override void Wireup(HeikenAshiBars bars, PriceItemFeed<TickerReference, IHeikenAshiBar> priceItemFeed)
        {
            bars.BarUpdated += priceItemFeed.PriceAction;
        }
    }
}
