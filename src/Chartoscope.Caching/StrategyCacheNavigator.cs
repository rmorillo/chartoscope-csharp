using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Droidworks.Common;

namespace Droidworks.Caching
{
    public class StrategyCacheNavigator
    {
        private StrategyCache strategyCache = null;
        private PricebarCache pricebarCache = null;
        private Dictionary<string, IndicatorCache> indicatorCacheList = null;
        private Dictionary<string, SignalCache> signalCacheList = null;
        private BarItemType barType = null;
        private Guid cacheId = Guid.Empty;

        private StrategyDataFrame strategyDataFrame = null;

        private Dictionary<string, StrategyCacheProfile> strategyCacheProfiles = null;

        public StrategyCacheNavigator(string cacheFolder, BarItemType barType, Guid cacheId, string[] strategyIdentityCodes)
        {
            this.pricebarCache = new PricebarCache(barType, cacheId, CacheModeOption.Read);

            this.barType = barType;
            this.cacheId = cacheId;

            strategyCacheProfiles = new Dictionary<string, StrategyCacheProfile>();

            foreach (string strategyIdentity in strategyIdentityCodes)
            {
                strategyCacheProfiles.Add(strategyIdentity, new StrategyCacheProfile(strategyIdentity, barType, cacheId));
            }
            
            InitializeDataFrame();
        }

        private DateTime startBarDate = DateTime.MinValue;

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

        private DateTime endBarDate = DateTime.MinValue;

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
        private Dictionary<DateTime, StrategyDataItem[]> timeKeyedStrategies = null;

        Dictionary<string, List<AnalyticsOwnerItem>> uniqueSignals = null;
        Dictionary<string, List<AnalyticsOwnerItem>> uniqueAlerts = null;
        Dictionary<string, List<AnalyticsOwnerItem>> uniqueIndicators = null;
        List<string> strategies = null;

        private void InitializeDataFrame()
        {
            this.strategyDataFrame = new StrategyDataFrame();

            strategies = new List<string>();
            uniqueSignals = new Dictionary<string, List<AnalyticsOwnerItem>>();
            uniqueIndicators= new Dictionary<string,List<AnalyticsOwnerItem>>();

            foreach (KeyValuePair<string, StrategyCacheProfile> strategyProfile in this.strategyCacheProfiles)
            {
                strategies.Add(strategyProfile.Key);

                foreach (SignalCacheProfile signalProfile in strategyProfile.Value.Signals)
                {
                    if (uniqueSignals.ContainsKey(signalProfile.Header.IdentityCode))
                    {
                        uniqueSignals[signalProfile.Header.IdentityCode].Add(new AnalyticsOwnerItem(strategyProfile.Key, AnalyticsTypeOption.Strategy));
                    }
                    else
                    {
                        List<AnalyticsOwnerItem> ownerStrategy= new List<AnalyticsOwnerItem>();
                        ownerStrategy.Add(new AnalyticsOwnerItem(strategyProfile.Key, AnalyticsTypeOption.Strategy));
                        uniqueSignals.Add(signalProfile.Header.IdentityCode, ownerStrategy);
                    }

                    //Signal level indicators
                    foreach (KeyValuePair<string, IndicatorCache> indicator in signalProfile.Indicators)
                    {
                        if (uniqueIndicators.ContainsKey(indicator.Key))
                        {
                            uniqueIndicators[indicator.Key].Add(new AnalyticsOwnerItem(signalProfile.Header.IdentityCode, AnalyticsTypeOption.Signal));
                        }
                        else
                        {
                            List<AnalyticsOwnerItem> ownerSignal= new List<AnalyticsOwnerItem>();
                            ownerSignal.Add(new AnalyticsOwnerItem(signalProfile.Header.IdentityCode, AnalyticsTypeOption.Signal));
                            uniqueSignals.Add(signalProfile.Header.IdentityCode, ownerSignal);
                        }
                    }

                    //TODO:  Do the same for Alerts
                }

                //Strategy level indicators
                foreach (KeyValuePair<string, IndicatorCache> indicator in strategyProfile.Value.Indicators)
                {
                    if (uniqueIndicators.ContainsKey(indicator.Key))
                    {
                        uniqueIndicators[indicator.Key].Add(new AnalyticsOwnerItem(strategyProfile.Value.Header.IdentityCode, AnalyticsTypeOption.Strategy));
                    }
                    else
                    {
                        List<AnalyticsOwnerItem> ownerStrategy = new List<AnalyticsOwnerItem>();
                        ownerStrategy.Add(new AnalyticsOwnerItem(strategyProfile.Value.Header.IdentityCode, AnalyticsTypeOption.Strategy));
                        uniqueSignals.Add(strategyProfile.Value.Header.IdentityCode, ownerStrategy);
                    }
                }

                //TODO:  Do the same for Alerts
            }

            timeKeyedStrategies = this.strategyCache.SelectAllStrategies();
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

            return signals.Count > 0 ? signals.ToArray() : null;
        }

