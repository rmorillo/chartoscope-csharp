using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/trading-strategy-doublestochastic
    //Forex trading strategy #6 (Double Stochastic)
    public class DoubleStochastic: SignalBase
    {
        public const string IDENTITY_CODE = "double_stoch";
        private Stochastics stochMajor = null;
        private Stochastics stochRetrace = null;

        public DoubleStochastic(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            stochMajor = new Stochastics(barType, 26, 14, 14);
            stochRetrace = new Stochastics(barType, 14, 3, 3);

            Register(stochMajor, stochRetrace);
        }

        private TrendDirectionMode majorTrend = TrendDirectionMode.Neutral;
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return majorTrend == TrendDirectionMode.Bullish && CrossesAbove(stochRetrace.PercentK(1), stochRetrace.PercentK(), stochRetrace.PercentD(1), stochRetrace.PercentD());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return majorTrend == TrendDirectionMode.Bearish && CrossesBelow(stochRetrace.PercentK(1), stochRetrace.PercentK(), stochRetrace.PercentD(1), stochRetrace.PercentD());
        }

        private TrendDirectionMode GetMajorTrend()
        {
            TrendDirectionMode trend;
            if (CrossesAbove(stochMajor.PercentK(1), stochMajor.PercentK(), stochMajor.PercentD(1), stochMajor.PercentD()))
            {
                trend = TrendDirectionMode.Bullish;
            }
            else if (CrossesBelow(stochMajor.PercentK(1), stochMajor.PercentK(), stochMajor.PercentD(1), stochMajor.PercentD()))
            {
                trend = TrendDirectionMode.Bearish;
            }
            else
            {
                trend = TrendDirectionMode.Neutral;
            }
            return trend;
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            majorTrend = GetMajorTrend();

            if (IsLongSetup(priceBar))
            {
                Enter(PositionMode.Long);
            }
            else if (IsShortSetup(priceBar))
            {
                Enter(PositionMode.Short);
            }

        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if ((position == PositionMode.Long && CrossesBelow(stochRetrace.PercentK(1), stochRetrace.PercentK(), stochRetrace.PercentD(1), stochRetrace.PercentD()))
                || (position == PositionMode.Short && CrossesAbove(stochRetrace.PercentK(1), stochRetrace.PercentK(), stochRetrace.PercentD(1), stochRetrace.PercentD()))
               )
            {
                Exit();
            }
        }
    }
}
