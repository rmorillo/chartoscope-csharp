using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Caching
{
    public class IndicatorDataItem
    {
        private Dictionary<string, double> values;

        public Dictionary<string, double> Values
        {
            get { return values; }
        }

        private DateTime time;

        public DateTime Time
        {
            get { return time; }
        }

        public IndicatorDataItem(DateTime time, Dictionary<string, double> values)
        {
            this.time = time;
            this.values = values;
        }

        public double this[string seriesName]
        {
            get
            {
                return values[seriesName];
            }
        }

    }
}
