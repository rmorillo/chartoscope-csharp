using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class CCICore: IndicatorBase<SingleValueIndicatorItem>
    {
        public static readonly string SHORT_NAME= "cci";
        public static readonly string DISPLAY_NAME= "Commodity Channel Index";
        public static readonly string DESCRIPTION= "Commodity Channel Index";

        private TPMACore tpmaCore = null;

        private CCICore(BarItemType barItemType, int barCount, TPMACore tpmaCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = barCount;
            this.maxBarIndex = barCount - 1;

            this.tpmaCore = tpmaCore;

            this.identityCode = CreateIdentityCode(barCount);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static CCICore CreateInstance(BarItemType barItemType, int barCount, TPMACore tpmaCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new CCICore(barItemType, barCount, tpmaCore, onCalculationCompleted);
        }

        public static string CreateIdentityCode(int periods)
        {
            return string.Format("{0}({1})", SHORT_NAME, periods);
        }

        private double GetMeanDeviation(PriceBars priceAction)
        {
            double sumOfDelta = 0;
            for (int index = 0; index < barCount; index++)
            {
                sumOfDelta += Math.Abs(tpmaCore.Last(index).Value - priceAction.Last(index).TypicalPrice());
            }

            return sumOfDelta / barCount;
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex) && tpmaCore.HasValue(maxBarIndex))
            {
                double smatp = tpmaCore.LastItem.Value;
                double meanDeviation = GetMeanDeviation(priceAction);
                double cci= (priceAction.LastItem.TypicalPrice() -smatp) / (0.015 * meanDeviation);
                this.Add(new SingleValueIndicatorItem(
                    priceAction.LastItem.Time, 
                    cci
                    ));

                lastCalculationSuccessful = true;
            }
        }     
    }
}
