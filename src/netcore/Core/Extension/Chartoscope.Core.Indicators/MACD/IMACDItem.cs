using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public interface IMACDValue: ITimestamp
    {
        double SignalLine { get; }
        ISMAItem SlowEMA { get; }
        ISMAItem FastEMA { get; }      
    }
}
