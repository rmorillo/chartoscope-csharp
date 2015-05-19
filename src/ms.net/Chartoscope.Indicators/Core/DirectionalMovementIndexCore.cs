using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class DirectionalMovementIndexCore: IndicatorBase<MultiValueIndicatorItem>
    {
        public const string SHORT_NAME = "dx";
        public const string DISPLAY_NAME = "DirectionalMovementIndex";
        public const string DESCRIPTION = "Directional Movement Index";

        //Multi-value field index
        public static readonly int DX_FIELD_INDEX = 0;
        public static readonly int SMOOTH_TR_FIELD_INDEX = 1;
        public static readonly int SMOOTH_PLUS_DM_FIELD_INDEX = 2;
        public static readonly int SMOOTH_MINUS_DM_INDEX = 3;
        public static readonly int PLUS_DI_FIELD_INDEX = 4;
        public static readonly int MINUS_DI_FIELD_INDEX = 5;

        private TrueRangeCore trueRange = null;
        private DirectionalMovementCore directionalMovement = null;


        private DirectionalMovementIndexCore(BarItemType barItemType, int periods, TrueRangeCore trueRangeCore, DirectionalMovementCore directionalMovementCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = periods;
            this.maxBarIndex = barCount - 1;         

            this.AddDependency(barItemType, directionalMovementCore);
            this.AddDependency(barItemType, trueRangeCore);

            this.directionalMovement = directionalMovementCore;
            this.trueRange = trueRangeCore;

            this.identityCode = CreateIdentityCode(periods);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static DirectionalMovementIndexCore CreateInstance(BarItemType barItemType, int periods, TrueRangeCore trueRangeCore, DirectionalMovementCore directionalMovementCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new DirectionalMovementIndexCore(barItemType, periods, trueRangeCore, directionalMovementCore, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (trueRange.HasValue(maxBarIndex) && directionalMovement.HasValue(maxBarIndex))
            {
                double smoothSumOfTrueRange = HasValue(0, SMOOTH_TR_FIELD_INDEX) ? LastItem.Values[SMOOTH_TR_FIELD_INDEX] - (LastItem.Values[SMOOTH_TR_FIELD_INDEX] / barCount) + trueRange.LastItem.Value : trueRange.Sum(barCount);
                double smoothSumOfPlusDM = HasValue(0, SMOOTH_PLUS_DM_FIELD_INDEX) ? LastItem.Values[SMOOTH_PLUS_DM_FIELD_INDEX] - (LastItem.Values[SMOOTH_PLUS_DM_FIELD_INDEX] / barCount) + directionalMovement.LastItem.Values[DX_FIELD_INDEX] : directionalMovement.Sum(barCount, 0);
                double smoothSumOfMinusDM = HasValue(0, SMOOTH_MINUS_DM_INDEX) ? LastItem.Values[SMOOTH_MINUS_DM_INDEX] - (LastItem.Values[SMOOTH_MINUS_DM_INDEX] / barCount) + directionalMovement.LastItem.Values[SMOOTH_TR_FIELD_INDEX] : directionalMovement.Sum(barCount, 1);
                double plusDI = (smoothSumOfPlusDM / smoothSumOfTrueRange) * 100;
                double minusDI = (smoothSumOfMinusDM / smoothSumOfTrueRange) * 100;
                double diDiff = Math.Abs(plusDI - minusDI);
                double diSum = plusDI + minusDI;
                double dx = (diDiff / diSum) * 100;                

                this.Add(new MultiValueIndicatorItem(
                    priceAction.LastItem.Time,
                    dx,
                    smoothSumOfTrueRange,
                    smoothSumOfPlusDM,
                    smoothSumOfMinusDM,
                    plusDI,
                    minusDI
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
