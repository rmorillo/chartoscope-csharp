using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Droidworks.Common;

namespace Droidworks.Indicators
{
    public class MultiValueIndicatorRingBuffer : RingBufferBase<DateTime, MultiValueIndicator>
    {
        public MultiValueIndicatorRingBuffer(KeyedCollection<DateTime, MultiValueIndicator> buffer, int length)
            : base(buffer, length)
        {
        }
    }
}
