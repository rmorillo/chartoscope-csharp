using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class DirectionalMovementCore: IndicatorBase<MultiValueIndicatorItem>
    {
        public static readonly string SHORT_NAME = "dm";
        public static readonly string DISPLAY_NAME = "DirectionalMovement";
        public static readonly string DESCRIPTION = "Directional Movement";

        private DirectionalMovementCore(BarItemType barItemType, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
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

        public static DirectionalMovementCore CreateInstance(BarItemType barItemType, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new DirectionalMovementCore(barItemType, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex))
            {
                BarItem currentPrice = priceAction.LastItem;
                BarItem previousPrice = priceAction.Last(1);

                double plusDM = (currentPrice.High - previousPrice.High) > (previousPrice.Low - currentPrice.Low) ? Math.Max(currentPrice.High - previousPrice.High, 0) : 0;
                double minusDM = (previousPrice.Low - currentPrice.Low) > (currentPrice.High - previousPrice.High) ? Math.Max(previousPrice.Low - currentPrice.Low, 0) : 0;
         
                this.Add(new MultiValueIndicatorItem(
                    priceAction.LastItem.Time,
                    plusDM, 
                    minusDM
                    ));
                lastCalculationSuccessful = true;
            }
        }

        public static string CreateIdentityCode()
        {
            return string.Format("{0}(2)", SHORT_NAME);
        }     

        public double PositiveMovement(int index = 0)
        {
            return this.Last(index).Values[0];
        }

        public double NegativeMovement(int index = 0)
        {
            return this.Last(index).Values[1];
        }
    }
}
