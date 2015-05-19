using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class ChartStrategyItem
    {
        private StrategyStateOption strategyState;

        public StrategyStateOption StrategyState
        {
            get { return strategyState; }
        }

        private DateTime time;

        public DateTime Time
        {
            get { return time; }     
        }

        public ChartStrategyItem(StrategyStateOption strategyState, DateTime time)
        {
            this.strategyState = strategyState;
            this.time = time;
        }
    }
}
