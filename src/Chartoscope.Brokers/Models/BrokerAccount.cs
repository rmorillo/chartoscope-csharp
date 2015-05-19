using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;


namespace Chartoscope.Brokers
{
    public class BrokerAccount
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private double fundAmount;

        public double FundAmount
        {
            get { return fundAmount; }
            set { fundAmount = value; }
        }

        private CurrencyOption fundCurrency;

        public CurrencyOption FundCurrency
        {
            get { return fundCurrency; }
            set { fundCurrency = value; }
        }

        private Guid accountId;

        public Guid AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        public BrokerAccount()
        {
        }
                
        public BrokerAccount(string userName, string password, double fundAmount, CurrencyOption fundCurrency)
        {
            this.userName = userName;
            this.password = password;
            this.fundAmount = fundAmount;
            this.fundCurrency = fundCurrency;
            this.accountId = Guid.NewGuid();
        }

        private MarketOrders orders;

        public MarketOrders Orders
        {
            get { return orders; }
            set { orders = value; }
        }

    }
}
