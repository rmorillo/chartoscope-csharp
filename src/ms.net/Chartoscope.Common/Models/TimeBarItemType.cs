using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class TimeBarItemType: BarItemType
    {
        public TimeBarItemType(TimeframeUnitOption timeframeUnit)
        {
            this.Mode = BarItemMode.Time;
            string timeframe = Enum.GetName(typeof(TimeframeUnitOption), timeframeUnit);
            this.Value = timeframe.StartsWith("MN") ? 1 : int.Parse(timeframe.Substring(1));
            this.Tag= timeframe.StartsWith("MN") ? "MN" : timeframe.Substring(0,1);
        }        
    }
}
