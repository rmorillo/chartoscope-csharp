using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class SharedCacheFactory
    {
        public static readonly SharedCacheFactory Instance = new SharedCacheFactory();

        private ICacheWriter cacheWriter;

        public ICacheWriter CacheWriter
        {
            get { return cacheWriter; }
            set { cacheWriter = value; }
        }

        private ICacheReader cacheReader;

        public ICacheReader CacheReader
        {
            get { return cacheReader; }
            set { cacheReader = value; }
        }

        private SharedCacheFactory()
        {
        }

        public TimeKeyedCaching CreateTimeKeyedCache(CacheHeaderInfo header)
        {
            cacheWriter.RegisterHeader(header);
            return new TimeKeyedCaching(header, cacheWriter, cacheReader);
        }

        public TimeKeyedCaching LoadTimeKeyedCache(string identityCode, BarItemType barType, Guid cacheId, CacheTypeOption cacheType)
        {            
            CacheHeaderInfo header= cacheReader.RequestHeader(identityCode, barType, cacheId, cacheType);
            cacheReader.RegisterHeader(header);
            return new TimeKeyedCaching(header, cacheWriter, cacheReader);
        }

        public SequenceKeyedCaching CreateSequenceKeyedCache(CacheHeaderInfo header)
        {
            cacheWriter.RegisterHeader(header);
            return new SequenceKeyedCaching(header, cacheWriter, cacheReader);
        }

        public SequenceKeyedCaching LoadSequenceKeyedCache(string identityCode, BarItemType barType, Guid cacheId, CacheTypeOption cacheType)
        {
            CacheHeaderInfo header = cacheReader.RequestHeader(identityCode, barType, cacheId, cacheType);
            cacheReader.RegisterHeader(header);
            return new SequenceKeyedCaching(header, cacheWriter, cacheReader);
        }  
    }
}
