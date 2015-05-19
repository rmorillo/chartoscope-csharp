using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/basic/stupid-guy-system
    //Forex trading strategy #7-a (The "stupid guy" system)
    public class StupidGuySystem: SignalBase
    {
        public const string IDENTITY_CODE = "stupid_guy_system";
        private MACD macd = null;

        public StupidGuySystem(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            macd = new MACD(barType, 35, 45, 30);

            Register(macd);
        }

        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return CrossesAbove(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return CrossesBelow(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine());
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
            if ((position == PositionMode.Long && CrossesBelow(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine()))
                || (position == PositionMode.Short && CrossesAbove(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine())))
            {
                Exit();
            }
        }
    }
}
