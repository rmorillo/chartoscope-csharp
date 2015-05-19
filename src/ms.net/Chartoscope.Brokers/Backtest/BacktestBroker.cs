using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Persistence;

namespace Chartoscope.Brokers
{
    public class BacktestBroker: BrokerBase
    {
        public Dictionary<Guid, BacktestSession> backtestSessions = null;

        private Guid cacheId = Guid.Empty;

        public BacktestSession CreateSession(Guid accountId, TickerType tickerType, BarItemType barType, string barDataFile, Guid cacheId)
        {
            if (backtestSessions == null)
            {
                backtestSessions = new Dictionary<Guid, BacktestSession>();
            }

            this.cacheId = cacheId;

            //this line should raise an exception if file is invalid
            BarItemFile.ValidateFile(barDataFile);
            
            BacktestSession backtestSession= new BacktestSession(accountId, tickerType, barDataFile, cacheId);
            //backtestSession.SignalClosePosition += OnSignalClosePosition;
            //backtestSession.SignalOpenPosition += OnSignalOpenPosition;

            backtestSession.StrategyClosePosition += OnStrategyClosePosition;
            backtestSession.StrategyOpenPosition+= OnStrategyOpenPosition;

            backtestSessions.Add(backtestSession.SessionId, backtestSession);

            brokerAccounts[accountId].Orders.Initialize(barType, backtestSession.SessionId);

            return backtestSession;
        }

        public void OpenCache(Guid accountId)
        {
            brokerAccounts[accountId].Orders.OpenCache();
        }

        public void CloseCache(Guid accountId)
        {
            brokerAccounts[accountId].Orders.CloseCache();
        }

        private void OnStrategyOpenPosition(Guid accountId, TickerType tickerType, PositionMode position, double bidPrice, double askPrice, BarItem barItem)
        {
            MarketOrderState marketOrderState = (MarketOrderState) Enum.Parse(typeof(MarketOrderState), Enum.GetName(typeof(PositionMode), position));
            brokerAccounts[accountId].Orders.CreateMarketOrder(DateTime.Now, tickerType, marketOrderState, bidPrice, askPrice, barItem);
        }

        private void OnStrategyClosePosition(Guid accountId, TickerType tickerType, double bidPrice, double askPrice, BarItem barItem)
        {
            brokerAccounts[accountId].Orders.CloseLastOrder(DateTime.Now, tickerType, bidPrice, askPrice, barItem);
        }

        private void OnSignalOpenPosition(Guid accountId, TickerType tickerType, MarketOrderState position, double bidPrice, double askPrice, BarItem barItem)
        {
            brokerAccounts[accountId].Orders.CreateMarketOrder(DateTime.Now, tickerType, position, bidPrice, askPrice, barItem);
        }

        private void OnSignalClosePosition(Guid accountId, TickerType tickerType, double bidPrice, double askPrice, BarItem barItem)
        {
            brokerAccounts[accountId].Orders.CloseLastOrder(DateTime.Now, tickerType, bidPrice, askPrice, barItem);
        }

        public double GetTotalProfit(Guid accountId, TickerType tickerType)
        {
            return brokerAccounts[accountId].Orders.GetTotalProfit(tickerType);
        }

        public double GetTotalLoss(Guid accountId, TickerType tickerType)
        {
            return brokerAccounts[accountId].Orders.GetTotalLoss(tickerType);
        }

    }
}
