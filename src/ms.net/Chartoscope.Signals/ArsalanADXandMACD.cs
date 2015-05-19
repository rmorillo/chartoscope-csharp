using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/simple/arsalans-adx-macd
    //Forex trading strategy #12 (Arsalan's ADX + MACD)
    public class ArsalanADXandMACD: SignalBase
    {
        public const string IDENTITY_CODE = "arsalan_adx_macd";
        private MACD macd = null;
        private ADX adx = null;

        public ArsalanADXandMACD(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            macd = new MACD(barType, 3, 10, 18);
            adx = new ADX(barType, 18);

            Register(macd, adx);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return adx.PlusDI() > adx.MinusDI() && CrossesAbove(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return adx.PlusDI() < adx.MinusDI() && CrossesBelow(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine());
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
