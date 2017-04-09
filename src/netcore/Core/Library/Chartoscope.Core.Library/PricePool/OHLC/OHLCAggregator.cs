using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class OHLCAggregator : PriceAggregator<OHLCBars, IOHLCBar, OHLCPriceOption>
    {
        private Dictionary<Guid, OHLCBars> _bars;
        
        public OHLCAggregator()
        {
            _bars = new Dictionary<Guid, OHLCBars>();
        } 

        public override OHLCBars CreatePriceBars(TickerReference tickerReference)
        {
            var bars= new OHLCBars(PricePoolSizeConfig.GetPoolSize(typeof(OHLCBars)));
            _bars.Add(tickerReference.Id, bars);
            return bars;
        }

        public override void UpdatePrice(OHLCBars bars, TickerReference tickerReference, IPriceBar priceBar)
        {
            bars.Write(priceBar);
        }

        protected override void Wireup(OHLCBars bars, PriceOptionFeed<TickerReference, OHLCPriceOption, double> priceOptionFeed)
        {
            bars.PriceUpdated += priceOptionFeed.PriceAction;
        }

        protected override void Wireup(OHLCBars bars, PriceItemFeed<TickerReference, IOHLCBar> priceItemFeed)
        {
            bars.BarUpdated+= priceItemFeed.PriceAction;
        }
    }
}
