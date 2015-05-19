using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public abstract class BrokerBase: IBroker
    {
        protected Dictionary<Guid, BrokerAccount> brokerAccounts= null;

        public BrokerBase()
        {
            brokerAccounts = new Dictionary<Guid, BrokerAccount>();
        }

        public void LoadAccount(BrokerAccount brokerAccount, Guid cacheId)
        {
            brokerAccount.Orders = new MarketOrders(brokerAccount.AccountId, cacheId);
            brokerAccounts.Add(brokerAccount.AccountId, brokerAccount);
        }


        public void LoadPriceHistory(CurrencyPairOption currencyPair, Dictionary<DateTime, BarItem> priceBars)
        {
            throw new NotImplementedException();
        }
    }
}
