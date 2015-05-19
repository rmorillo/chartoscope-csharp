using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Caching;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public class MarketOrders
    {
        private Dictionary<Guid, MarketOrder> marketOrders = null;
        private MarketOrderCache marketOrderCache = null;
        private Dictionary<string, Guid> lastTickerOrder = null;

        private Guid sessionId = Guid.Empty;
        private Guid accountId = Guid.Empty;
        private Guid cacheId = Guid.Empty;

        public MarketOrders(Guid accountId, Guid cacheId)
        {
            marketOrders = new Dictionary<Guid, MarketOrder>();
            lastTickerOrder = new Dictionary<string, Guid>();            
            this.accountId = accountId;
            this.cacheId = cacheId;
        }

        public void OpenCache()
        {
            marketOrderCache.Open(CachingModeOption.Writing);
        }

        public void CloseCache()
        {
            marketOrderCache.Close();
        }


        public void Initialize(BarItemType barItemType, Guid sessionId)
        {
            this.sessionId = sessionId;
            marketOrderCache = new MarketOrderCache(barItemType, sessionId, cacheId);
            marketOrderCache.Initialize();
        }

        public Guid CreateMarketOrder(DateTime time, TickerType tickerType, MarketOrderState orderState, double bidPrice, double askPrice, BarItem barItem)
        {
            Guid marketOrderId= Guid.NewGuid();

            MarketOrder marketOrder= new MarketOrder(tickerType);
            marketOrder.OpenOrder(time, orderState, bidPrice, askPrice, barItem.Time);
            marketOrders.Add(marketOrderId, marketOrder);           
             
            if (lastTickerOrder.ContainsKey(tickerType.Symbol))
            {
                lastTickerOrder[tickerType.Symbol] = marketOrderId;
            }
            else
            {
                lastTickerOrder.Add(tickerType.Symbol, marketOrderId);
            }

            return marketOrderId;
        }

        public MarketOrder[] GetOrders()
        {
            return marketOrders.Values.ToArray();
        }

        public void CloseLastOrder(DateTime time, TickerType tickerType, double bidPrice, double askPrice, BarItem barItem)
        {
            MarketOrder marketOrder=  marketOrders[lastTickerOrder[tickerType.Symbol]];
            marketOrder.CloseOrder(time, bidPrice, askPrice, barItem.Time);
            marketOrderCache.AppendMarketOrder(marketOrders.Count, marketOrder.Position, marketOrder.OrderTime, marketOrder.OpeningBarTime, marketOrder.OpeningBidPrice, marketOrder.OpeningAskPrice,
                barItem.Time, marketOrder.ClosingBarTime, marketOrder.ClosingBidPrice, marketOrder.ClosingAskPrice);

        }

        public double GetTotalProfit(TickerType tickerType)
        {
            double profit = 0;
            foreach (MarketOrder marketOrder in marketOrders.Values)
            {
                if (marketOrder.Ticker.Symbol == tickerType.Symbol)
                {
                    profit += marketOrder.ProfitInPips;
                }
            }
            return profit;
        }

        public double GetTotalLoss(TickerType tickerType)
        {
            double loss = 0;
            foreach (MarketOrder marketOrder in marketOrders.Values)
            {
                if (marketOrder.Ticker.Symbol == tickerType.Symbol)
                {
                    loss += marketOrder.LossInPips;
                }
            }
            return loss;
        }

        public int GetTotalWinningOrders(TickerType tickerType)
        {
            int winningOrders = 0;
            foreach (MarketOrder marketOrder in marketOrders.Values)
            {
                if (marketOrder.ProfitOrLossInPips > 0)
                {
                    winningOrders++;
                }
            }
            return winningOrders;
        }

        public int GetTotalLosingOrders(TickerType tickerType)
        {
            int losingOrders = 0;
            foreach (MarketOrder marketOrder in marketOrders.Values)
            {
                if (marketOrder.ProfitOrLossInPips < 0)
                {
                    losingOrders++;
                }
            }
            return losingOrders;
        }

        public int GetTotalBreakevenOrders(TickerType tickerType)
        {
            int breakevenOrders = 0;
            foreach (MarketOrder marketOrder in marketOrders.Values)
            {
                if (marketOrder.ProfitOrLossInPips == 0)
                {
                    breakevenOrders++;
                }
            }
            return breakevenOrders;
        }

    }
}
