using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Common
{
    public class NumericBytePool : LookBehindPool<byte>
    {
        public NumericBytePool(int capacity) : base(capacity)
        {
        }

        public void Write(byte value)
        {
            WriteCopy(value);
        }

        protected override byte CreatePoolItem()
        {
            return byte.MinValue;
        }
    }
}
