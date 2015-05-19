using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class SignalCacheNavigator
    {
        private SignalCache signalCache = null;
        private PricebarCache pricebarCache = null;
        private Dictionary<string, IndicatorCache> indicatorCacheList = null;
        private BarItemType barType = null;
        private Guid cacheId = Guid.Empty;        

        private SignalDataFrame signalDataFrame = null;

        public SignalCacheNavigator(string cacheFolder, BarItemType barType, Guid cacheId, string signalIdentityCode)
        {
            this.signalCache = new SignalCache(signalIdentityCode, barType, cacheId);
            this.pricebarCache = new PricebarCache(barType, cacheId, CacheModeOption.Read);
 
            this.barType = barType;
            this.cacheId = cacheId;

            CacheHeaderInfo headerInfo = this.signalCache.Header;

            foreach(KeyValuePair<string, string> extendedProperty in headerInfo.ExtendedProperties)
            {
                switch (extendedProperty.Key)
                {
                    case "Indicators":
                        indicatorCacheList = CacheHelper.LoadIndicatorCacheList(extendedProperty.Value, barType, cacheId);                                               
                        break;
                }
            }

            InitializeSignalDataFrame();
        }

        private DateTime startBarDate= DateTime.MinValue;

        public DateTime StartBarDate
        {
            get 
            {
                if (startBarDate == DateTime.MinValue)
                {
                    startBarDate = new DateTime((long)pricebarCache.Select(0)[0][0]);
                }
                return startBarDate;
            }
        }

        private DateTime endBarDate= DateTime.MinValue;

        public DateTime EndBarDate
        {
            get 
            {
                if (endBarDate == DateTime.MinValue)
                {
                    endBarDate = new DateTime((long)pricebarCache.Select(pricebarCache.LastRowCount)[0][0]);
                }
                return endBarDate;
            }
        }

        private Dictionary<DateTime, SignalDataItem[]> timeKeyedSignals = null;

        private void InitializeSignalDataFrame()
        {
            CacheHeaderInfo[] indicatorCacheHeaders = new CacheHeaderInfo[indicatorCacheList.Count];
            int index = 0;
            foreach (IndicatorCache indicatorCache in indicatorCacheList.Values)
            {
                indicatorCacheHeaders[index] = indicatorCache.Header;
                index++;
            }

            this.signalDataFrame= new SignalDataFrame(signalCache.Header, indicatorCacheHeaders);
            timeKeyedSignals = this.signalCache.SelectAllSignals();
        }

        private SignalDataItem[] GetSignals(DateTime startDate, DateTime endDate)
        {
            List<SignalDataItem> signals = new List<SignalDataItem>();
            foreach (KeyValuePair<DateTime, SignalDataItem[]> signalValuePair in this.timeKeyedSignals)
            {               
                if (signalValuePair.Key >= startDate && signalValuePair.Key <= endDate)
                {
                    signals.AddRange(signalValuePair.Value);
                }
            }

            return signals.Count>0? signals.ToArray(): null;
        }

        public SignalDataFrame GetSignalDataFrame(DateTime targetDate, int frameSize)
        {
            signalDataFrame.Clear();

            BarItem[] bars= pricebarCache.SelectBars(targetDate, frameSize); 
            
            DateTime frameStartDate= bars[0].Time;
            DateTime frameEndDate= bars[bars.Length-1].Time;

            SignalDataItem[] signalDataItems = this.GetSignals(frameStartDate, frameEndDate);
            Dictionary<string, Dictionary<long, IndicatorDataItem>> indicatorDataItems = new Dictionary<string, Dictionary<long, IndicatorDataItem>>();

            foreach (KeyValuePair<string, IndicatorCache> indicator in indicatorCacheList)
            {
                indicatorDataItems.Add(indicator.Key, indicator.Value.SelectIndicators(frameStartDate, frameEndDate, frameSize));
            }
            int signalOffset = 0;
            foreach(BarItem bar in bars)
            {
                List<SignalDataItem> signals = FindSignalDataItem(signalDataItems, bar.Time, ref signalOffset);
                Dictionary<string, IndicatorDataItem> indicators = new Dictionary<string, IndicatorDataItem>();
                Dictionary<string, long> indicatorOffsets = new Dictionary<string, long>();
                foreach (KeyValuePair<string, IndicatorCache> indicator in indicatorCacheList)
                {
                    indicatorOffsets.Add(indicator.Key, 0);
                }
                foreach (KeyValuePair<string, IndicatorCache> indicator in indicatorCacheList)
                {
                    long indicatorOffset= indicatorOffsets[indicator.Key];
                    IndicatorDataItem indicatorDataItem= FindIndicatorDataItem(indicatorDataItems[indicator.Key], bar.Time, ref indicatorOffset);
                    if (indicatorDataItem != null)
                    {
                        indicators.Add(indicator.Key, indicatorDataItem);
                    }
                    indicatorOffsets[indicator.Key] = indicatorOffset;
                }

                signalDataFrame.Add(bar.Time, new SignalDataFrameItem(bar, signals, indicators));
            }

            return signalDataFrame;
        }

        private IndicatorDataItem FindIndicatorDataItem(Dictionary<long, IndicatorDataItem> indicatorDataItems, DateTime targetDate, ref long offset)
        {
            IndicatorDataItem result = null;

            foreach (long key in indicatorDataItems.Keys) 
            {
                if (indicatorDataItems[key].Time == targetDate)
                {
                    result = indicatorDataItems[key];                    
                }
                else if (indicatorDataItems[key].Time > targetDate)
                {
                    break;
                }
            }

            return result;
        }

        private List<SignalDataItem> FindSignalDataItem(SignalDataItem[] signalDataItems, DateTime targetDate, ref int offset)
        {
            List<SignalDataItem> result = new List<SignalDataItem>();
            if (signalDataItems != null)
            {
                for (int index = offset + 1; index < signalDataItems.Length; index++)
                {
                    if (signalDataItems[index].ClosingBarTime == targetDate)
                    {
                        result.Add(signalDataItems[index]);
                    }
                    else if (signalDataItems[index].ClosingBarTime > targetDate)
                    {
                        break;
                    }
                    offset = index;
                }
            }

            return result.Count>0? result: null;
        }        
    }
}
