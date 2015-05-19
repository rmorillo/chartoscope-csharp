using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class ADXCore: IndicatorBase<MultiValueIndicatorItem>
    {
        public const string SHORT_NAME = "adx";
        public const string DISPLAY_NAME = "Average Directional Index";
        public const string DESCRIPTION = "Average Directional Index";

        //Multi-value field index
        public static readonly int ADX_FIELD_INDEX = 0;
        public static readonly int PLUS_DI_FIELD_INDEX = 1;
        public static readonly int MINUS_DI_FIELD_INDEX = 2;

        private DirectionalMovementIndexCore dxCore = null;

        private ADXCore(BarItemType barItemType, int periods, DirectionalMovementIndexCore directionalMovementIndexCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = periods;
            this.maxBarIndex = periods - 1;      

            this.AddDependency(barItemType, directionalMovementIndexCore);

            this.dxCore = directionalMovementIndexCore;

            this.identityCode = CreateIdentityCode(periods);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static ADXCore CreateInstance(BarItemType barItemType, int periods, DirectionalMovementIndexCore directionalMovementIndexCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new ADXCore(barItemType, periods, directionalMovementIndexCore, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (dxCore.HasValue(maxBarIndex))
            {                       
                double average= dxCore.Average(barCount);

                this.Add(new MultiValueIndicatorItem(
                    priceAction.LastItem.Time,     
                    average,
                    dxCore.LastItem.Values[DirectionalMovementIndexCore.PLUS_DI_FIELD_INDEX],
                    dxCore.LastItem.Values[DirectionalMovementIndexCore.MINUS_DI_FIELD_INDEX]
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
