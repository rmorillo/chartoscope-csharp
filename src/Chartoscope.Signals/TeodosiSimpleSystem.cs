using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/teodosi-simple-system
    //Forex trading strategy #8 (Teodosi's simple system)
    public class TeodosiSimpleSystem: SignalBase
    {
        public const string IDENTITY_CODE = "teodosi_simple_system";
        private RSI rsi = null;
        private Stochastics stoch = null;

        private int candleLookbackLimit = 5;

        public TeodosiSimpleSystem(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            rsi = new RSI(barType, 7);
            stoch = new Stochastics(barType, 5, 3, 3);

            Register(rsi, stoch);
        }

        private int LastBearishCandle(PriceBars priceBar)
        {
            int index=0;
            while (!priceBar.Last(index).BearishCandle() && index < candleLookbackLimit)
            {
                index++;
            }

            return index == candleLookbackLimit ? int.MinValue : index;
        }

        private int LastBullishCandle(PriceBars priceBar)
        {
            int index = 0;            
            while (!priceBar.Last(index).BullishCandle() && index < candleLookbackLimit)
            {
                index++;
            }

            return index == candleLookbackLimit ? int.MinValue : index;
        }

        private bool IsLongSetup(PriceBars priceBar)
        {
            int lastBearishCandle = LastBearishCandle(priceBar);
            bool closedMidCandle = lastBearishCandle!=int.MinValue;
            if (lastBearishCandle > 0)
            {
                closedMidCandle = priceBar.Last(lastBearishCandle).High > priceBar.Last(lastBearishCandle - 1).Close && priceBar.Last(lastBearishCandle).Low > priceBar.Last(lastBearishCandle - 1).Close;
            }

            return rsi.Value() < 50 && stoch.PercentD() < 50 && CrossesAbove(stoch.PercentK(1), stoch.PercentK(), stoch.PercentD(1), stoch.PercentD()) && closedMidCandle;                
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            int lastBullishCandle = LastBullishCandle(priceBar);
            bool closedMidCandle = false;
            if (lastBullishCandle > 0)
            {
                closedMidCandle = priceBar.Last(lastBullishCandle).High > priceBar.Last(lastBullishCandle - 1).Close && priceBar.Last(lastBullishCandle).Low > priceBar.Last(lastBullishCandle - 1).Close;
            }

            return rsi.Value() > 50 && stoch.PercentD() > 50 && CrossesBelow(stoch.PercentK(1), stoch.PercentK(), stoch.PercentD(1), stoch.PercentD()) && closedMidCandle;
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
