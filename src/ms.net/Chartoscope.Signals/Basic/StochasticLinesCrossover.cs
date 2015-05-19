using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/trading-strategy-stochasticcross
    //Forex trading strategy #5 (Stochastic lines crossover)
    public class StochasticLinesCrossover: SignalBase
    {
        public const string IDENTITY_CODE = "stoch_high_low";
        private Stochastics stoch = null;

        public StochasticLinesCrossover(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            stoch = new Stochastics(barType, 14, 3, 3);

            Register(stoch);
        }

        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return CrossesAbove(stoch.PercentK(1), stoch.PercentK(), stoch.PercentD(1), stoch.PercentD());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return CrossesBelow(stoch.PercentK(1), stoch.PercentK(), stoch.PercentD(1), stoch.PercentD());
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
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
            if ((position == PositionMode.Long && CrossesBelow(stoch.PercentK(1), stoch.PercentK(), stoch.PercentD(1), stoch.PercentD()))
                || (position == PositionMode.Short && CrossesAbove(stoch.PercentK(1), stoch.PercentK(), stoch.PercentD(1), stoch.PercentD())))
            {
                Exit();
            }
        }
    }
}
