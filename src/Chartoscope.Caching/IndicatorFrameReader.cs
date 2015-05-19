using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Caching
{
    public class IndicatorFrameReader
    {   
        internal IndicatorFrameReader(DateTime startDate, DateTime endDate, int frameSize, IndicatorCache cache)
        {
             dataFrameItems= cache.SelectIndicators(startDate, endDate, frameSize);
        }

        private Dictionary<long, IndicatorDataItem> dataFrameItems;

        public double Read(string seriesName, long time)
        {
            return dataFrameItems[time][seriesName];
        }
    }
}
