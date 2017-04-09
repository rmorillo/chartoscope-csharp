using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest
{
    public class PrimitiveTypePool: LookBehindPool<double>
    {
        public PrimitiveTypePool(int capacity) : base(capacity)
        {
        }

        public void Write(double value)
        {
            WriteCopy(value);
        }

        protected override double CreatePoolItem()
        {
            return int.MinValue;
        }
    }
}
