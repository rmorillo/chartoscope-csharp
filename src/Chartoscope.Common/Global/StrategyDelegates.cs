using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class StrategyDelegates
    {
        public delegate void EntryStrategyHandler(IStrategy source, long sequence, PositionMode position);
        public delegate void ExitStrategyHandler(IStrategy source, long sequence);
        public delegate void RegistrationHandler(object instance);
    }
}
