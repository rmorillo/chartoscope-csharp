using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class SessionIndicator: ISessionIndicator
    {
        public BarItemType BarType { get; set; }
        public Dictionary<string, ICoreIndicator> CoreIndicators { get; set; }

        public SessionIndicator(BarItemType barItemType)
        {
            this.BarType = barItemType;
            this.CoreIndicators = new Dictionary<string, ICoreIndicator>();
        }
    }
}
