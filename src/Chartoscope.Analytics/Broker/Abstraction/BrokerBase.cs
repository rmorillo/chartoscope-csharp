using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadroids.Analytics.Models;

namespace Metadroids.Analytics.Broker
{
    public abstract class BrokerBase: IBroker
    {
        private Dictionary<string, BrokerAccount> brokerAccounts= null;

        public BrokerBase()
        {
            brokerAccounts = new Dictionary<string, BrokerAccount>();
        }

        public void LoadAccount(BrokerAccount brokerAccount)
        {
            brokerAccounts.Add(brokerAccount.UserName, brokerAccount);
        }


        public void LoadPriceHistory(Common.Enumerations.CurrencyPairOption currencyPair, Dictionary<DateTime, Common.Models.BarItem> priceBars)
        {
            throw new NotImplementedException();
        }
    }
}
