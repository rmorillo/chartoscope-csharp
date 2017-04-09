using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class NumericInt16Pool : LookBehindPool<short>
    {
        public NumericInt16Pool(int capacity) : base(capacity)
        {
        }

        public void Write(short value)
        {
            WriteCopy(value);
        }

        protected override short CreatePoolItem()
        {
            return short.MinValue;
        }
    }
}
