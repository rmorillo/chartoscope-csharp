using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //TODO:  Apply divergence
    //Works great in 15 minute timeframe
    //http://forex-strategies-revealed.com/simple/best-of-macd-entries
    //Forex trading strategy #22 (Best of MACD Entries)
 
    public class BestOfMACD: SignalBase
    {
        public const string IDENTITY_CODE = "best_of_macd";

        private SMA sma10 = null;
        private SMA sma50 = null;
        private MACD macd = null;

        public BestOfMACD(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            sma10 = new SMA(barType, 10);
            sma50 = new SMA(barType, 50);
            macd = new MACD(barType, 12, 16, 9);

            Register(sma10, sma50, macd);
        }

        private TrendDirectionMode trend = TrendDirectionMode.Neutral;

        private bool IsLongSetup(PriceBars priceBar)
        {           
            return trend==TrendDirectionMode.Bullish && CrossesAbove(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return trend == TrendDirectionMode.Bearish && CrossesBelow(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine()) ;
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (CrossesAbove(sma10.Value(1), sma10.Value(), sma50.Value(1), sma50.Value()))
            {
                trend = TrendDirectionMode.Bullish;
            }
            else if (CrossesBelow(sma10.Value(1), sma10.Value(), sma50.Value(1), sma50.Value()))
            {
                trend = TrendDirectionMode.Bearish;
            }

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
