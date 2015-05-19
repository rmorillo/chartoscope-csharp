using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class StrategyFrameReader
    {
        internal StrategyFrameReader(DateTime startDate, DateTime endDate, int frameSize, BarItem[] priceBars, StrategyCache cache, BarItemType barType, Guid cacheId)
        {
            this.startDate = startDate;
            this.endDate = endDate;
            this.frameSize = frameSize;
            this.priceBars = priceBars;
            dataFrameItems = cache.SelectStrategies(startDate, endDate, frameSize);

            if (dataFrameItems != null)
            {
                strategyIndex = new Dictionary<DateTime, int>();
                int index = 0;
                DateTime lastClosingBarDate = DateTime.MinValue;
                foreach (StrategyDataItem item in dataFrameItems)
                {
                    if (lastClosingBarDate != item.ClosingBarTime)
                    {
                        strategyIndex.Add(item.ClosingBarTime, index);
                        lastClosingBarDate = item.ClosingBarTime;
                    }

                    index++;
                }
            }

            CacheHeaderInfo headerInfo = cache.Header;

            foreach (KeyValuePair<string, string> extendedProperty in headerInfo.ExtendedProperties)
            {
                switch (extendedProperty.Key)
                {
                    case "Indicators":
                        indicatorCacheList = CacheHelper.LoadIndicatorCacheList(extendedProperty.Value, barType, cacheId);
                        break;
                }
            }
        }

        private BarItem[] priceBars;

        public BarItem[] PriceBars
        {
            get { return priceBars; }
        }


        private DateTime startDate = DateTime.MinValue;
        private DateTime endDate = DateTime.MinValue;
        private int frameSize = 0;

        public IndicatorFrameReader GetIndicatorFrameReader(string identityCode)
        {
            return new IndicatorFrameReader(startDate, endDate, frameSize, indicatorCacheList[identityCode]);
        }

        private Dictionary<string, IndicatorCache> indicatorCacheList = null;
        private Dictionary<DateTime, int> strategyIndex = null;
        private StrategyDataItem[] dataFrameItems = null;

        public MarketOrderState Read(DateTime time)
        {
            MarketOrderState strategy = MarketOrderState.NoOrder;
            if (strategyIndex.ContainsKey(time))
            {
                int strategyPosition = strategyIndex[time];

                //Check if position is Closed
                if (dataFrameItems[strategyPosition].Position == PositionMode.Closed)
                {
                    //Check for reversal
                    int nextPosition = strategyPosition + 1;
                    if (nextPosition < dataFrameItems.Length && dataFrameItems[nextPosition].ClosingBarTime == time)
                    {
                        if (dataFrameItems[nextPosition].Position == PositionMode.Long)
                        {
                            strategy = MarketOrderState.LongReversal;
                        }
                        else if (dataFrameItems[nextPosition].Position == PositionMode.Short)
                        {
                            strategy = MarketOrderState.ShortReversal;
                        }
                        else
                        {
                            strategy = MarketOrderState.Closed;
                        }
                    }
                    else
                    {
                        strategy = MarketOrderState.Closed;
                    }
                }
                else if (dataFrameItems[strategyPosition].Position == PositionMode.Long)
                {
                    strategy = MarketOrderState.Long;
                }
                else if (dataFrameItems[strategyPosition].Position == PositionMode.Short)
                {
                    strategy = MarketOrderState.Short;
                }
                else if (dataFrameItems[strategyPosition].HasOrderUpdate)
                {
                    strategy = MarketOrderState.OrderUpdate;
                }

            }
            return strategy;
        }
    }
}
