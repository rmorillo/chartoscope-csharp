using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public static class DataFeederDelegates
    {
        public delegate void NewBarHandler(BarItemType barItemType, BarItem barItem);
        public delegate void BarGapHandler(BarItemType barItemType, DateTime dateTime);
        public delegate void NewQuoteHandler(double bidPrice, double askPrice);
    }
}
