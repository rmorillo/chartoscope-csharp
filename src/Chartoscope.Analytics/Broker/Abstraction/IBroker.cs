using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadroids.Analytics.Models;
using Metadroids.Common.Enumerations;
using Metadroids.Common.Models;

namespace Metadroids.Analytics.Broker
{
    public interface IBroker
    {
        void LoadAccount(BrokerAccount brokerAccount);
        void LoadPriceHistory(CurrencyPairOption currencyPair, Dictionary<DateTime, BarItem> priceBars);        
    }
}
