using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class CandlestickAggregator : PriceAggregator<CandlestickBars, ICandlestickBar, CandlestickPriceOption>
    {
        private Dictionary<Guid, CandlestickBars> _bars;

        public CandlestickAggregator()
        {
            _bars = new Dictionary<Guid, CandlestickBars>();
        }

        public override CandlestickBars CreatePriceBars(TickerReference tickerReference)
        {
            var bars = new CandlestickBars(PricePoolSizeConfig.GetPoolSize(typeof(CandlestickBars)));
            _bars.Add(tickerReference.Id, bars);
            return bars;
        }

        public override void UpdatePrice(CandlestickBars bars, TickerReference tickerReference, IPriceBar priceBar)
        {
            bars.Write(priceBar);
        }

        protected override void Wireup(CandlestickBars bars, PriceOptionFeed<TickerReference, CandlestickPriceOption, double> priceOptionFeed)
        {
            bars.PriceUpdated += priceOptionFeed.PriceAction;
        }

        protected override void Wireup(CandlestickBars bars, PriceItemFeed<TickerReference, ICandlestickBar> priceItemFeed)
        {
            bars.BarUpdated += priceItemFeed.PriceAction;
        }
    }
}
