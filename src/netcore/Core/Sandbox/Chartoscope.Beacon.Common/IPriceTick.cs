using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Common
{
    public interface IPriceTick
    {
        long Timestamp { get; set; }
        double Ask { get; set; }
        double Bid { get; set; }
    }
}
