using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class CacheHeaderInfo
    {
        private string identityCode;

        public string IdentityCode
        {
            get { return identityCode; }
        }

        private CacheTypeOption cacheType;

        public CacheTypeOption CacheType
        {
            get { return cacheType; }
        }

        private Dictionary<string, CacheColumn> columns;

        public Dictionary<string, CacheColumn> Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        private Guid cacheId;

        public Guid CacheId
        {
            get { return cacheId; }
        }

        private string subSection;

        public string SubSection
        {
            get { return subSection; }
            set { subSection = value; }
        }

        private Dictionary<string, string> extendedProperties;

        public Dictionary<string, string> ExtendedProperties
        {
            get { return extendedProperties; }
        }   

        public CacheHeaderInfo(string identityCode, string subSection, Guid cacheId, CacheTypeOption cacheType, CacheColumn[] columns, Dictionary<string, string> extendedProperties=null)
        {
            this.identityCode = identityCode;
            this.subSection = subSection;
            this.cacheId = cacheId;
            this.cacheType = cacheType;

            this.columns = new Dictionary<string, CacheColumn>();
            foreach (CacheColumn column in columns)
            {
                this.columns.Add(column.Name, column);
            }

            this.extendedProperties = extendedProperties;
        }       
    }
}
