using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/simple/stochastic-emas-cross
    //Forex trading strategy #14 (Stochastic + EMAs' cross)
    public class ArsalanStochasticAndEMA: SignalBase
    {
        public const string IDENTITY_CODE = "arsalan_stochastic_ema";
        private Stochastics stoch = null;
        private EMA ema2 = null;
        private EMA ema4 = null;

        public ArsalanStochasticAndEMA(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            ema2 = new EMA(barType, 2);
            ema4 = new EMA(barType, 4);
            stoch = new Stochastics(barType, 5, true);

            Register(ema2, ema4, stoch);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return stoch.PercentD() < 50 && CrossesAbove(ema2.Value(1), ema2.Value(), ema4.Value(1), ema4.Value());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return stoch.PercentD() > 50 && CrossesBelow(ema2.Value(1), ema2.Value(), ema4.Value(1), ema4.Value());
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
            if ((position == PositionMode.Long && IsShortSetup(priceBar))
                || (position == PositionMode.Short && IsLongSetup(priceBar)))
            {
                Exit();
            }
        }
    }
}
