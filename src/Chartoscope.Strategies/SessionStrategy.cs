using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Strategies
{
    public class SessionStrategy
    {
        public BarItemType BarType { get; set; }
        public Dictionary<string, CoreStrategy> CoreStrategies { get; set; }

        public SessionStrategy(BarItemType barItemType)
        {
            this.BarType = barItemType;
            this.CoreStrategies = new Dictionary<string, CoreStrategy>();
        }
    }
}
