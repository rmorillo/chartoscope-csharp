using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public class MACDItem : IMACDValue
    {
        public ISMAItem FastEMA { get; private set; }
        
        public double SignalLine { get; private set; }

        public ISMAItem SlowEMA { get; private set; }

        public long Timestamp { get; private set; }
    }
}
