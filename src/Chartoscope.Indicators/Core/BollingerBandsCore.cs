using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class BollingerBandsCore: IndicatorBase<MultiValueIndicatorItem>
    {
        public const string SHORT_NAME = "bb";
        public const string DISPLAY_NAME = "BollingerBands";
        public const string DESCRIPTION = "Bollinger Bands";

        //Multi-value field index
        public static readonly int UPPER_BAND_FIELD_INDEX = 0;
        public static readonly int MIDDLE_BAND_FIELD_INDEX = 1;
        public static readonly int LOWER_BAND_FIELD_INDEX = 2;

        private SMACore smaCore = null;
        private double stdMultiplier = 2;

        private BollingerBandsCore(BarItemType barItemType, int barCount, SMACore smaCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted=null, double stdMultiplier = 2)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = barCount;
            this.maxBarIndex = barCount - 1;
            this.stdMultiplier = stdMultiplier;

            this.AddDependency(barItemType, smaCore);

            this.smaCore = smaCore;

            this.identityCode = CreateIdentityCode(barCount);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static BollingerBandsCore CreateInstance(BarItemType barItemType, int barCount, SMACore smaCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted=null, double stdMultiplier=2)
        {
            return new BollingerBandsCore(barItemType, barCount, smaCore, onCalculationCompleted, stdMultiplier);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex) && smaCore.HasValue(maxBarIndex))
            {
                BarItem lastBar= priceAction.LastItem;

                double lastSMA= this.smaCore.LastItem.Value;
                double sumOfDeviations= 0;
                for(int index=0; index<barCount; index++)
                {
                    sumOfDeviations+= Math.Pow(priceAction.Last(index).Close-smaCore.Last(index).Value,2);
                }
                double std= Math.Sqrt(sumOfDeviations/barCount);

                this.Add(new MultiValueIndicatorItem(
                    lastBar.Time,
                    lastSMA + (stdMultiplier * std), //Upper band
                    lastSMA, //Middle band
                    lastSMA - (stdMultiplier * std) // Lower band
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
