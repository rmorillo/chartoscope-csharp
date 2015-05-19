using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class ParabolicSARCore: IndicatorBase<MultiValueIndicatorItem>
    {
        public const string SHORT_NAME = "psar";
        public const string DISPLAY_NAME = "Parabolic SAR";
        public const string DESCRIPTION = "Parabolic SAR";
        public const string TEST = "ABC";
        //Multi-value field index
        public static readonly int CURRENT_SAR_FIELD_INDEX = 0;
        public static readonly int NEXT_SAR_FIELD_INDEX = 1;
        public static readonly int EP_FIELD_INDEX = 2;
        public static readonly int DELTA_EP_SAR_FIELD_INDEX = 3;
        public static readonly int AF_FIELD_INDEX = 4;
        public static readonly int DIRECTION_FIELD_INDEX = 5;
        public static readonly int DELTA_SAR_FIELD_INDEX = 6;

        private const double MAX_ACCELERATION_FACTOR= 0.2;
        private const double STARTING_ACCELERATION_FACTOR = 0.02;

        private ParabolicSARCore(BarItemType barItemType, int periods, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
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

        public static ParabolicSARCore CreateInstance(BarItemType barItemType, int periods, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new ParabolicSARCore(barItemType, periods, onCalculationCompleted);
        }

        private double GetDirection(double previousDirection, double currentLow, double currentHigh, double currentSAR)
        {
            if (previousDirection == 1)
            {
                if (currentLow > currentSAR)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (previousDirection == -1)
                {
                    if (currentHigh < currentSAR)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else // not in orginal formula because value must not be 0
                {
                    return double.NaN;
                }
            }
        }

        private double GetCurrentSAR(double previousSAR, double previousExtremePrice, double previousDirection, double secondPreviousDirection, double previousAFDiffProduct,
                double previousLow, double secondPreviousLow, double previousHigh, double secondPreviousHigh)
        {
            return GetNextSAR(previousSAR, previousExtremePrice, previousDirection, secondPreviousDirection, previousAFDiffProduct,
                previousLow, secondPreviousLow, previousHigh, secondPreviousHigh);
        }

        private double GetNextSAR(double currentSAR, double currentExtremePrice, double currentDirection, double previousDirection, double currentAFDiffProduct,
                double currentLow, double previousLow, double currentHigh, double previousHigh)
        {
            if (currentDirection == previousDirection)
            {
                if (currentDirection == 1)
                {
                    if ((currentSAR + currentAFDiffProduct) < Math.Min(previousLow, currentLow))
                    {
                        return currentSAR + currentAFDiffProduct;
                    }
                    else
                    {
                        return Math.Min(previousLow, currentLow);
                    }
                }
                else
                {
                    if ((currentSAR + currentAFDiffProduct) > Math.Max(previousHigh, currentHigh))
                    {
                        return currentSAR + currentAFDiffProduct;
                    }
                    else
                    {
                        return Math.Max(previousHigh, currentHigh);
                    }
                }
            }
            else
            {
                return currentExtremePrice;
            }
        }

        private double GetAccelarationFactor(double direction, double previousDirection,  double extremePrice, double previousExtremePrice, 
            double previousAccelFactor)
        {
            if (direction == previousDirection)
            {
                if (direction == 1)
                {
                    if (extremePrice > previousExtremePrice)
                    {
                        if (previousAccelFactor == MAX_ACCELERATION_FACTOR)
                        {
                            return previousAccelFactor;
                        }
                        else
                        {
                            return STARTING_ACCELERATION_FACTOR + previousAccelFactor;
                        }
                    }
                    else
                    {
                        return previousAccelFactor;
                    }
                }
                else
                {
                    if (extremePrice < previousExtremePrice)
                    {
                        if (previousAccelFactor == MAX_ACCELERATION_FACTOR)
                        {
                            return previousAccelFactor;
                        }
                        else
                        {
                            return STARTING_ACCELERATION_FACTOR + previousAccelFactor;
                        }
                    }
                    else
                    {
                        return previousAccelFactor;
                    }
                }
            }
            else
            {
                return STARTING_ACCELERATION_FACTOR;
            }
        }

        private double GetExtremePrice(double previousDirection, double currentHigh, double previousExtremePrice, double currentLow)
        {
            if (previousDirection == 1)
            {
                if (currentHigh > previousExtremePrice)
                {
                    return currentHigh;
                }
                else
                {
                    return previousExtremePrice;
                }
            }
            else
            {
                if (currentLow < previousExtremePrice)
                {
                    return currentLow;
                }
                else
                {
                    return previousExtremePrice;
                }
            }
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex + 1)) // + 1 to accommodate for previous extreme price
            {                
                double previousExtremePrice= HasValue()? LastItem.Values[EP_FIELD_INDEX]: priceAction.MaximumHigh(barCount, 1);
                double previousDirection = HasValue() ? LastItem.Values[DIRECTION_FIELD_INDEX] : 1;
                double previousAcclerationFactor = HasValue() ? LastItem.Values[AF_FIELD_INDEX] : 0;

                double sar = HasValue()? LastItem.Values[NEXT_SAR_FIELD_INDEX]: priceAction.MinimumLow(barCount);

                double extremePrice = HasValue()? GetExtremePrice(previousDirection, priceAction.LastItem.High, previousExtremePrice,
                    priceAction.LastItem.Low) :priceAction.MaximumHigh(barCount);
                double epSARDelta = extremePrice - sar;                   
                double direction = GetDirection(previousDirection, priceAction.LastItem.Low, priceAction.LastItem.High, sar);               
                double acclerationFactor = GetAccelarationFactor(direction, previousDirection, extremePrice, previousExtremePrice, previousAcclerationFactor);
                double afDiffProduct = acclerationFactor * epSARDelta;
                double deltaSAR = HasValue() ? sar / LastItem.Values[CURRENT_SAR_FIELD_INDEX] : 0;

                double nextSAR = GetNextSAR(sar, extremePrice, direction, previousDirection, afDiffProduct, priceAction.LastItem.Low,
                    priceAction.Last(1).Low, priceAction.LastItem.High, priceAction.Last(1).High);

                this.Add(new MultiValueIndicatorItem(
                    priceAction.LastItem.Time,
                    sar,
                    nextSAR,
                    extremePrice,
                    epSARDelta,
                    acclerationFactor,
                    direction,
                    deltaSAR
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
