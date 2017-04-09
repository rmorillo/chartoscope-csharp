using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class TimeframeUnit: FeedInterval
    {
        public TimeframeUnit(TimeframeOption timeframe)
        {
            Unit = IntervalOption.Timeframe;
            UnitId = (int)timeframe;
            UnitType = typeof(TimeframeOption);
        }

        public TimeframeOption Timeframe
        {
            get
            {
                return (TimeframeOption)UnitId;
            }
        }        
    }
}
