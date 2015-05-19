using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class TrueRangeCore: IndicatorBase<SingleValueIndicatorItem>
    {
        public static readonly string SHORT_NAME = "tr";
        public static readonly string DISPLAY_NAME = "TrueRange";
        public static readonly string DESCRIPTION = "True Range";

        private TrueRangeCore(BarItemType barItemType, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = 2;
            this.maxBarIndex = barCount - 1;

            this.identityCode = CreateIdentityCode();

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static TrueRangeCore CreateInstance(BarItemType barItemType, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new TrueRangeCore(barItemType, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex))
            {
                BarItem currentPrice = priceAction.LastItem;
                BarItem previousPrice = priceAction.Last(1);
                
                double trueRange= Math.Max(currentPrice.High-currentPrice.Low, 
                    Math.Max(Math.Abs(currentPrice.High-previousPrice.Close),Math.Abs(currentPrice.Low-previousPrice.Close)));
         
                this.Add(new SingleValueIndicatorItem(
                    priceAction.LastItem.Time,
                    trueRange
                    ));
                lastCalculationSuccessful = true;
            }
        }

        public static string CreateIdentityCode()
        {
            return string.Format("{0}(2)", SHORT_NAME);
        }        
    }
}
