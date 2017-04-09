using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public abstract class EventProbe<TValue, TEvent> : ILookBehindReader<TValue> where TEvent : ILookBehindReader<TValue>
    {
        protected TickerReference Ticker;
        public EventProbe(TickerReference ticker)
        {
            Ticker = ticker;
        }

        protected TEvent Probe;

        public abstract void Subscribe(IPriceFeedService priceFeedService);

        protected void TrySubscribe(params Func<bool>[] subscriptions)
        {
            var subscribed = false;

            foreach(var trySubscribe in subscriptions)
            {
                if (trySubscribe())
                {
                    subscribed = true;
                    break;
                }
            }

            if (!subscribed)
            {
                throw new Exception("Failed to subscribe to any feed!");
            }
        }

        public TValue this[int index]
        {
            get
            {
                return Probe[index];
            }
        }

        public string CodeName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TValue Current
        {
            get
            {
                return Probe.Current;
            }
        }

        public string FullName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Length
        {
            get
            {
                return Probe.Length;
            }
        }

        public int Position
        {
            get
            {
                return Probe.Position;
            }
        }

        public TValue Previous
        {
            get
            {
                return Probe.Previous;
            }
        }

        public long Sequence
        {
            get
            {
                return Probe.Sequence;
            }
        }
    }
}
