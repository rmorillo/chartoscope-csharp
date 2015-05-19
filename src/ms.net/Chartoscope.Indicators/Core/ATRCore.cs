using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class ATRCore: IndicatorBase<SingleValueIndicatorItem>
    {
        public static readonly string SHORT_NAME = "atr";
        public static readonly string DISPLAY_NAME = "AverageTrueRange";
        public static readonly string DESCRIPTION = "Average True Range";        

        private TrueRangeCore trueRangeCore = null;

        private ATRCore(BarItemType barItemType, int periods, TrueRangeCore trueRangeCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = periods;
            this.maxBarIndex = periods - 1;      

            this.AddDependency(barItemType, trueRangeCore);

            this.trueRangeCore = trueRangeCore;

            this.identityCode = CreateIdentityCode(periods);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static ATRCore CreateInstance(BarItemType barItemType, int periods, TrueRangeCore trueRangeCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new ATRCore(barItemType, periods, trueRangeCore, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (trueRangeCore.HasValue(maxBarIndex))
            {
                double smoothAverage = HasValue(0) ? ((LastItem.Value * (barCount - 1)) + trueRangeCore.LastItem.Value) / barCount : trueRangeCore.Average(barCount);

                this.Add(new SingleValueIndicatorItem(
                    priceAction.LastItem.Time,
                    smoothAverage
                    ));
                lastCalculationSuccessful = true;
            }
        }

        public static string CreateIdentityCode(int periods)
        {
            return string.Format("{0}({1})", SHORT_NAME, periods);
        }       
    }
}
