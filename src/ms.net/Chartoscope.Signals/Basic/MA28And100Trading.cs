using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/simple/28-100-ma-trading
    //Forex trading strategy #2-a (28-100 MA trading)
    public class MA28And100Trading: SignalBase
    {
        public const string IDENTITY_CODE = "ma_28_100";
        private EMA ema100 = null;
        private SMA sma28 = null;

        public MA28And100Trading(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            ema100 = new EMA(barType, 100);
            sma28 = new SMA(barType, 28);

            Register(ema100, sma28);
        }

        private bool enteredPosition = false;
        private PositionMode lastPosition;

        private bool IsLongSetup(PriceBars priceBar)
        {
            bool longSetup= priceBar.LastItem.Close > ema100.Value();
            //possible re-entry
            if (enteredPosition && lastPosition == PositionMode.Long && priceBar.LastItem.Close > sma28.Value())
            {
                longSetup= true;
            }

            return longSetup;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            bool shortSetup = priceBar.LastItem.Close < ema100.Value();
            //possible re-entry
            if (enteredPosition && lastPosition == PositionMode.Short && priceBar.LastItem.Close < sma28.Value())
            {
                shortSetup = true;
            }

            return shortSetup;
        }

        private double stopLossPrice = double.NaN;

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (IsLongSetup(priceBar))
            {
                Enter(PositionMode.Long);
                stopLossPrice = priceBar.LastItem.Close - (20 * 0.0001);
                enteredPosition = true;
            }
            else if (IsShortSetup(priceBar))
            {
                Enter(PositionMode.Short);
                stopLossPrice = priceBar.LastItem.Close + (20 * 0.0001);
                enteredPosition = true;
            }

        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if ((position == PositionMode.Long && (priceBar.LastItem.Close < sma28.Value() || priceBar.LastItem.Close <= stopLossPrice))
                || (position == PositionMode.Short && (priceBar.LastItem.Close > sma28.Value() || priceBar.LastItem.Close >= stopLossPrice)))
            {
                lastPosition = position;
                Exit();
            }
         
        }
    }
}
