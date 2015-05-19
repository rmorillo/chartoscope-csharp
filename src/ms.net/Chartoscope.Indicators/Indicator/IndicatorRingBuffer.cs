using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class IndicatorRingBuffer<T>: RingBufferBase<DateTime, T>
    {
        public IndicatorRingBuffer(KeyedCollection<DateTime, T> buffer, int length)
            : base(buffer, length)
        {
        }
    }
}
