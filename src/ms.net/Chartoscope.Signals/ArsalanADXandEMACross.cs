using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/arsalan-adx-ema-cross
    //Forex trading strategy #11 (Arsalan's ADX+EMAs cross system) 
    public class ArsalanADXandEMACross: SignalBase
    {
        public const string IDENTITY_CODE = "arsalan_adx_ema_cross";
        private EMA ema3 = null;
        private EMA ema10 = null;
        private ADX adx = null;

        public ArsalanADXandEMACross(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            ema3 = new EMA(barType, 3);
            ema10 = new EMA(barType, 10);
            adx = new ADX(barType, 14);

            Register(ema3, ema10, adx);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return adx.PlusDI() > adx.MinusDI() && CrossesAbove(ema3.Value(1), ema3.Value(), ema10.Value(1), ema10.Value());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return adx.PlusDI() < adx.MinusDI() && CrossesBelow(ema3.Value(1), ema3.Value(), ema10.Value(1), ema10.Value());
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
