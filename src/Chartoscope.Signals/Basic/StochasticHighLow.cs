using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/trading-strategy-fullstochastic
    //Forex trading strategy #3 (Stochastic High-Low)
    public class StochasticHighLow: SignalBase
    {
        public const string IDENTITY_CODE = "stoch_high_low";
        private Stochastics stoch = null;

        public StochasticHighLow(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            stoch = new Stochastics(barType, 14, 3, 3);

            Register(stoch);
        }

        private bool crossedBelow20 = false;
        private bool reached10 = false;

        private bool crossedAbove80 = false;
        private bool reached90 = false;
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            if (!crossedBelow20)
            {
                crossedBelow20 = stoch.PercentD()<20;
            }

            if (!reached10)
            {
                reached10 = crossedBelow20 && stoch.PercentD() >= 10;
            }
            
            return reached10 && stoch.PercentD()>20;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            if (!crossedAbove80)
            {
                crossedAbove80 = stoch.PercentD() > 80;
            }

            if (!reached90)
            {
                reached90 = crossedAbove80 && stoch.PercentD() >= 90;
            }

            return reached90 && stoch.PercentD() <= 80;
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (IsLongSetup(priceBar))
            {
                Enter(PositionMode.Long);
                crossedBelow20 = false;
                reached10 = false;
            }
            else if (IsShortSetup(priceBar))
            {
                Enter(PositionMode.Short);
                crossedAbove80 = false;
                reached90 = false;
            }

        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if ((position == PositionMode.Long && stoch.PercentD()>=80)
                || (position == PositionMode.Short && stoch.PercentD()<=20))
            {
                Exit();
            }
        }
    }
}
