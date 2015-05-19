using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class StrategyDataFrameItem
    {
        private BarItem priceBar;

        public BarItem PriceBar
        {
            get { return priceBar; }
        }

        private List<StrategyDataItem> strategies;

        public List<StrategyDataItem> Strategies
        {
            get { return strategies; }
            set { strategies = value; }
        }

        private Dictionary<string, SignalDataItem> signals;

        public Dictionary<string, SignalDataItem> Signals
        {
            get { return signals; }
            set { signals = value; }
        }

        private Dictionary<string, IndicatorDataItem> indicators;

        public Dictionary<string, IndicatorDataItem> Indicators
        {
            get { return indicators; }
        }

        public StrategyDataFrameItem(BarItem priceBar, List<StrategyDataItem> strategies, Dictionary<string, SignalDataItem> signals, Dictionary<string, IndicatorDataItem> indicators)
        {
            this.priceBar = priceBar;
            this.strategies = strategies;
            this.signals = signals;
            this.indicators = indicators;
        }
    }
}
