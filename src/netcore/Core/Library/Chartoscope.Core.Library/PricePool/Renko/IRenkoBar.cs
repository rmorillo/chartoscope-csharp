using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IRenkoBar: ITimestamp
    {      
        double Open { get; }
        double Close { get; }
    }
}
