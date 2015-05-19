using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://strategy4forex.com/strategies-based-on-forex-indicators/strategy-forex-parabolic-sar-stochastic.html
    //Strategy Forex Parabolic SAR + Stochastic
    
    //Revised
    //Original strategy misses the big reversal moves because of applying retracement only trading
    //I only intend to use EMA for potential breakout signals
    public class PSARandStochastic2 : SignalBase
    {
        public const string IDENTITY_CODE = "psar+stoc";
        private ParabolicSAR psar = null;
        private Stochastics stoc = null;
        private EMA ema = null;

        public PSARandStochastic2(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            psar = new ParabolicSAR(barType, 5);
            stoc = new Stochastics(barType, 14);
            ema = new EMA(barType, 100);

            Register(psar, stoc, ema);
        }

        private const double OVERSOLD_START = 63;
        private const double OVERBOUGHT_START = 37;

        private bool IsWithinOversoldRange()
        {
            return stoc.PercentK() > OVERSOLD_START;
        }

        private const int STOCH_LOOPBACK_PERIOD = 3;

        private ChartPoint[] CreateStochChartPoints()
        {
            ChartPoint[] chartPoints = new ChartPoint[STOCH_LOOPBACK_PERIOD];
            for (int i = 0; i < STOCH_LOOPBACK_PERIOD; i++)
            {
                chartPoints[i] = new ChartPoint((double)i, stoc.PercentD((STOCH_LOOPBACK_PERIOD - 1) - i));
            }

            return chartPoints;
        }

        private bool IsUpwardStochasticSlope()
        {
            return LinearMathHelper.GetSlope(CreateStochChartPoints()) > 0;
        }

        private bool IsDownwardStochasticSlope()
        {
            return LinearMathHelper.GetSlope(CreateStochChartPoints()) < 0;
        }

        private bool IsWithinOverboughtRange()
        {
            return stoc.PercentK() < OVERBOUGHT_START;
        }

        private const int REVERSAL_LOOKBACK_PERIOD = 7;
     
        private bool BullishReversal(PriceBars priceBar, int lookbackPeriod=1)
        {
            bool hasReversed = false;
            for (int i = 0; i < lookbackPeriod; i++)
            {
                if (psar.NextSAR(i) < priceBar.Last(i).Low && psar.NextSAR(i+1) > priceBar.Last(i+1).Low)
                {
                    hasReversed= true;
                    break;
                }
            }
            return hasReversed;
        }

        private bool IsLongSetup(PriceBars priceBar)
        {
            return (IsWithinOversoldRange() || (IsUpwardStochasticSlope() && stoc.PercentK() > 50)) && BullishReversal(priceBar, REVERSAL_LOOKBACK_PERIOD);
        }

        private bool BearishReversal(PriceBars priceBar, int lookbackPeriod = 1)
        {
            bool hasReversed = false;
            for (int i = 0; i < lookbackPeriod; i++)
            {
                if (psar.NextSAR(i) > priceBar.Last(i).High && psar.NextSAR(i+1) < priceBar.Last(i+1).Low)
                {
                    hasReversed= true;
                    break;
                }
            }
            return hasReversed;          
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return (IsWithinOverboughtRange() || (IsDownwardStochasticSlope() && stoc.PercentK() < 50)) && BearishReversal(priceBar, REVERSAL_LOOKBACK_PERIOD);
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (priceBar.LastItem.Time >= new DateTime(2012, 8, 2, 4, 0, 0))
            {
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
            if (priceBar.LastItem.Time >= new DateTime(2012, 8, 2, 4, 0, 0))
            {
            }
            if ((position == PositionMode.Long && psar.Direction()==-1)
                || (position == PositionMode.Short && psar.Direction()==1))
            {
                Exit();
            }
        }
    }
}
