using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class SignalDataFrame
    {
        private Dictionary<DateTime, SignalDataFrameItem> signalDataFrame = new Dictionary<DateTime,SignalDataFrameItem>();

        private CacheHeaderInfo[] indicatorHeaderInfo = null;
        private CacheHeaderInfo signalHeaderInfo = null;

        private Dictionary<int, IndicatorChartingInfo> indicatorChartingInfo = null;

        public SignalDataFrame(CacheHeaderInfo signalHeaderInfo, CacheHeaderInfo[] indicatorHeaderInfo)
        {
            this.signalHeaderInfo = signalHeaderInfo;
            this.indicatorHeaderInfo = indicatorHeaderInfo;

            indicatorChartingInfo = new Dictionary<int, IndicatorChartingInfo>();
            int index = 0;
            foreach (CacheHeaderInfo headerInfo in indicatorHeaderInfo)
            {
                foreach (CacheColumn columnInfo in headerInfo.Columns.Values)
                {
                    if (columnInfo.ExtendedProperties!=null)
                    {
                        indicatorChartingInfo.Add(index, new IndicatorChartingInfo(index, headerInfo.IdentityCode, columnInfo.Name
                                , (ChartRangeOption)Enum.Parse(typeof(ChartRangeOption), columnInfo.ExtendedProperties["ChartRange"])
                                , (ChartTypeOption)Enum.Parse(typeof(ChartTypeOption), columnInfo.ExtendedProperties["ChartType"]), headerInfo.Columns.Count>2));
                        index++;
                    }
                }
            }
        }

        public void Add(DateTime time, SignalDataFrameItem signalDataFrameItem)
        {
            signalDataFrame.Add(time, signalDataFrameItem);
        }

        public void Clear()
        {
            signalDataFrame.Clear();
        }

        public BarItem[] GetPriceBars()
        {
            BarItem[] priceBars= new BarItem[signalDataFrame.Count];
            int index = 0;
            foreach(SignalDataFrameItem item in signalDataFrame.Values)
            {
                priceBars[index] = item.PriceBar;
                index++;
            }

            return priceBars;
        }

        public SignalDataItem[] GetSignals()
        {
            List<SignalDataItem> signals = new List<SignalDataItem>();
            foreach (SignalDataFrameItem item in signalDataFrame.Values)
            {
                if (item.Signals != null)
                {
                    signals.AddRange(item.Signals);
                }
            }

            return signals.ToArray();
        }

        public ChartIndicatorItem[] GetIndicators(string identityCode, string seriesName)
        {
            List<ChartIndicatorItem> indicators = new List<ChartIndicatorItem>();

            foreach (SignalDataFrameItem item in signalDataFrame.Values)
            {
                if (item.Indicators.Count > 0 && item.Indicators.ContainsKey(identityCode))
                {
                    indicators.Add(new ChartIndicatorItem(item.PriceBar.Time, item.Indicators[identityCode].Values[seriesName]));
                }
            }

            return indicators.ToArray();
        }

        public List<IndicatorChartingInfo> GetIndicatorChartingInfo()
        {
            return new List<IndicatorChartingInfo>(indicatorChartingInfo.Values);
        }        
    }
}
