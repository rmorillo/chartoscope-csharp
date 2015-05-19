using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class CacheColumn
    {
        private string name;

        public string Name
        {
            get { return name; }
        }

        private CacheDataTypeOption dataType;

        public CacheDataTypeOption DataType
        {
            get { return dataType; }
        }

        private int size;

        public int Size
        {
            get { return size; }
        }

        private int index;

        public int Index
        {
            get { return index; }
        }

        private Dictionary<string, string> extendedProperties;

        public Dictionary<string, string> ExtendedProperties
        {
            get { return extendedProperties; }
        }        

        public CacheColumn(int index, string name, CacheDataTypeOption dataType, int size, Dictionary<string, string> extendedProperties=null)
        {
            this.index = index;
            this.name = name;
            this.dataType = dataType;
            this.size = size;
            this.extendedProperties = extendedProperties;
        }

    }
}
