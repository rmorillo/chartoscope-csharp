using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class SMACore: IndicatorBase<SingleValueIndicatorItem>
    {
        public const string SHORT_NAME= "sma";
        public const string DISPLAY_NAME = "SimpleMovingAverage";
        public const string DESCRIPTION = "Simple Moving Average";

        private SMACore(BarItemType barItemType, int barCount, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted=null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = barCount;
            this.maxBarIndex = barCount - 1;

            this.identityCode = CreateIdentityCode(barCount);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static SMACore CreateInstance(BarItemType barItemType, int barCount, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted=null)
        {
            return new SMACore(barItemType, barCount, onCalculationCompleted);
        }

        public static string CreateIdentityCode(int periods)
        {
            return string.Format("{0}({1})", SHORT_NAME, periods);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex))
            {                
                this.Add(new SingleValueIndicatorItem(
                    priceAction.LastItem.Time, 
                    priceAction.Average(barCount) //Simple Moving Average
                    ));

                lastCalculationSuccessful = true;
            }
        }       
    }
}
