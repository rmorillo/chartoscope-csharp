using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/trading-strategy-macd
    //Forex trading strategy #7 (Simple MACD crossover)
    public class SimpleMACDCrossover: SignalBase
    {
        public const string IDENTITY_CODE = "simple_macd_cross";
        private MACD macd = null;

        public SimpleMACDCrossover(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            macd = new MACD(barType, 12, 26, 9);

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
