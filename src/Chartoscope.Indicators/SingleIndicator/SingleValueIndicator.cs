using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Droidworks.Indicators
{
    public struct SingleValueIndicator
    {
        private double value;

        public double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private DateTime timestamp;

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public SingleValueIndicator(DateTime timestamp, double value)
        {
            this.value = value;
            this.timestamp = timestamp;
        }
        
    }
}
