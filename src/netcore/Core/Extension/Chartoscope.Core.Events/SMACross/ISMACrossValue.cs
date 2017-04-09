using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Events
{
    public interface ISMACrossValue
    {
        long Timestamp { get; }
        EventStateOption Status { get; }
    }
}
