using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Droidworks.Common;

namespace Droidworks.Indicators
{
    public class SingleValueIndicatorRingBuffer: RingBufferBase<DateTime, SingleValueIndicator>
    {
        public SingleValueIndicatorRingBuffer(KeyedCollection<DateTime, SingleValueIndicator> buffer, int length)
            : base(buffer, length)
        {
        }
    }
}
