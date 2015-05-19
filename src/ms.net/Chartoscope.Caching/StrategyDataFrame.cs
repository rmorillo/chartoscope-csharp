using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class StrategyDataFrame
    {
        private StrategyCache strategyCache = null;
        private PricebarCache pricebarCache = null;
        private Dictionary<string, IndicatorCache> indicatorCacheList = null;
        private Dictionary<string, SignalCache> signalCacheList = null;
        private BarItemType barType = null;
        private Guid cacheId = Guid.Empty;
        private int frameSize = 0;

        public StrategyDataFrame(string strategyIdentityCode, int frameSize, PricebarCache pricebarCache)
        {
            this.pricebarCache = pricebarCache;
            this.frameSize = frameSize;
            this.barType = pricebarCache.BarType;
            this.cacheId = pricebarCache.CacheId;
            this.strategyCache = new StrategyCache(strategyIdentityCode, this.barType, this.cacheId);
        }

        public StrategyFrameReader GetFrameReader(DateTime targetDate)
        {
            BarItem[] bars = pricebarCache.SelectBars(targetDate, frameSize);

            DateTime frameStartDate = bars[0].Time;
            DateTime frameEndDate = bars[bars.Length - 1].Time;

            return new StrategyFrameReader(frameStartDate, frameEndDate, this.frameSize, bars, strategyCache, barType, cacheId);
        }               
    }
}
