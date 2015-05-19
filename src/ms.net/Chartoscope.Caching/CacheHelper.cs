using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class CacheHelper
    {
        public static Dictionary<string, IndicatorCache> LoadIndicatorCacheList(string indicatorIdentityCodes, BarItemType barType, Guid cacheId)
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

        public static Dictionary<int, IndicatorChartingInfo> LoadIndicatorChartingInfo(CacheHeaderInfo headerInfo)
        {
            Dictionary<int, IndicatorChartingInfo> indicatorChartingInfo = new Dictionary<int, IndicatorChartingInfo>();
            int index = 0;

            foreach (CacheColumn columnInfo in headerInfo.Columns.Values)
            {
                if (columnInfo.ExtendedProperties != null)
                {
                    indicatorChartingInfo.Add(index, new IndicatorChartingInfo(index, headerInfo.IdentityCode, columnInfo.Name
                            , (ChartRangeOption)Enum.Parse(typeof(ChartRangeOption), columnInfo.ExtendedProperties["ChartRange"])
                            , (ChartTypeOption)Enum.Parse(typeof(ChartTypeOption), columnInfo.ExtendedProperties["ChartType"]), headerInfo.Columns.Count > 2));
                    index++;
                }
            }

            return indicatorChartingInfo.Count > 0 ? indicatorChartingInfo : null;
        }
    }
}
