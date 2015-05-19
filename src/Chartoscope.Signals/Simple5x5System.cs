using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //TODO:  This system uses Pips clearance, must know what pip range per currency.
    // Works well on D1 on backtesting

    //http://forex-strategies-revealed.com/trading-strategy-5x5
    //Forex trading strategy #5 (5x5 Simple system)
    public class Simple5x5System: SignalBase
    {
        public const string IDENTITY_CODE = "simple_5x5";
        private SMA sma = null;
        private RSI rsi = null;

        public Simple5x5System(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            sma = new SMA(barType, 5);
            rsi = new RSI(barType, 5);

            Register(sma, rsi);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return priceBar.HasValue(1) && priceBar.Last(1).High > sma.Value(1) && priceBar.Last(1).Low < sma.Value(1) && priceBar.LastItem.Low > sma.Value() && rsi.Value() > 50;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return priceBar.HasValue(1) && priceBar.Last(1).High > sma.Value(1) && priceBar.Last(1).Low < sma.Value(1) && priceBar.LastItem.High < sma.Value() && rsi.Value() < 50;
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
