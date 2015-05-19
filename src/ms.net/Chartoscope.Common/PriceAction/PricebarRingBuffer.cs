using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class PricebarRingBuffer: RingBufferBase<DateTime, BarItem>
    {
        public PricebarRingBuffer(KeyedCollection<DateTime, BarItem> buffer, int length)
            : base(buffer, length)
        {
        }
    }
}
