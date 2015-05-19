using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class StrategyDataItem
    {
        private DateTime closingBarTime;

        public DateTime ClosingBarTime
        {
            get { return closingBarTime; }
        }

        private DateTime executionTime;

        public DateTime ExecutionTime
        {
            get { return executionTime; }
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

        private bool hasOrderUpdate= false;

        public bool HasOrderUpdate
        {
            get { return hasOrderUpdate; }
        }


        public StrategyDataItem(long sequence, DateTime closingBarTime, PositionMode position, DateTime executionTime)
        {
            this.sequence = sequence;
            this.closingBarTime = closingBarTime;
            this.position = position;
            this.executionTime = executionTime;
        }

    }
}
