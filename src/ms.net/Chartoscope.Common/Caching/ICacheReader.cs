using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ICacheReader
    {
        void RegisterHeader(CacheHeaderInfo header);
        CacheHeaderInfo RequestHeader(string identityCode, BarItemType barType, Guid cacheId, CacheTypeOption cacheType);
        void BeginReading(string identityCode);
        CacheRow[] Select(string identityCode, long rowNumber, long rows = 1);
        CacheRow[] Next(string identityCode, long rows = 1);
        CacheRow NextItem(string identityCode);
        long GetRowCount(string identityCode);
        CacheRow Find(string identityCode, string searchKey);
        void EndReading(string identityCode);
    }
}
