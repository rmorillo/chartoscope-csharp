using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/basic/adx-power
    //Forex trading strategy #14 (ADX Power)
    public class ADXPower: SignalBase
    {
        public const string IDENTITY_CODE = "adx_power";

        private EMA ema9 = null;
        private EMA ema26 = null;
        private ADX adx= null;

        public ADXPower(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            ema9 = new EMA(barType, 9);
            ema26 = new EMA(barType, 26);
            adx = new ADX(barType, 14);

            Register(ema9, ema26, adx);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return CrossesAbove(ema9.Value(1), ema9.Value(), ema26.Value(1), ema26.Value()) && adx.PlusDI() >= 25 && adx.ADXValue() >= 20
                && adx.ADXValue() >= Math.Min(adx.PlusDI(), adx.MinusDI()) && adx.ADXValue() <= Math.Max(adx.PlusDI(), adx.MinusDI());
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return CrossesAbove(ema26.Value(1), ema26.Value(), ema9.Value(1), ema9.Value()) && adx.MinusDI() >= 25 && adx.ADXValue() >= 20
                && adx.ADXValue() >= Math.Min(adx.PlusDI(), adx.MinusDI()) && adx.ADXValue() <= Math.Max(adx.PlusDI(), adx.MinusDI());
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

        private bool ExitLong(PriceBars priceBar)
        {
            bool emaCross = CrossesAbove(ema26.Value(1), ema26.Value(), ema9.Value(1), ema9.Value()) && adx.MinusDI() > adx.PlusDI();
            if (!emaCross)
            {
                bool diCross = CrossesAbove(adx.PlusDI(1), adx.PlusDI(), adx.MinusDI(1), adx.MinusDI()) || CrossesBelow(adx.PlusDI(1), adx.PlusDI(), adx.MinusDI(1), adx.MinusDI());
                if (diCross)
                {
                    return false;
                }

                if (priceBar.LastItem.High > ema26.Value() && priceBar.LastItem.Low < ema26.Value())
                {
                    return false;
                }
            }

            return emaCross && adx.MinusDI() > adx.PlusDI();
        }

        private bool ExitShort(PriceBars priceBar)
        {
            bool emaCross = CrossesAbove(ema9.Value(1), ema9.Value(), ema26.Value(1), ema26.Value()) && adx.PlusDI() > adx.MinusDI();
            if (!emaCross)
            {
                bool diCross = CrossesAbove(adx.PlusDI(1), adx.PlusDI(), adx.MinusDI(1), adx.MinusDI()) || CrossesBelow(adx.PlusDI(1), adx.PlusDI(), adx.MinusDI(1), adx.MinusDI());
                if (diCross)
                {
                    return false;
                }

                if (priceBar.LastItem.High > ema9.Value() && priceBar.LastItem.Low < ema9.Value())
                {
                    return false;
                }
            }

            return emaCross && adx.MinusDI() > adx.PlusDI();
        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if ((position == PositionMode.Long && ExitLong(priceBar))
                || (position == PositionMode.Short && ExitShort(priceBar)))
            {
                Exit();
            }
        }
    }
}
