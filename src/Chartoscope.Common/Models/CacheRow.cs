using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class CacheRow
    {
        public object this[int columnIndex]
        {
            get
            {
                return data[columnIndex];
            }
        }

        public object this[string columnName]
        {
            get
            {
                return data[columns[columnName].Index];
            }
        }

        private Dictionary<string, CacheColumn> columns = null;
        private object[] data = null;

        private long rowNumber;

        public long RowNumber
        {
            get { return rowNumber; }
        }


        public CacheRow(Dictionary<string, CacheColumn> columns, long rowNumber, object[] data)
        {
            this.columns = columns;
            this.data = data;
            this.rowNumber = rowNumber;
        }        
        
    }
}
