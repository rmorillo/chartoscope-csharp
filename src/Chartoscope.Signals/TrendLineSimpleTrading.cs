using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //TODO:  Use linear regression algorithm to determine the trendline based on lowest price.

    //http://forex-strategies-revealed.com/basic/trend-line-trading
    //Forex trading strategy #15 (Trend line simple trading)
    public class TrendLineSimpleTrading: SignalBase
    {
        public const string IDENTITY_CODE = "trendline_simple_trading";
        private SMA sma = null;

        public TrendLineSimpleTrading(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            sma = new SMA(barType, 30);

            Register(sma);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {           
            return sma.Value() < priceBar.LastItem.High && sma.Value() > priceBar.LastItem.Low && priceBar.LastItem.Close > priceBar.LastItem.Open;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return sma.Value() < priceBar.LastItem.High && sma.Value() > priceBar.LastItem.Low && priceBar.LastItem.Close < priceBar.LastItem.Open;
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
