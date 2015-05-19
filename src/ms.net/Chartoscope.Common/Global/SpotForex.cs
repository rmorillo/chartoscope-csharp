using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class SpotForex
    {
        public static TickerType EURUSD= new TickerType("EURUSD", "EUR/USD", MarketTypeOption.Forex, InstrumentTypeOption.Spot);        
    }
}
