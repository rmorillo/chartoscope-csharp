using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public static class IndicatorDelegates
    {
        public delegate void NewIndicatorValueHandler(object source, BarItemType barItemType);
        public delegate void CalculationCompletedHandler();
    }
}
