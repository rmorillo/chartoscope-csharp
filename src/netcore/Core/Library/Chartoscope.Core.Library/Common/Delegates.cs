using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class Delegates
    {
        public delegate void BarUpdateNotificationEventHandler<T>(T current);
        public delegate void PriceUpdateNotificationEventHandler<TOption, TValue>(long currentTimestmap, TOption currentPriceOption, TValue currentPriceValue);

        public delegate void BuySignalEventHandler(int strength = 0);

        public delegate void SellSignalEventHandler(int strength = 0);

        public delegate void CloseSignalEventHandler();

        public delegate void PriceBarFeedEventHandler(TickerReference tickerReference, IPriceBar priceBar);

        public delegate void QuoteFeedEventHandler(long timestamp, double bid, double ask);
    }
}
