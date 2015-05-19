using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //Works great in D1.

    //http://forex-strategies-revealed.com/trading-strategy-keysimplicity
    //Forex trading strategy #6 ("Key Simplicity")
    public class KeySimplicity: SignalBase
    {
        public const string IDENTITY_CODE = "key_simplicity";
        
        private EMA ema5 = null;
        private EMA ema12 = null;
        private RSI rsi = null;

        public KeySimplicity(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            ema5 = new EMA(barType, 5);
            ema12 = new EMA(barType, 12);
            rsi = new RSI(barType, 21);

            Register(ema5, ema12, rsi);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return CrossesAbove(ema5.Value(1), ema5.Value(), ema12.Value(1), ema12.Value()) && rsi.Value() > 50;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return CrossesBelow(ema5.Value(1), ema5.Value(), ema12.Value(1), ema12.Value()) && rsi.Value() < 50;
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
