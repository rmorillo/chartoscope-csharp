using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public interface IBroker
    {
        void LoadAccount(BrokerAccount brokerAccount, Guid cacheId);
        void LoadPriceHistory(CurrencyPairOption currencyPair, Dictionary<DateTime, BarItem> priceBars);        
    }
}