        private StrategyDataItem[] GetStrategies(DateTime startDate, DateTime endDate)
        {
            List<StrategyDataItem> strategies = new List<StrategyDataItem>();
            foreach (KeyValuePair<DateTime, StrategyDataItem[]> signalValuePair in this.timeKeyedStrategies)
            {
                if (signalValuePair.Key >= startDate && signalValuePair.Key <= endDate)
                {
                    strategies.AddRange(signalValuePair.Value);
                }
            }

            return strategies.Count > 0 ? strategies.ToArray() : null;
        }

        public StrategyDataFrame GetStrategyDataFrame(DateTime targetDate, int frameSize)
        {
            strategyDataFrame.Clear();

            BarItem[] bars = pricebarCache.SelectBars(targetDate, frameSize);

            DateTime frameStartDate = bars[0].Time;
            DateTime frameEndDate = bars[bars.Length - 1].Time;

            StrategyDataItem[] strategyDataItems = this.GetStrategies(frameStartDate, frameEndDate);
            Dictionary<string, Dictionary<int, IndicatorDataItem>> indicatorDataItems = new Dictionary<string, Dictionary<int, IndicatorDataItem>>();

            foreach (KeyValuePair<string, IndicatorCache> indicator in indicatorCacheList)
            {
                indicatorDataItems.Add(indicator.Key, indicator.Value.SelectIndicators(frameStartDate, frameEndDate, frameSize));
            }
            int signalOffset = 0;
            foreach (BarItem bar in bars)
            {
                List<StrategyDataItem> signals = FindStrategyDataItem(strategyDataItems, bar.Time, ref signalOffset);
                Dictionary<string, IndicatorDataItem> indicators = new Dictionary<string, IndicatorDataItem>();
                Dictionary<string, int> indicatorOffsets = new Dictionary<string, int>();
                foreach (KeyValuePair<string, IndicatorCache> indicator in indicatorCacheList)
                {
                    indicatorOffsets.Add(indicator.Key, 0);
                }
                foreach (KeyValuePair<string, IndicatorCache> indicator in indicatorCacheList)
                {
                    int indicatorOffset = indicatorOffsets[indicator.Key];
                    IndicatorDataItem indicatorDataItem = FindIndicatorDataItem(indicatorDataItems[indicator.Key], bar.Time, ref indicatorOffset);
                    if (indicatorDataItem != null)
                    {
                        indicators.Add(indicator.Key, indicatorDataItem);
                    }
                    indicatorOffsets[indicator.Key] = indicatorOffset;
                }

                //strategyDataFrame.Add(bar.Time, new StrategyDataFrameItem(bar, signals, indicators));
            }

            return strategyDataFrame;
        }

        private IndicatorDataItem FindIndicatorDataItem(Dictionary<int, IndicatorDataItem> indicatorDataItems, DateTime targetDate, ref int offset)
        {
            IndicatorDataItem result = null;
            for (int index = offset + 1; index < indicatorDataItems.Count; index++)
            {
                if (indicatorDataItems[index].Time == targetDate)
                {
                    result = indicatorDataItems[index];
                }
                else if (indicatorDataItems[index].Time > targetDate)
                {
                    break;
                }
                offset = index;
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

            return result.Count > 0 ? result : null;
        }

        private List<StrategyDataItem> FindStrategyDataItem(StrategyDataItem[] strategyDataItems, DateTime targetDate, ref int offset)
        {
            List<StrategyDataItem> result = new List<StrategyDataItem>();
            if (strategyDataItems != null)
            {
                for (int index = offset + 1; index < strategyDataItems.Length; index++)
                {
                    if (strategyDataItems[index].ClosingBarTime == targetDate)
                    {
                        result.Add(strategyDataItems[index]);
                    }
                    else if (strategyDataItems[index].ClosingBarTime > targetDate)
                    {
                        break;
                    }
                    offset = index;
                }
            }

            return result.Count > 0 ? result : null;
        }

        private Dictionary<string, IndicatorCache> LoadIndicatorCacheList(string indicatorIdentityCodes)
        {
            string[] indicatorIdentities = indicatorIdentityCodes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Dictionary<string, IndicatorCache> cacheList = new Dictionary<string, IndicatorCache>();

            foreach (string indicatorIdentity in indicatorIdentities)
            {
                IndicatorCache indicatorCache = new IndicatorCache(indicatorIdentity, barType, cacheId);
                cacheList.Add(indicatorIdentity, indicatorCache);
            }

            return cacheList;
        }

        private Dictionary<string, SignalCache> LoadSignalCacheList(string signalIdentityCodes)
        {
            string[] signalIdentities = signalIdentityCodes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Dictionary<string, SignalCache> cacheList = new Dictionary<string, SignalCache>();

            foreach (string signalIdentity in signalIdentities)
            {
                SignalCache signalCache = new SignalCache(signalIdentity, barType, cacheId);
                cacheList.Add(signalIdentity, signalCache);
            }

            return cacheList;
        }
    }
}
