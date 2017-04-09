using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class NumericFloatPool : LookBehindPool<float>
    {
        public NumericFloatPool(int capacity) : base(capacity)
        {
        }

        public void Write(float value)
        {
            WriteCopy(value);
        }

        protected override float CreatePoolItem()
        {
            return float.NaN;
        }
    }
}
