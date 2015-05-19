using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class SignalFrameReader
    {
        protected SignalFrameReader(DateTime startDate, DateTime endDate, int frameSize, SignalCache cache, BarItemType barType, Guid cacheId)
        {
            this.startDate = startDate;
            this.endDate = endDate;
            this.frameSize = frameSize;
            dataFrameItems= cache.SelectSignals(startDate, endDate, frameSize);

            int index= 0;
            DateTime lastClosingBarDate= DateTime.MinValue;
            foreach (SignalDataItem item in dataFrameItems)
            {
                if (lastClosingBarDate!=item.ClosingBarTime)
                {
                    signalIndex.Add(item.ClosingBarTime, index);
                    lastClosingBarDate = item.ClosingBarTime;
                }

                index++;
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

        private DateTime startDate = DateTime.MinValue;
        private DateTime endDate = DateTime.MinValue;
        private int frameSize = 0;

        public IndicatorFrameReader GetIndicatorFrameReader(string identityCode)
        {
            return new IndicatorFrameReader(startDate, endDate, frameSize, indicatorCacheList[identityCode]);
        }

        private Dictionary<string, IndicatorCache> indicatorCacheList = null;
        private Dictionary<DateTime, int> signalIndex = null;
        private SignalDataItem[] dataFrameItems= null;

        public SignalState Read(DateTime time)
        {
            SignalState signal= SignalState.NoSignal;
            if (signalIndex.ContainsKey(time))
            {
                int signalPosition = signalIndex[time];

                //Check if position is Closed
                if (dataFrameItems[signalPosition].Position == PositionMode.Closed)
                {
                    //Check for reversal
                    int nextPosition = signalPosition + 1;
                    if (nextPosition < dataFrameItems.Length && dataFrameItems[nextPosition].ClosingBarTime == time)
                    {
                        if (dataFrameItems[nextPosition].Position == PositionMode.Long)
                        {
                            signal = SignalState.LongReversal;
                        }
                        else if (dataFrameItems[nextPosition].Position == PositionMode.Short)
                        {
                            signal = SignalState.ShortReversal;
                        }
                    }
                }
                else
                {
                    signal = dataFrameItems[signalPosition].Position == PositionMode.Long ? SignalState.Long : SignalState.Short;
                }

            }
            return signal;
        }
    }
}
