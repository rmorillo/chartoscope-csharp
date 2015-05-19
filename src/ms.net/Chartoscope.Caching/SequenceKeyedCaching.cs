using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class SequenceKeyedCaching
    {
        private ICacheWriter cacheWriter = null;
        private ICacheReader cacheReader = null;
        private CacheHeaderInfo header = null;

        public CacheHeaderInfo Header { get { return header; } }

        public SequenceKeyedCaching(CacheHeaderInfo header, ICacheWriter cacheWriter, ICacheReader cacheReader)
        {
            this.header = header;
            this.cacheWriter = cacheWriter;
            this.cacheReader = cacheReader;
        }

        private bool writeMode = false;

        public bool OpenForWriting()
        {
            writeMode = true;
            cacheWriter.BeginWriting(header.IdentityCode);
            return true;
        }

        public bool OpenForReading()
        {
            writeMode = false;
            cacheReader.BeginReading(header.IdentityCode);
            return true;
        }

        private long lastReadRowNumber = 0;

        public CacheRow[] Select(long rowNumber, long rows = 1)
        {
            CacheRow[] cacheRows = cacheReader.Select(header.IdentityCode, rowNumber, rows);
            if (cacheRows == null)
            {
                return null;
            }
            else
            {
                lastReadRowNumber = rowNumber + cacheRows.Length - 1;
                return cacheRows;
            }
        }

        public CacheRow[] Next(long rows = 1)
        {
            return cacheReader.Next(header.IdentityCode, rows);
        }

        public CacheRow NextItem()
        {
            return cacheReader.NextItem(header.IdentityCode);
        }

        public long GetRowCount()
        {
            return cacheReader.GetRowCount(header.IdentityCode);
        }

        public void Append(long sequence, object[] values)
        {
            object[] data = new object[values.Length + 1];
            data[0] = sequence;
            int index = 1;

            foreach (object value in values)
            {
                data[index] = value;
                index++;
            }

            cacheWriter.Write(header.IdentityCode, data);
        }

        public void Close()
        {
            if (writeMode)
            {
                cacheWriter.EndWriting(header.IdentityCode);
                writeMode = false;
            }
        }
    }
}
