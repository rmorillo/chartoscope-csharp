using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class BarItem
    {
        public DateTime Time { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        private Guid id;

        public BarItem(DateTime time, double open, double close, double high, double low)
        {
            this.Time = time;
            this.Open = open;
            this.Close = close;
            this.High = high;
            this.Low = low;
            this.id = Guid.NewGuid();
        }

        public override int GetHashCode()
        {
            return Time.GetHashCode() ^ Open.GetHashCode() ^ Close.GetHashCode() ^ High.GetHashCode() ^ Low.GetHashCode() ^ id.GetHashCode();
        }
    }
}
