using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class HighsAndLowsCore: IndicatorBase<MultiValueIndicatorItem>
    {
        public static readonly string SHORT_NAME = "h&l";
        public static readonly string DISPLAY_NAME = "Period Highs-Period Lows";
        public static readonly string DESCRIPTION = "Period Highs-Period Lows";


        private HighsAndLowsCore(BarItemType barItemType, int periods, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = periods;
            this.maxBarIndex = barCount - 1;

            this.identityCode = CreateIdentityCode(periods);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static HighsAndLowsCore CreateInstance(BarItemType barItemType, int periods, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new HighsAndLowsCore(barItemType, periods, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex))
            {                
                this.Add(new MultiValueIndicatorItem(
                    priceAction.LastItem.Time,
                    priceAction.MaximumHigh(barCount),
                    priceAction.MinimumLow(barCount)
                    ));
                lastCalculationSuccessful = true;
            }
        }

        public static string CreateIdentityCode(int periods)
        {
            return string.Format("{0}({1})", SHORT_NAME, periods);
        }

        public double PeriodsHigh(int index = 0)
        {
            return this.Last(index).Values[0];
        }

        public double PeriodsLow(int index = 0)
        {
            return this.Last(index).Values[1];
        }
    }
}
