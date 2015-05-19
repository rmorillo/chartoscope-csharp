using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public class SessionSignalDelegates
    {
        public delegate void EntrySignalHandler(long sequence, PositionMode position);
        public delegate void ExitSignalHandler(long sequence);
    }
}
