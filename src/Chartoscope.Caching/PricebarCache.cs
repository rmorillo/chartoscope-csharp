using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class PricebarCache : TimeKeyedCacheBase
    {
        private BarItemType barType = null;
        public BarItemType BarType { get { return barType; } }

        private Guid cacheId = Guid.Empty;
        public Guid CacheId { get { return cacheId; } }

        private CacheModeOption cacheMode = CacheModeOption.ReadWrite;        

        public PricebarCache(BarItemType barType, Guid cacheId, CacheModeOption cacheMode= CacheModeOption.Write)
            : base(cacheMode)
        {
            this.barType = barType;
            this.cacheId = cacheId;
            this.cacheMode = CacheModeOption.Read;

            if (cacheMode == CacheModeOption.Read || cacheMode == CacheModeOption.ReadWrite)
            {
                this.Initialize("ohlc", barType, cacheId, CacheTypeOption.Pricebar);
                this.cacheIndex = CreateIndex();
            }
        }

        public override CacheHeaderInfo GetHeader()
        {
            List<CacheColumn> columnList = GetCacheColumns();
            return new CacheHeaderInfo("ohlc", barType.Code, cacheId, CacheTypeOption.Pricebar, columnList.ToArray());
        }

        private List<CacheColumn> GetCacheColumns()
        {
            List<CacheColumn> columns = new List<CacheColumn>();

            columns.Add(new CacheColumn(0, "Time", CacheDataTypeOption.Long, 8));
            columns.Add(new CacheColumn(1, "Open", CacheDataTypeOption.Double, 8));
            columns.Add(new CacheColumn(2, "Close", CacheDataTypeOption.Double, 8));
            columns.Add(new CacheColumn(3, "High", CacheDataTypeOption.Double, 8));
            columns.Add(new CacheColumn(4, "Low", CacheDataTypeOption.Double, 8));

            return columns;
        }        

        public BarItem[] SelectBars(DateTime targetDate, int frameSize)
        {
            BarItem[] bars = null;            

            if (this.Indexed)
            {                
                long closestRowNumber = ClosestLowerTargetRowNumber(targetDate);

                int halfFrame = frameSize / 2;
                long targetRow = closestRowNumber - halfFrame > 0 ? closestRowNumber - halfFrame : 0;
                long rowCount = targetRow + Math.Max(this.rowsPerIndex, frameSize) + halfFrame;

                CacheRow[] cacheRows = this.Select(targetRow, rowCount);

                int startIndex = 0;
                int endIndex = 0;
                FindStartEndIndexes(cacheRows, targetDate, frameSize, ref startIndex, ref endIndex);

                bars = new BarItem[endIndex - startIndex + 1];
                int barIndex = 0;
                for (int filterIndex = startIndex; filterIndex <= endIndex; filterIndex++)
                {
                    CacheRow cacheRow = cacheRows[filterIndex];
                    bars[barIndex] = new BarItem(new DateTime((long)cacheRow[0]), (double)cacheRow[1], (double)cacheRow[2], (double)cacheRow[3], (double)cacheRow[4]);
                    barIndex++;
                }
            }

            return bars;
        }

        private DateTime startBarDate = DateTime.MinValue;

        public DateTime StartBarDate
        {
            get
            {
                if (startBarDate == DateTime.MinValue)
                {
                    startBarDate = new DateTime((long)Select(0)[0][0]);
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
                    endBarDate = new DateTime((long)Select(LastRowCount)[0][0]);
                }
                return endBarDate;
            }
        }
    }
}
