using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Common
{
    public class NumericDoublePool : LookBehindPool<double>
    {
        public NumericDoublePool(int capacity) : base(capacity)
        {
        }

        public void Write(double value)
        {
            WriteCopy(value);
        }

        protected override double CreatePoolItem()
        {
            return double.NaN;
        }
    }
}
