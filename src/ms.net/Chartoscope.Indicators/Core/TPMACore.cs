using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class TPMACore: IndicatorBase<SingleValueIndicatorItem>
    {
        public static readonly string SHORT_NAME= "tpma";
        public static readonly string DISPLAY_NAME= "TypicalPriceMovingAverage";
        public static readonly string DESCRIPTION= "Typical Price Moving Average";


        private TPMACore(BarItemType barItemType, int barCount, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
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

        public static TPMACore CreateInstance(BarItemType barItemType, int barCount, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new TPMACore(barItemType, barCount, onCalculationCompleted);
        }

        public int Period { get { return this.barCount; } }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex))
            {                    
                this.Add(new SingleValueIndicatorItem(
                    priceAction.LastItem.Time, 
                    priceAction.TypicalPriceAverage(barCount) //TP Moving Average
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
