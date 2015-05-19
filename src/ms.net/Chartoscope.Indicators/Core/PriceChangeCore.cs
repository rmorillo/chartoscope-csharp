using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class PriceChangeCore: IndicatorBase<MultiValueIndicatorItem>
    {
        public static readonly string SHORT_NAME = "pch";
        public static readonly string DISPLAY_NAME = "PriceChange";
        public static readonly string DESCRIPTION = "Price Change (Up/Down)";

        private PriceChangeCore(BarItemType barItemType, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
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

        public static PriceChangeCore CreateInstance(BarItemType barItemType, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new PriceChangeCore(barItemType, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex))
            {       
                double change= priceAction.LastItem.Close-priceAction.Last(1).Close;
         
                this.Add(new MultiValueIndicatorItem(
                    priceAction.LastItem.Time,
                    change, //Raw change
                    change>0? change: 0, //Change Up
                    change<=0? Math.Abs(change): 0 //Change down
                    ));
                lastCalculationSuccessful = true;
            }
        }

        public static string CreateIdentityCode()
        {
            return string.Format("{0}(2)", SHORT_NAME);
        }

        public double RawChange(int index = 0)
        {
            return this.Last(index).Values[0];
        }

        public double ChangeUp(int index = 0)
        {
            return this.Last(index).Values[1];
        }

        public double ChangeDown(int index = 0)
        {
            return this.Last(index).Values[2];
        }
    }
}
