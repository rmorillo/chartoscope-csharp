using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Indicators
{
    public struct SumItem
    {
        private double sum;
        public double Sum { get { return sum; } }

        private DateTime timestamp;
        public DateTime Timestamp { get { return timestamp; } }

        public SumItem(double sum, DateTime timestamp)
        {
            this.sum = sum;
            this.timestamp = timestamp;
        }

        public void Set(double sum, DateTime timestamp)
        {
            this.sum = sum;
            this.timestamp = timestamp;
        }
    }
}
