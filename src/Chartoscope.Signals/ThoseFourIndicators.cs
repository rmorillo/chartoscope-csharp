using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/those4indicators
    //Forex trading strategy #21 (Those 4 Indicators)
    public class ThoseFourIndicators: SignalBase
    {
        public const string IDENTITY_CODE = "those_four_indicators";
        private CCI cci = null;
        private Stochastics stoch = null;
        private MACD macd = null;
        private ADX adx = null;

        public ThoseFourIndicators(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            cci = new CCI(barType, 14);
            stoch = new Stochastics(barType, 14, 2, 2);
            macd = new MACD(barType, 8, 40, 8);
            adx = new ADX(barType, 14);

            Register(cci, stoch, macd, adx);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return cci.Value() > 0 && stoch.PercentD() > 50 && (macd.MACDValue() > 0 || adx.PlusDI() > adx.MinusDI());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return cci.Value() < 0 && stoch.PercentD() < 50 && (macd.MACDValue() < 0 || adx.PlusDI() < adx.MinusDI());
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
