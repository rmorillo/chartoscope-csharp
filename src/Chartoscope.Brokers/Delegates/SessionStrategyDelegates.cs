using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public class SessionStrategyDelegates
    {
        public delegate void EntryPositionHandler(long sequence, PositionMode position);
        public delegate void ExitPositionHandler(long sequence);
    }
}
