using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface IIndicatorItem: IBufferItemKey<DateTime>
    {
        double Value { get; }
        double[] Values { get; }
    }
}
