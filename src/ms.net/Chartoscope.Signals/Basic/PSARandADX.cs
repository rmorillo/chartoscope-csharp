using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/trading-strategy-adxsar
    //Forex trading strategy #2 (Parabolic SAR + ADX)
    public class PSARandADX : SignalBase
    {
        public const string IDENTITY_CODE = "psar+adx";
        private ParabolicSAR psar = null;
        private ADX adx = null;

        public PSARandADX(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            psar = new ParabolicSAR(barType, 5);
            adx = new ADX(barType, 50);

            Register(psar, adx);
        }

        private bool IsLongSetup(PriceBars priceBar)
        {
            return (adx.PlusDI() > adx.MinusDI() && psar.Direction() == 1);
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return (adx.PlusDI() < adx.MinusDI() && psar.Direction() == -1);
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
            if ((position == PositionMode.Long && adx.PlusDI() < adx.MinusDI()) || (position == PositionMode.Short && adx.PlusDI() > adx.MinusDI()))
            {
                Exit();
            }
        }
    }
}
