using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Common
{
    public class PriceTick: IPriceTick
    {
        public long Timestamp { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
    }
}
