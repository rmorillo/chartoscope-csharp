using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class NumericDecimalPool : LookBehindPool<decimal>
    {
        public NumericDecimalPool(int capacity) : base(capacity)
        {
        }

        public void Write(decimal value)
        {
            WriteCopy(value);
        }

        protected override decimal CreatePoolItem()
        {
            return decimal.MinValue;
        }
    }
}
