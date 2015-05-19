using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/trading-strategy-rsi
    //Forex trading strategy #4 (RSI High-Low)
    public class RSIHighLow: SignalBase
    {
        public const string IDENTITY_CODE = "rsi_high_low";
        private RSI rsi = null;

        public RSIHighLow(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            rsi = new RSI(barType, 14);

            Register(rsi);
        }

        private bool crossedBelow30 = false;
        private bool crossedAbove70 = false;

        
        private bool IsLongSetup(PriceBars priceBar)
        {
            if (!crossedBelow30)
            {
                crossedBelow30 = rsi.Value() < 30;
            }

            return crossedBelow30 && rsi.Value() > 30;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            if (!crossedAbove70)
            {
                crossedAbove70 = rsi.Value() > 70;
            }

            return crossedAbove70 && rsi.Value() <= 70;
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (IsLongSetup(priceBar))
            {
                Enter(PositionMode.Long);
                crossedBelow30 = false;
            }
            else if (IsShortSetup(priceBar))
            {
                Enter(PositionMode.Short);
                crossedAbove70 = false;
            }

        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if ((position == PositionMode.Long && rsi.Value()>=70)
                || (position == PositionMode.Short && rsi.Value()<=30))
            {
                Exit();
            }
        }
    }
}
