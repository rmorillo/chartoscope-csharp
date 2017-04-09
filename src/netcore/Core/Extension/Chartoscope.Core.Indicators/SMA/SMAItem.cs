using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public class SMAItem : ISMAItem
    {
        public SMAItem(long timestamp, double value)
        {
            Update(timestamp, value);
        }

        public void Update(long timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }

        public long Timestamp { get; private set; }

        public double Value { get; private set; }
    }
}
