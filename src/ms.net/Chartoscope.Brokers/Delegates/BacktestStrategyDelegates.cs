using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public class BacktestStrategyDelegates
    {
        public delegate void OpenPositionHandler(Guid accountId, TickerType tickerType, PositionMode position, double bidPrice, double askPrice, BarItem barItem);
        public delegate void ClosePositionHandler(Guid accountId, TickerType tickerType, double bidPrice, double askPrice, BarItem barItem);
    }
}
