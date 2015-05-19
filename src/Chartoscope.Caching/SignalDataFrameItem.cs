using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class SignalDataFrameItem
    {
        private BarItem priceBar;

        public BarItem PriceBar
        {
            get { return priceBar; }
        }

        private List<SignalDataItem> signals;

        public List<SignalDataItem> Signals
        {
            get { return signals; }
            set { signals = value; }
        }

        private Dictionary<string, IndicatorDataItem> indicators;

        public Dictionary<string, IndicatorDataItem> Indicators
        {
            get { return indicators; }
        }

        public SignalDataFrameItem(BarItem priceBar, List<SignalDataItem> signals, Dictionary<string, IndicatorDataItem> indicators)
        {
            this.priceBar= priceBar;
            this.signals= signals;
            this.indicators= indicators;
        }
    }
}
