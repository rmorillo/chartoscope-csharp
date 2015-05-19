using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public struct TickerType
    {

        private string symbol;

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        

        private MarketTypeOption market;

        public MarketTypeOption Market
        {
            get { return market; }
            set { market = value; }
        }

        private InstrumentTypeOption instrumentType;

        public InstrumentTypeOption InstrumentType
        {
            get { return instrumentType; }
            set { instrumentType = value; }
        }

        
        public TickerType(string symbol, string displayName, MarketTypeOption market, InstrumentTypeOption instrumentType)
        {
            this.symbol = symbol;
            this.displayName = displayName;
            this.market = market;
            this.instrumentType = instrumentType;
        }

        public double PipsFactor { get { return 10000; } }
    }
}
