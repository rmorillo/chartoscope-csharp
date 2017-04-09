using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class PriceItemFeed<TickerReference, T>: IPriceItemFeed<T>
    {
        private Dictionary<int, IProbe<T>> _probes;

        private Action<TickerReference, T> _priceAction;

        private TickerReference _tickerReference;
        public TickerReference Ticker { get { return _tickerReference; } }

        public PriceItemFeed(TickerReference tickerReference, Action<TickerReference, T> priceAction)
        {
            _tickerReference = tickerReference;
            _priceAction = priceAction;
            _probes = new Dictionary<int, IProbe<T>>();
        }

        public void PriceAction(T current)
        {
            foreach (var probe in _probes.Values)
            {
                probe.PriceAction(current);
            }

            _priceAction(_tickerReference, current);
        }

        public IPriceItemFeed<T> Register(IProbe<T> probe)
        {
            _probes.Add(probe.Id, probe);

            return this;
        }
    }
}
