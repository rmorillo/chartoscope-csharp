using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //This works great on daily charts! zero losses on backtesting!
    //minimal loss on hourly chart

    //Note ADR can be used to stop loss and profit target levels
    //http://forex-strategies-revealed.com/trading-strategy-basicbalanced
    //Forex trading strategy #1 (Simple balanced system)
    public class SimpleBalancedSystem: SignalBase
    {
        public const string IDENTITY_CODE = "ema_breakthrough";
        private EMA ema5 = null;
        private EMA ema10 = null;
        private Stochastics stoch = null;
        private RSI rsi = null;

        public SimpleBalancedSystem(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            ema5 = new EMA(barType, 5);
            ema10 = new EMA(barType, 10);
            stoch = new Stochastics(barType, 14, 3, 3);
            rsi = new RSI(barType, 14);

            Register(ema5, ema10, stoch, rsi);
        }

        private bool emaCrossed = false;
        private bool ema5CrossedAboveEMA10 = false;

        private bool IsLongSetup(PriceBars priceBar)
        {
            return emaCrossed && ema5CrossedAboveEMA10 && RisingStochasticLines() && stoch.PercentD()<=80 & stoch.PercentD()>=20 & rsi.Value()>50;
        }

        private bool RisingStochasticLines()
        {
            bool rising = false;
            if (!double.IsNaN(stoch.PercentK(2)))
            {
                double averagePreviousPercentK = (stoch.PercentK(1) + stoch.PercentK(2)) / 2;
                double averagePreviousPercentD = (stoch.PercentD(1) + stoch.PercentD(2)) / 2;
                rising = averagePreviousPercentK < stoch.PercentK() && averagePreviousPercentD < stoch.PercentD();
            }
            return rising;
        }

        private bool DecliningStochasticLines()
        {
            bool declining = false;
            if (!double.IsNaN(stoch.PercentK(2)))
            {
                double averagePreviousPercentK = (stoch.PercentK(1) + stoch.PercentK(2)) / 2;
                double averagePreviousPercentD = (stoch.PercentD(1) + stoch.PercentD(2)) / 2;
                declining = averagePreviousPercentK > stoch.PercentK() && averagePreviousPercentD > stoch.PercentD();
            }
            return declining;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return emaCrossed && !ema5CrossedAboveEMA10 && DecliningStochasticLines() && stoch.PercentD() >= 20 && stoch.PercentD()<=80 & rsi.Value() < 50;
        }


        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {

            if (CrossesAbove(ema5.Value(1), ema5.Value(), ema10.Value(1), ema10.Value()))
            {
                ema5CrossedAboveEMA10 = true;
                emaCrossed= true;
            }
            else if (CrossesBelow(ema5.Value(1), ema5.Value(), ema10.Value(1), ema10.Value()))
            {
                ema5CrossedAboveEMA10 = false;
                emaCrossed= true;
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
            if ((position == PositionMode.Long && ((CrossesBelow(ema5.Value(2), ema5.Value(1), ema10.Value(2), ema10.Value(1)) && ema10.Value() > ema5.Value()) || rsi.Value()<50))
                || (position == PositionMode.Short && ((CrossesAbove(ema5.Value(2), ema5.Value(1), ema10.Value(2), ema10.Value(1)) && ema10.Value() < ema5.Value()) || rsi.Value() > 50)))
            {
                Exit();
            }
        }
    }
}
