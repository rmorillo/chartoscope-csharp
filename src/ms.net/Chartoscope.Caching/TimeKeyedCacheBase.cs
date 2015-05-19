using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public abstract class TimeKeyedCacheBase: ITimeKeyedCacheAppender, IDisposable
    {
        private TimeKeyedCaching cache = null;

        private CacheModeOption cacheMode = CacheModeOption.ReadWrite;

        public CacheHeaderInfo Header { get { return cache.Header==null? GetHeader(): cache.Header; } }

        protected Dictionary<DateTime, long> cacheIndex = null;

        public TimeKeyedCacheBase(CacheModeOption cacheMode)
        {
            this.cacheMode = cacheMode;                               
        }

        public bool Indexed { get { return cacheIndex != null && cacheIndex.Count > 0; } }

        public void Initialize()
        {
            if (cacheMode == CacheModeOption.Write)
            {
                cache = SharedCacheFactory.Instance.CreateTimeKeyedCache(GetHeader());
            }           
        }

        public void Initialize(string identityCode, BarItemType barType, Guid cacheId, CacheTypeOption cacheType)
        {
            if (cacheMode == CacheModeOption.Read || cacheMode==CacheModeOption.ReadWrite)
            {
                cache = SharedCacheFactory.Instance.LoadTimeKeyedCache(identityCode, barType, cacheId, cacheType);
            }
        }

        protected long rowsPerIndex;

        public long RowsPerIndex
        {
            get { return rowsPerIndex; }
            set { rowsPerIndex = value; }
        }

        private long lastRowCount;

        public long LastRowCount
        {
            get { return lastRowCount; }
            set { lastRowCount = value; }
        }


        public Dictionary<DateTime, long> CreateIndex()
        {
            cache.OpenForReading();

            lastRowCount = cache.GetRowCount();
            
            rowsPerIndex = lastRowCount / 1000;

            Dictionary<DateTime, long> cacheIndex = new Dictionary<DateTime, long>();

            if (rowsPerIndex < 10)
            {
                this.rowsPerIndex = 1;
                CacheRow cacheRow = cache.Select(0)[0];
                cacheIndex.Add(new DateTime((long)cacheRow[0]), cacheRow.RowNumber);
                cacheRow = cache.NextItem();
                while (cacheRow != null)
                {                    
                    cacheIndex.Add(new DateTime((long)cacheRow[0]), cacheRow.RowNumber);
                    cacheRow = cache.NextItem();
                }     
            }
            else
            {                

                for (int indexCount = 0; indexCount < rowsPerIndex; indexCount++)
                {
                    CacheRow[] cacheRow = cache.Select(indexCount * rowsPerIndex);
                    if (cacheRow!=null)
                    {
                        cacheIndex.Add(new DateTime((long)cacheRow[0][0]), cacheRow[0].RowNumber);
                    }
                }
            }

            return cacheIndex.Count > 0 ? cacheIndex : null;  
        }

        public virtual CacheHeaderInfo GetHeader()
        {
            return null;
        }

        public void Append(DateTime time, params object[] values)
        {            
            cache.Append(time, values);
        }

        public void Dispose()
        {
            cache.Close();
        }

        public void Close()
        {
            cache.Close();
        }

        public CacheRow[] Select(long rowNumber, long rows=1)
        {
            return cache.Select(rowNumber, rows);
        }

        public CacheRow[] Next(long rows = 1)
        {
            return cache.Next(rows);
        }

        public CacheRow NextItem()
        {
            return cache.NextItem();
        }

        public void Open(CachingModeOption cachingMode)
        {
            if (cacheMode == CacheModeOption.Write)
            {
                cache.OpenForWriting();
            }
            else
            {
                cache.OpenForReading();
            }
        }

        public void FindStartEndIndexes(CacheRow[] cacheRows, DateTime targetDate, int frameSize, ref int startIndex, ref int endIndex)
        {
            int targetIndex = 0;
            int index = 0;
            foreach (CacheRow cacheRow in cacheRows)
            {
                DateTime keyDate = new DateTime((long)cacheRow[0]);
                if (keyDate >= targetDate)
                {
                    targetIndex = index;
                    break;
                }
                index++;
            }

            int halfFrameSize = frameSize / 2;
            int offset = targetIndex - halfFrameSize;
            startIndex = offset > 0 ? offset : 0;
            endIndex = startIndex == 0 ? frameSize : startIndex + frameSize - 1;
            if (endIndex > cacheRows.Length - 1)
            {
                endIndex = cacheRows.Length - 1;
                startIndex = endIndex - frameSize < 0 ? 0 : endIndex - frameSize;
            }
        }

        protected long ClosestLowerTargetRowNumber(DateTime targetDate)
        {
            DateTime lastDate = DateTime.MinValue;
            long lastRowNumber = 0;

            if (cacheIndex != null)
            {
                foreach (KeyValuePair<DateTime, long> priceBarIndex in cacheIndex)
                {
                    if (priceBarIndex.Key >= targetDate)
                    {
                        break;
                    }

                    lastDate = priceBarIndex.Key;
                    lastRowNumber = priceBarIndex.Value;
                }
            }
            else
            {
                
               
            }

            return lastRowNumber;
        }

        protected long ClosestUpperTargetRowNumber(DateTime targetDate)
        {
            DateTime lastDate = DateTime.MinValue;
            long lastRowNumber = lastRowCount;

            if (cacheIndex != null)
            {
                foreach (KeyValuePair<DateTime, long> priceBarIndex in cacheIndex)
                {
                    lastDate = priceBarIndex.Key;
                    lastRowNumber = priceBarIndex.Value;
                    if (priceBarIndex.Key >= targetDate)
                    {
                        break;
                    }                    
                }
            }

            return lastRowNumber;
        }
    }
}
