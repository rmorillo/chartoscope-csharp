using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class ChartSignalItem
    {
        private SignalState signalState;

        public SignalState SignalState
        {
            get { return signalState; }
        }

        private DateTime time;

        public DateTime Time
        {
            get { return time; }     
        }

        public ChartSignalItem(SignalState signalState, DateTime time)
        {
            this.signalState = signalState;
            this.time = time;
        }
    }
}
