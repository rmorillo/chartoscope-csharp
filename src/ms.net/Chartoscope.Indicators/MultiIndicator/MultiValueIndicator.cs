using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Droidworks.Indicators
{
    public class MultiValueIndicator
    {
        private List<double> values;

        public List<double> Values
        {
            get { return this.values; }
            set { this.values = value; }
        }

        private DateTime timestamp;

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public MultiValueIndicator(DateTime timestamp, params double[] values)
        {
            this.values = new List<double>(values);
            this.timestamp = timestamp;
        }
    }
}
