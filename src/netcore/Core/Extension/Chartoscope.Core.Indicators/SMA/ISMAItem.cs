using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public interface ISMAItem
    {
        long Timestamp { get; }
        double Value { get; }
    }
}
