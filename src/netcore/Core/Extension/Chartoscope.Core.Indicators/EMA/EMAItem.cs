using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public class EMAItem : IEMAItem, ITimestamp
    {
        public EMAItem()
        {
            Write(long.MinValue, double.NaN);
        }

        public EMAItem(long timestamp, double value)
        {
            Write(timestamp, value);
        }

        public void Write(long timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }

        public long Timestamp { get; private set; }

        public double Value { get; private set; }
    }
}
