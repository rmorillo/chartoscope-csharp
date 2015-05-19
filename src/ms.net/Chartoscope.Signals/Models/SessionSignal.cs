using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Caching;
using Chartoscope.Common;

namespace Chartoscope.Signals
{
    public class SessionSignal
    {
        public BarItemType BarType { get; set; }
        public Dictionary<string, CoreSignal> CoreSignals { get; set; }

        public SessionSignal(BarItemType barItemType)
        {
            this.BarType = barItemType;
            this.CoreSignals = new Dictionary<string, CoreSignal>();
        }
    }
}
