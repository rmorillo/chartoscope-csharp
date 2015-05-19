using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class ChartIndicatorItem
    {
        public DateTime Time { get; set; }
        public double Value { get; set; }
        public double[] Values { get; set; }

        public ChartIndicatorItem(DateTime time, double value)
        {
            this.Time = time;
            this.Value = value;
        }
    }
}
