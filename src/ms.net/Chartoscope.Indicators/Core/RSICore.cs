using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class RSICore: IndicatorBase<SingleValueIndicatorItem>
    {
        public const string SHORT_NAME = "rsi";
        public const string DISPLAY_NAME = "RSI";
        public const string DESCRIPTION = "RSI";        

        private PriceChangeCore priceChangeCore = null;

        private RSICore(BarItemType barItemType, int periods, PriceChangeCore priceChangeCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = periods;
            this.maxBarIndex = periods - 1;      

            this.AddDependency(barItemType, priceChangeCore);

            this.priceChangeCore = priceChangeCore;

            this.identityCode = CreateIdentityCode(periods);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static RSICore CreateInstance(BarItemType barItemType, int periods, PriceChangeCore priceChangeCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new RSICore(barItemType, periods, priceChangeCore, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceChangeCore.HasValue(maxBarIndex))
            {
                double averageChangeUp = priceChangeCore.Average(barCount, 1);
                double averageChangeDown = priceChangeCore.Average(barCount, 2);                

                this.Add(new SingleValueIndicatorItem(
                    priceAction.LastItem.Time,
                    100-(100/(1 + (averageChangeUp/averageChangeDown)))
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
