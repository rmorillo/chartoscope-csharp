using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/trading-strategy-sma
    //Forex trading strategy #2 (Slow moving averages crossover)
    public class SlowMovingMACrossover: SignalBase
    {
        public const string IDENTITY_CODE = "slow_ma_cross";
        private SMA sma7 = null;
        private SMA sma14 = null;
        private SMA sma21 = null;

        public SlowMovingMACrossover(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            sma7 = new SMA(barType, 10);
            sma14 = new SMA(barType, 25);
            sma21 = new SMA(barType, 50);

            Register(sma7, sma14, sma21);
        }

        private bool crossedAbove14 = false;
        private bool crossedBelow14 = false;
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            if (!crossedAbove14)
            {
                crossedAbove14 = CrossesAbove(sma7.Value(1), sma7.Value(), sma14.Value(1), sma14.Value());
            }

            bool crossedAbove21 = crossedAbove14 && CrossesAbove(sma7.Value(1), sma7.Value(), sma21.Value(1), sma21.Value());

            if (crossedAbove21)
            {
                crossedBelow14 = false;                
            }

            return crossedAbove21;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            if (!crossedBelow14)
            {
                crossedBelow14 = CrossesBelow(sma7.Value(1), sma7.Value(), sma14.Value(1), sma14.Value());
            }

            bool crossedBelow21 = crossedBelow14 && CrossesBelow(sma7.Value(1), sma7.Value(), sma21.Value(1), sma21.Value());

            if (crossedBelow21)
            {
                crossedAbove14 = false;
            }

            return crossedBelow21;
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (IsLongSetup(priceBar))
            {
                Enter(PositionMode.Long);
                crossedAbove14 = false;
            }
            else if (IsShortSetup(priceBar))
            {
                Enter(PositionMode.Short);
                crossedBelow14 = false;
            }

        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if ((position == PositionMode.Long && (CrossesBelow(sma7.Value(1), sma7.Value(), sma14.Value(1), sma14.Value()) || CrossesBelow(sma7.Value(1), sma7.Value(), sma21.Value(1), sma21.Value()))
                || (position == PositionMode.Short && (CrossesAbove(sma7.Value(1), sma7.Value(), sma14.Value(1), sma14.Value()) || CrossesAbove(sma7.Value(1), sma7.Value(), sma21.Value(1), sma21.Value())))))
            {
                Exit();
            }
        }
    }
}
