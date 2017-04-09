using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Common
{
    public class TimestampPool : LookBehindPool<DateTime>
    {
        public TimestampPool(int capacity) : base(capacity)
        {
        }

        public void Write(DateTime value)
        {
            WriteCopy(value);
        }

        protected override DateTime CreatePoolItem()
        {
            return DateTime.MinValue;
        }
    }
}
