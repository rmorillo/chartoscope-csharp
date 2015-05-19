using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class EMACore: IndicatorBase<SingleValueIndicatorItem>
    {
        public const string SHORT_NAME = "ema";
        public const string DISPLAY_NAME = "ExponentialMovingAverage";
        public const string DESCRIPTION= "Exponential Moving Average";

        private double smoothingConstant = 0;

        private EMACore(BarItemType barItemType, int barCount, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted=null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = barCount;
            this.maxBarIndex = barCount - 1;
            this.smoothingConstant = (double)2 / (barCount + 1);

            this.identityCode = CreateIdentityCode(barCount);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static EMACore CreateInstance(BarItemType barItemType, int barCount, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted=null)
        {
            return new EMACore(barItemType, barCount, onCalculationCompleted);
        }

        public int Period { get { return this.barCount; } }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex))
            {
                double previousEMA = HasValue() ? LastItem.Value : priceAction.Average(barCount);                
                double ema= (priceAction.LastItem.Close * smoothingConstant) + (previousEMA * (1 - smoothingConstant));

                this.Add(new SingleValueIndicatorItem(
                    priceAction.LastItem.Time, 
                    ema //Exponential Moving Average
                    ));

                lastCalculationSuccessful = true;
            }
        }

        public double GetValue(int index = 0)
        {
            return this.Last(index).Value;
        }

        public static string CreateIdentityCode(int periods)
        {
            return string.Format("{0}({1})", SHORT_NAME, periods);
        }
    }
}
