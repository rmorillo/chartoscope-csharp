using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class IndicatorCache : TimeKeyedCacheBase
    {
        private object indicatorInstance = null;

        private IndicatorPlotting[] charting;
        public IndicatorPlotting[] Charting
        {
            get { return charting; }
        }

        private string identityCode = null;
        private BarItemType barType = null;
        private Guid cacheId = Guid.Empty;
        private CacheModeOption cacheMode = CacheModeOption.ReadWrite;

        public IndicatorCache(object instance, IndicatorPlotting[] chartPlotting, Guid cacheId, bool continousWriteMode = false): base(CacheModeOption.Write)
        {
            this.indicatorInstance = instance;
            this.cacheId = cacheId;
            this.charting = chartPlotting;
        }

        public IndicatorCache(string identityCode, BarItemType barType, Guid cacheId)
            : base(CacheModeOption.Read)
        {
            this.identityCode = identityCode;
            this.barType = barType;
            this.cacheId = cacheId;
            this.cacheMode = CacheModeOption.Read;
           
            this.Initialize(identityCode, barType, cacheId, CacheTypeOption.Indicator);
            this.cacheIndex = this.CreateIndex();
            this.charting = CreateIndicatorCharting();
        }

        public override CacheHeaderInfo GetHeader()
        {
            IIndicatorCore core = indicatorInstance as IIndicatorCore;           
            List<CacheColumn> columnList = GetCacheColumns(indicatorInstance);
            return new CacheHeaderInfo(core.IdentityCode, core.BarType.Code, cacheId, CacheTypeOption.Indicator, columnList.ToArray());
        }

        private List<CacheColumn> GetCacheColumns(object instance)
        {
            List<CacheColumn> columns = new List<CacheColumn>();

            columns.Add(new CacheColumn(0, "Time", CacheDataTypeOption.Long, 8));

            int index = 1;
            foreach (IndicatorPlotting indicatorPlotting in charting)
            {
                Dictionary<string, string> extendedProperties = new Dictionary<string, string>();
                extendedProperties.Add("ChartType", Enum.GetName(typeof(ChartTypeOption), indicatorPlotting.ChartType));
                extendedProperties.Add("ChartRange", Enum.GetName(typeof(ChartRangeOption), indicatorPlotting.ChartRange));
                columns.Add(new CacheColumn(index, indicatorPlotting.Label, CacheDataTypeOption.Double, 8, extendedProperties));
                index++;
            }
            
            return columns;
        }

        public Dictionary<long, IndicatorDataItem> SelectIndicators(DateTime startDate, DateTime endDate, int frameSize)
        {
            long closestStartRowNumber = ClosestLowerTargetRowNumber(startDate); 
            long closestEndRowNumber = ClosestUpperTargetRowNumber(endDate);

            if (closestEndRowNumber - closestStartRowNumber < frameSize)
            {
                closestEndRowNumber = closestStartRowNumber + frameSize;
            }

            CacheRow[] cacheRows = this.Select(closestStartRowNumber, closestEndRowNumber - closestStartRowNumber);

            Dictionary<long, IndicatorDataItem> indicator = null;

            if (cacheRows == null)
            {
                return null;
            }
            else
            {
                indicator = new Dictionary<long, IndicatorDataItem>();

                foreach (CacheRow cacheRow in cacheRows)
                {
                    long dateTimeValue = (long)cacheRow[0];
                    DateTime key = new DateTime(dateTimeValue);
                    if (key >= startDate && key <= endDate)
                    {
                        Dictionary<string, double> indicatorValues = new Dictionary<string, double>();
                        foreach(IndicatorPlotting plotting in this.charting)
                        {
                            indicatorValues.Add(plotting.Label, (double)cacheRow[plotting.Label]);
                        }
                        indicator.Add(dateTimeValue, new IndicatorDataItem(key, indicatorValues));
                    }
                }
            }

            return indicator;
        }

        private IndicatorPlotting[] CreateIndicatorCharting()
        {
            List<IndicatorPlotting> plottingList = new List<IndicatorPlotting>();
            foreach (CacheColumn columnInfo in this.Header.Columns.Values)
            {
                if (columnInfo.ExtendedProperties != null)
                {
                    plottingList.Add(new IndicatorPlotting(columnInfo.Name
                            , (ChartTypeOption)Enum.Parse(typeof(ChartTypeOption), columnInfo.ExtendedProperties["ChartType"])
                            , (ChartRangeOption)Enum.Parse(typeof(ChartRangeOption), columnInfo.ExtendedProperties["ChartRange"])));
                }
            }

            return plottingList.Count > 0 ? plottingList.ToArray() : null;
            
        }
    }
}
