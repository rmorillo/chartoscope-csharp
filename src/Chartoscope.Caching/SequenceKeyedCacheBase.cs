using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public abstract class SequenceKeyedCacheBase : ISequenceKeyedCacheAppender, IDisposable
    {
        private SequenceKeyedCaching cache = null;

        private CacheModeOption cacheMode = CacheModeOption.ReadWrite;

        public CacheHeaderInfo Header { get { return cache.Header == null ? GetHeader() : cache.Header; } }

        protected Dictionary<long, long> cacheIndex = null;

        public SequenceKeyedCacheBase(CacheModeOption cacheMode)
        {
            this.cacheMode = cacheMode;
        }

        public bool Indexed { get { return cacheIndex != null && cacheIndex.Count > 0; } }

        public void Initialize()
        {
            if (cacheMode == CacheModeOption.Write)
            {
                cache = SharedCacheFactory.Instance.CreateSequenceKeyedCache(GetHeader());
            }
        }

        public void Initialize(string identityCode, BarItemType barType, Guid cacheId, CacheTypeOption cacheType)
        {
            if (cacheMode == CacheModeOption.Read || cacheMode == CacheModeOption.ReadWrite)
            {
                cache = SharedCacheFactory.Instance.LoadSequenceKeyedCache(identityCode, barType, cacheId, cacheType);
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


        public Dictionary<long, long> CreateIndex()
        {
            cache.OpenForReading();

            lastRowCount = cache.GetRowCount();

            rowsPerIndex = lastRowCount / 1000;

            Dictionary<long, long> cacheIndex = new Dictionary<long, long>();

            if (rowsPerIndex < 10)
            {
                CacheRow cacheRow = cache.Select(0)[0];
                cacheIndex.Add((long)cacheRow[0], cacheRow.RowNumber);
                cacheRow = cache.NextItem();
                while (cacheRow != null)
                {
                    cacheIndex.Add((long)cacheRow[0], cacheRow.RowNumber);
                    cacheRow = cache.NextItem();
                }
            }
            else
            {

                for (int indexCount = 0; indexCount < rowsPerIndex; indexCount++)
                {
                    CacheRow[] cacheRow = cache.Select(indexCount * rowsPerIndex);
                    if (cacheRow != null)
                    {
                        cacheIndex.Add((long)cacheRow[0][0], cacheRow[0].RowNumber);
                    }
                }
            }

            return cacheIndex.Count > 0 ? cacheIndex : null;
        }

        public virtual CacheHeaderInfo GetHeader()
        {
            return null;
        }

        public void Append(long sequence, params object[] values)
        {
            cache.Append(sequence, values);
        }

        public void Dispose()
        {
            cache.Close();
        }

        public void Close()
        {
            cache.Close();
        }

        public CacheRow[] Select(long rowNumber, long rows = 1)
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

        public void FindStartEndIndexes(CacheRow[] cacheRows, long targetSequence, int frameSize, ref int startIndex, ref int endIndex)
        {
            int targetIndex = 0;
            int index = 0;
            foreach (CacheRow cacheRow in cacheRows)
            {
                long keySequence = (long)cacheRow[0];
                if (keySequence >= targetSequence)
                {
                    targetIndex = index;
                    break;
                }
                index++;
            }

            int halfFrameSize = frameSize / 2;
            int offset = targetIndex - halfFrameSize;
            startIndex = offset > 0 ? offset : 0;
            endIndex = startIndex == 0 ? frameSize : startIndex + frameSize;
            if (endIndex > cacheRows.Length - 1)
            {
                endIndex = cacheRows.Length - 1;
                startIndex = endIndex - frameSize < 0 ? 0 : endIndex - frameSize;
            }
        }

        protected long ClosestLowerTargetRowNumber(long targetSequence)
        {
            long lastSequence = long.MinValue;
            long lastRowNumber = 0;

            if (cacheIndex != null)
            {
                foreach (KeyValuePair<long, long> valuePair in cacheIndex)
                {
                    if (valuePair.Key >= targetSequence)
                    {
                        break;
                    }

                    lastSequence = valuePair.Key;
                    lastRowNumber = valuePair.Value;
                }
            }

            return lastRowNumber;
        }

        protected long ClosestUpperTargetRowNumber(long targetSequence)
        {
            long lastSequence = long.MinValue;
            long lastRowNumber = lastRowCount;

            if (cacheIndex != null)
            {
                foreach (KeyValuePair<long, long> valueKey in cacheIndex)
                {
                    lastSequence = valueKey.Key;
                    lastRowNumber = valueKey.Value;
                    if (valueKey.Key >= targetSequence)
                    {
                        break;
                    }
                }
            }

            return lastRowNumber;
        }
    }
}
