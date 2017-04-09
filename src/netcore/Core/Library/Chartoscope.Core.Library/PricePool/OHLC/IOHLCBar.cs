using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IOHLCBar: ITimestamp
    {        
        double Open { get; }
        double High { get; }
        double Low { get; }
        double Close { get; }        
    }
}
