using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class FeedRegistry<TItem, TOption>
    {
        public FeedRegistry()
        {
            _item = new Dictionary<Guid, Dictionary<int, PriceItemFeed<TickerReference, TItem>>>();
            _option = new Dictionary<Guid, Dictionary<int, PriceOptionFeed<TickerReference, TOption, double>>>();
        }

        private Dictionary<Guid, Dictionary<int, PriceItemFeed<TickerReference, TItem>>> _item;
        private Dictionary<Guid, Dictionary<int, PriceOptionFeed<TickerReference, TOption, double>>> _option;

        public Dictionary<int, PriceItemFeed<TickerReference, TItem>>[] Items { get { return _item.Values.ToArray() ;} }
        public Dictionary<int, PriceOptionFeed<TickerReference, TOption, double>>[] Options { get { return _option.Values.ToArray(); } }
        public void ItemSetup(Guid tickerSymbolId, int intervalUnitId)
        {
            Dictionary<int, PriceItemFeed<TickerReference, TItem>> itemFeed= null;

            if (_item.ContainsKey(tickerSymbolId))
            {
                itemFeed = _item[tickerSymbolId];
            }
            else
            {
                itemFeed = new Dictionary<int, PriceItemFeed<TickerReference, TItem>>();                
                _item.Add(tickerSymbolId, itemFeed);
            }

            itemFeed.Add(intervalUnitId, null);
        }

        public void OptionSetup(Guid tickerSymbolId, int intervalUnitId)
        {
            Dictionary<int, PriceOptionFeed<TickerReference, TOption, double>> optionFeed = null;

            if (_option.ContainsKey(tickerSymbolId))
            {
                optionFeed = _option[tickerSymbolId];
            }
            else
            {
                optionFeed = new Dictionary<int, PriceOptionFeed<TickerReference, TOption, double>>();
                _option.Add(tickerSymbolId, optionFeed);
            }

            optionFeed.Add(intervalUnitId, null);
        }

        public IPriceItemFeed<TItem> Assign(TickerReference tickerReference, Action<TickerReference, TItem> priceAction)
        {
            var itemFeed= new PriceItemFeed<TickerReference, TItem>(tickerReference, priceAction);
            _item[tickerReference.Symbol.Id][tickerReference.Interval.UnitId] = itemFeed;
            return itemFeed;
        }

        public IPriceOptionFeed<TOption, double> Assign(TickerReference tickerReference, TOption priceOption, Action<TickerReference, long, TOption, double> priceAction)
        {
            var optionFeed = new PriceOptionFeed<TickerReference, TOption, double>(tickerReference, priceOption, priceAction);
            _option[tickerReference.Symbol.Id][tickerReference.Interval.UnitId] = optionFeed;
            return optionFeed;
        }

        public void PriceAction(TickerReference tickerReference, TItem priceBar)
        {
            _item[tickerReference.Symbol.Id][tickerReference.Interval.UnitId].PriceAction(priceBar);
        }

        public void PriceAction(TickerReference tickerReference, long timestamp, TOption priceOption, double value)
        {
            _option[tickerReference.Symbol.Id][tickerReference.Interval.UnitId].PriceAction(timestamp, priceOption, value);
        }
    }
}
