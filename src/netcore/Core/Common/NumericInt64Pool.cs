using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Common
{
    public class NumericInt64Pool : LookBehindPool<long>
    {
        public NumericInt64Pool(int capacity) : base(capacity)
        {
        }

        public void Write(long value)
        {
            WriteCopy(value);
        }

        protected override long CreatePoolItem()
        {
            return long.MinValue;
        }
    }
}
