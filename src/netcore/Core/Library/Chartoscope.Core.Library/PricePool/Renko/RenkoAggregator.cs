using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class RenkoAggregator : PriceAggregator<RenkoBars, IRenkoBar, RenkoPriceOption>
    {
        private Dictionary<Guid, RenkoBars> _bars;

        public RenkoAggregator()
        {
            _bars = new Dictionary<Guid, RenkoBars>();
        }

        public override RenkoBars CreatePriceBars(TickerReference tickerReference)
        {
            var bars = new RenkoBars(PricePoolSizeConfig.GetPoolSize(typeof(RenkoBars)), 10, 1);
            _bars.Add(tickerReference.Id, bars);
            return bars;
        }

        public override void UpdatePrice(RenkoBars bars, TickerReference tickerReference, IPriceBar priceBar)
        {
            bars.Write(priceBar.Timestamp, priceBar.Close);
        }

        protected override void Wireup(RenkoBars bars, PriceOptionFeed<TickerReference, RenkoPriceOption, double> priceOptionFeed)
        {
            bars.PriceUpdated += priceOptionFeed.PriceAction;
        }

        protected override void Wireup(RenkoBars bars, PriceItemFeed<TickerReference, IRenkoBar> priceItemFeed)
        {
            bars.BarUpdated += priceItemFeed.PriceAction;
        }
    }
}
