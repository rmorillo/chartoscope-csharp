using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Brokers
{
    public interface IDataFeederEvents
    {
        event DataFeederDelegates.NewBarHandler NewBar;
        event DataFeederDelegates.BarGapHandler BarGap;
    }
}
