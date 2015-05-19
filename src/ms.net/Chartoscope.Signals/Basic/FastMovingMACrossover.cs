using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/trading-strategy-ema
    //Forex trading strategy #1 (Fast moving averages crossover)
    public class FastMovingMACrossover: SignalBase
    {
        public const string IDENTITY_CODE = "fast_ma_cross";
        private EMA ema10 = null;
        private EMA ema25 = null;
        private EMA ema50 = null;

        public FastMovingMACrossover(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            ema10 = new EMA(barType, 10);
            ema25 = new EMA(barType, 25);
            ema50 = new EMA(barType, 50);

            Register(ema10, ema25, ema50);
        }

        private bool crossedAbove25 = false;
        private bool crossedBelow25 = false;
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            if (!crossedAbove25)
            {
                crossedAbove25 = CrossesAbove(ema10.Value(1), ema10.Value(), ema25.Value(1), ema25.Value());
            }

            bool crossedAbove50 = crossedAbove25 && CrossesAbove(ema10.Value(1), ema10.Value(), ema50.Value(1), ema50.Value());

            if (crossedAbove50)
            {
                crossedBelow25 = false;                
            }

            return crossedAbove50;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            if (!crossedBelow25)
            {
                crossedBelow25 = CrossesBelow(ema10.Value(1), ema10.Value(), ema25.Value(1), ema25.Value());
            }

            bool crossedBelow50 = crossedBelow25 && CrossesBelow(ema10.Value(1), ema10.Value(), ema50.Value(1), ema50.Value());

            if (crossedBelow50)
            {
                crossedAbove25 = false;
            }

            return crossedBelow50;
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (IsLongSetup(priceBar))
            {
                Enter(PositionMode.Long);
                crossedAbove25 = false;
            }
            else if (IsShortSetup(priceBar))
            {
                Enter(PositionMode.Short);
                crossedBelow25 = false;
            }

        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if ((position == PositionMode.Long && (CrossesBelow(ema10.Value(1), ema10.Value(), ema25.Value(1), ema25.Value()) || CrossesBelow(ema10.Value(1), ema10.Value(), ema50.Value(1), ema50.Value()))
                || (position == PositionMode.Short && (CrossesAbove(ema10.Value(1), ema10.Value(), ema25.Value(1), ema25.Value()) || CrossesAbove(ema10.Value(1), ema10.Value(), ema50.Value(1), ema50.Value())))))
            {
                Exit();
            }
        }
    }
}
