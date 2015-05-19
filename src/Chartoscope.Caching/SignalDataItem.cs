using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class SignalDataItem
    {
        private DateTime closingBarTime;

        public DateTime ClosingBarTime
        {
            get { return closingBarTime; }
        }

        private DateTime signalTime;

        public DateTime SignalTime
        {
            get { return signalTime; }
        }

        private PositionMode position;

        public PositionMode Position
        {
            get { return position; }
        }

        private long sequence;

        public long Sequence
        {
            get { return sequence; }
        }
        
        public SignalDataItem(long sequence, DateTime closingBarTime, PositionMode position, DateTime signalTime)
        {
            this.sequence = sequence;
            this.closingBarTime = closingBarTime;
            this.position = position;
            this.signalTime = signalTime;
        }

    }
}
