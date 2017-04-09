using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class PriceOptionFeed<TickerReference, TOption, TValue>: IPriceOptionFeed<TOption, TValue>
    {
        private Dictionary<int, IProbe<TValue>> _probes;

        private Action<TickerReference, long, TOption, TValue> _priceAction;

        private TickerReference _tickerReference;
        public TickerReference Ticker { get { return _tickerReference; } }

        private TOption _priceOption;           

        public PriceOptionFeed(TickerReference tickerReference, TOption priceOption, Action<TickerReference, long, TOption, TValue> priceAction)
        {
            _tickerReference = tickerReference;
            _priceOption = priceOption;
            _priceAction = priceAction;
            _probes = new Dictionary<int, IProbe<TValue>>();
        }

        public void PriceAction(long currentTimestamp, TOption currentPriceOption, TValue currentValue, 
            long previousTimestamp, TOption previousPriceOption, TValue previousValue)
        {
            PriceAction(currentTimestamp, currentPriceOption, currentValue);
        }

        public void PriceAction(long timestamp, TOption priceOption, TValue priceBar)
        {
            foreach(var probe in _probes.Values)
            {
                probe.PriceAction(priceBar);
            }

            _priceAction(_tickerReference, timestamp, priceOption, priceBar);
        }        

        public IPriceOptionFeed<TOption, TValue> Register(IProbe<TValue> probe)
        {
            _probes.Add(probe.Id, probe);

            return this;
        }
    }
}
