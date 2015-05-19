using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/trading-strategy-emabreak
    //Forex trading strategy #8 (EMA breakthrough)
    public class EMABreakthrough: SignalBase
    {
        public const string IDENTITY_CODE = "ema_breakthrough";
        private EMA ema = null;

        public EMABreakthrough(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            ema = new EMA(barType, 50);

            Register(ema);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {           
            return ema.Value() < priceBar.LastItem.Close && ema.Value() > priceBar.LastItem.Low;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return ema.Value() > priceBar.LastItem.Close && ema.Value() < priceBar.LastItem.High;
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
