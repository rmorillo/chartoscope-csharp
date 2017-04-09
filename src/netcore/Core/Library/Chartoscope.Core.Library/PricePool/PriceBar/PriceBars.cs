using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class PriceBars: LookBehindPool<IPriceBar>, INotifyUpdate<IPriceBar, PriceBarOption>
    {
        public event Delegates.BarUpdateNotificationEventHandler<IPriceBar> BarUpdated;
        public event Delegates.PriceUpdateNotificationEventHandler<PriceBarOption, double> PriceUpdated;

        public PriceBars(int capacity) : base(capacity)
        {
        }

        protected override IPriceBar CreatePoolItem()
        {
            return new PriceBar();
        }

        public void Write(long timeStamp, double open, double high, double low, double close, double volume=0)
        {
            var priceBar = (PriceBar) NextPoolItem;
            priceBar.Write(timeStamp, open, high, low, close, volume);
            MoveNext();

            BarUpdated?.Invoke(Current);

            PriceUpdated?.Invoke(Current.Timestamp,PriceBarOption.Close, Current.Close);
        }
    }
}
