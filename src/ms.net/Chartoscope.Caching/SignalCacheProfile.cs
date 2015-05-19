using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class SignalCacheProfile
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

        private SignalCache signalCache = null;

        private Dictionary<string, IndicatorCache> indicators=null;

        public Dictionary<string, IndicatorCache> Indicators { get { return this.indicators; } }

        public SignalCacheProfile(string signalIdentity, BarItemType barType, Guid cacheId)
        {
            this.signalCache = new SignalCache(signalIdentity, barType, cacheId);

            CacheHeaderInfo headerInfo = this.signalCache.Header;

            foreach (KeyValuePair<string, string> extendedProperty in headerInfo.ExtendedProperties)
            {
                switch (extendedProperty.Key)
                {
                    case "Indicators":
                        indicators = CacheHelper.LoadIndicatorCacheList(extendedProperty.Value, barType, cacheId);
                        break;
                    case "Alerts":
                        break;
                }
            }
        }
    }
}
