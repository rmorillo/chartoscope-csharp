using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class StrategyCacheProfile
    {
        private CacheHeaderInfo header;

        public CacheHeaderInfo Header
        {
            get { return header; }
        }

        private Dictionary<string, CacheHeaderInfo> alerts;

        public Dictionary<string, CacheHeaderInfo> Alerts
        {
            get { return alerts; }
        }

        private StrategyCache strategyCache = null;

        private Dictionary<string, IndicatorCache> indicators = null;

        public Dictionary<string, IndicatorCache> Indicators { get { return indicators; } }

        public SignalCacheProfile[] Signals { get { return signalCacheProfiles.Values.ToArray<SignalCacheProfile>(); } }

        private Dictionary<string, SignalCacheProfile> signalCacheProfiles = null;

        public StrategyCacheProfile(string strategyIdentity, BarItemType barType, Guid cacheId)
        {
            this.strategyCache = new StrategyCache(strategyIdentity, barType, cacheId);

            CacheHeaderInfo headerInfo = this.strategyCache.Header;

            foreach (KeyValuePair<string, string> extendedProperty in headerInfo.ExtendedProperties)
            {
                switch (extendedProperty.Key)
                {
                    case "Indicators":
                        indicators = CacheHelper.LoadIndicatorCacheList(extendedProperty.Value, barType, cacheId);
                        break;
                    case "Alerts":
                        break;
                    case "Signals":
                        foreach (KeyValuePair<string, string> keyValuePair in strategyCache.Header.ExtendedProperties)
                        {
                            switch (keyValuePair.Key)
                            {
                                case "Indicators":
                                    indicators = CacheHelper.LoadIndicatorCacheList(keyValuePair.Value, barType, cacheId);
                                    break;
                                case "Alerts":
                                    break;
                                case "Signals":
                                    string[] signalValues = keyValuePair.Value.Split(new string[] {Environment.NewLine}, StringSplitOptions.None);
                                    signalCacheProfiles = new Dictionary<string, SignalCacheProfile>();
                                    foreach (string signalIdentity in signalValues)
                                    {
                                        signalCacheProfiles.Add(signalIdentity, new SignalCacheProfile(signalIdentity, barType, cacheId));
                                    }
                                    break;

                            }
                        }
                        break;
                }
            }
        }
    }
}
