using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //Works great on H4 timeframe according to backtest
    //http://forex-strategies-revealed.com/trading-strategy-eurusd
    //Forex trading strategy #3 (EUR/USD simple system)
    public class EurUsdSimpleSystem: SignalBase
    {
        public const string IDENTITY_CODE = "ema_breakthrough";

        private ParabolicSAR psar = null;
        private MACD macd= null;

        private int signalLookbackPeriod= 2;

        public EurUsdSimpleSystem(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            psar = new ParabolicSAR(barType, 5);
            macd = new MACD(barType, 12, 26, 9);
            Register(psar, macd);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return buySignal == 2 && barSignalCount <= signalLookbackPeriod && macd.MACDValue() > macd.SignalLine() && psar.Direction() == 1;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return sellSignal == 2 && barSignalCount <= signalLookbackPeriod && macd.MACDValue() < macd.SignalLine() && psar.Direction() == -1;
        }

        int buySignal = 0;
        int sellSignal = 0;
        int barSignalCount = 0;

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (CrossesAbove(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine()))
            {                
                buySignal++;
                sellSignal = 0;
            }
            else if (CrossesBelow(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine()))
            {
                sellSignal++;
                buySignal = 0;
            }

            if (psar.Direction(1)==-1 && psar.Direction()==1)
            {
                buySignal++;               
                sellSignal = 0;
            }
            else if (psar.Direction(1)==1 && psar.Direction() == -1)
            {
                sellSignal++;
                buySignal = 0;
            }            

            if (buySignal > 0 || sellSignal>0)
            {
                barSignalCount++;
            }

            if (barSignalCount > signalLookbackPeriod)
            {
                buySignal = 0;
                sellSignal = 0;
                barSignalCount = 0;
            }

            if (IsLongSetup(priceBar))
            {
                Enter(PositionMode.Long);
                buySignal = 0;
                sellSignal = 0;
            }
            else if (IsShortSetup(priceBar))
            {
                Enter(PositionMode.Short);
                buySignal = 0;
                sellSignal = 0;
            }

        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if ((position == PositionMode.Long && (CrossesBelow(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine()) || psar.Direction()==-1))
                || (position == PositionMode.Short && (CrossesAbove(macd.MACDValue(1), macd.MACDValue(), macd.SignalLine(1), macd.SignalLine()) || psar.Direction() == 1)))
            {
                Exit();
            }
        }
    }
}
