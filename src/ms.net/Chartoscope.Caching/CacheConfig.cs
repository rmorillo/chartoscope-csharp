using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class CacheConfig: ICacheConfigAppender, ICacheConfigNavigator, IDisposable
    {    
        private const string CACHE_CONFIG_FILE = "cache";

        public string cacheFolder = null;

        public CacheConfig(string cacheFolder)
        {
            this.cacheFolder = cacheFolder;                              
        }

        private ICacheWriter cacheWriter = null;
        private ICacheReader cacheReader = null;

        private CacheHeaderInfo header = null;

        public void Initialize()
        {
            header= GetHeader();

            cacheWriter = SharedCacheFactory.Instance.CacheWriter;
            cacheWriter.RegisterHeader(header);

            cacheReader = SharedCacheFactory.Instance.CacheReader;
            cacheReader.RegisterHeader(header);
        }

        public CacheHeaderInfo GetHeader()
        {
            List<CacheColumn> columnList = GetConfigColumns();

            return new CacheHeaderInfo(CACHE_CONFIG_FILE, null, Guid.Empty, CacheTypeOption.Configuration, columnList.ToArray());          
        }

        private List<CacheColumn> GetConfigColumns()
        {
            List<CacheColumn> columns = new List<CacheColumn>();

            columns.Add(new CacheColumn(0,"DatafeedHashKey", CacheDataTypeOption.Long, 8));
            columns.Add(new CacheColumn(1,"SessionId", CacheDataTypeOption.Guid, 16));
            columns.Add(new CacheColumn(2,"SessionType", CacheDataTypeOption.Integer, 4));

            return columns;
        }      

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            if (activeCachingMode == CachingModeOption.Reading)
            {
                cacheReader.EndReading(CACHE_CONFIG_FILE);
            }
            else if (activeCachingMode == CachingModeOption.Writing)
            {
                cacheWriter.EndWriting(CACHE_CONFIG_FILE);
            }        
        }

        private CachingModeOption activeCachingMode;

        public void Open(CachingModeOption cachingMode)
        {
            if (cachingMode == CachingModeOption.Reading)
            {
                activeCachingMode = cachingMode;
                cacheReader.BeginReading(CACHE_CONFIG_FILE);
            }
            else if (cachingMode == CachingModeOption.Writing)
            {
                activeCachingMode = cachingMode;
                cacheWriter.BeginWriting(CACHE_CONFIG_FILE);
            }           
        }

        public void Append(long datafeedHashKey, Guid sessionId, SessionModeOption sessionMode)
        {
            cacheWriter.Write(CACHE_CONFIG_FILE, datafeedHashKey, sessionId, sessionMode);
        }

        public void Append(CacheConfigItem item)
        {
            this.Append(item.DatafeedHashKey, item.SessionId, item.SessionMode);
        }
        
        public CacheRow Read(long datafeedHashKey)
        {
            CacheRow cacheRow = cacheReader.Find(CACHE_CONFIG_FILE, datafeedHashKey.ToString());            
            return cacheRow;
        }
    }
}
