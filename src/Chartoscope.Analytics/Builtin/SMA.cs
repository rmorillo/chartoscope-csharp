using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadroids.Common.Enumerations;

namespace Metadroids.Analytics.Builtin
{
    public class SMA
    {
        internal SMACore _smaCore { get; set; }

        private TimeframeMode timeframe;
        public TimeframeMode Timeframe { get { return Timeframe; } }

        public SMA(TimeframeMode timeframe)
        {
            this.timeframe = timeframe;   
        }
    }
}
