using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Common
{
    public class NumericInt32Pool : LookBehindPool<int>
    {
        public NumericInt32Pool(int capacity) : base(capacity)
        {
        }

        public void Write(int value)
        {
            WriteCopy(value);
        }

        protected override int CreatePoolItem()
        {
            return int.MinValue;
        }
    }
}
