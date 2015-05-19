using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://strategy4forex.com/strategies-based-on-forex-indicators/strategy-forex-parabolic-sar-stochastic.html
    //Strategy Forex Parabolic SAR + Stochastic
    public class PSARandStochastic: SignalBase
    {
        public const string IDENTITY_CODE = "psar+stoc";
        private ParabolicSAR psar = null;
        private Stochastics stoc = null;
        private EMA ema = null;

        public PSARandStochastic(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            psar = new ParabolicSAR(barType, 5);
            stoc = new Stochastics(barType, 14);
            ema = new EMA(barType, 100);

            Register(psar, stoc, ema);
        }

        private bool IsLongSetup(PriceBars priceBar)
        {
            return (stoc.PercentK() > 63 && psar.Direction(1) != psar.Direction() && psar.Direction() == 1 && priceBar.LastItem.Close > ema.Value());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return (stoc.PercentK() < 37 && psar.Direction(1) != psar.Direction() && psar.Direction() == -1 && priceBar.LastItem.Close < ema.Value());
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
            if ((position == PositionMode.Long && psar.Direction() == -1) || (position == PositionMode.Short && psar.Direction() == 1))
            {
                Exit();
            }
        }        
    }
}
