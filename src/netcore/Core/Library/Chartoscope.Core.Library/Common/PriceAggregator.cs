using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public abstract class PriceAggregator<TBars, TItem, TOption>
    {
        private Dictionary<Guid, Dictionary<int, TBars>> _bars;

        public PriceAggregator()
        {
            _bars = new Dictionary<Guid, Dictionary<int, TBars>>();
        }

        public void Setup(TickerReference tickerReference)
        {
            if (!_bars.ContainsKey(tickerReference.Symbol.Id))
            {
                _bars.Add(tickerReference.Symbol.Id, new Dictionary<int, TBars>());
            }

            if (!_bars[tickerReference.Symbol.Id].ContainsKey(tickerReference.Interval.UnitId))
            {
                _bars[tickerReference.Symbol.Id].Add(tickerReference.Interval.UnitId, CreatePriceBars(tickerReference));
            }
        }

        public abstract TBars CreatePriceBars(TickerReference tickerReference);

        public void MinutePriceAction(TickerReference tickerReference, IPriceBar priceBar)
        {
            if (_bars.ContainsKey(tickerReference.Symbol.Id) && _bars[tickerReference.Symbol.Id].ContainsKey(tickerReference.Interval.UnitId))
            {
                UpdatePrice(_bars[tickerReference.Symbol.Id][tickerReference.Interval.UnitId], tickerReference, priceBar);
            }
        }

        public abstract void UpdatePrice(TBars bars, TickerReference tickerReference, IPriceBar priceBar);

        public void Wireup(FeedRegistry<TItem, TOption> feedRegistry)
        {

            foreach (var ticker in feedRegistry.Items)
            {
                foreach (var PriceItemFeed in ticker.Values)
                {
                    Wireup(_bars[PriceItemFeed.Ticker.Symbol.Id][PriceItemFeed.Ticker.Interval.UnitId],PriceItemFeed);
                }
            }

            foreach (var ticker in feedRegistry.Options)
            {
                foreach (var priceOption in ticker.Values)
                {
                    Wireup(_bars[priceOption.Ticker.Symbol.Id][priceOption.Ticker.Interval.UnitId], priceOption);
                }
            }
        }

        protected abstract void Wireup(TBars bars, PriceItemFeed<TickerReference, TItem> priceItemFeed);

        protected abstract void Wireup(TBars bars, PriceOptionFeed<TickerReference, TOption, double> priceOptionFeed);


    }
}
