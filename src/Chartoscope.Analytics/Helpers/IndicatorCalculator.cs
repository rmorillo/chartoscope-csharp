using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadroids.Common.Models;
using Metadroids.Common.RingBuffers;

namespace Metadroids.Analytics.Helpers
{
    public static class IndicatorCalculator
    {
        public static double Sum(IRingNavigator<SingleValueIndicator> ringNavigator, int barCount)
        {
            double sumResult = 0;
            for (int index = 0; index < barCount; index++)
            {
                sumResult += ringNavigator.Last(index).Value;
            }
            return sumResult;
        }

        public static double Sum(IRingNavigator<MultiValueIndicator> ringNavigator, int valueIndex, int barCount)
        {
            double sumResult = 0;
            for (int index = 0; index < barCount; index++)
            {
                sumResult += ringNavigator.Last(index).Values[valueIndex];
            }
            return sumResult;
        }

        public static double NextSum(IRingNavigator<SingleValueIndicator> ringNavigator, double lastSum, int barCount)
        {
            return (lastSum + ringNavigator.FirstToLast.Value) - ringNavigator.Last(barCount + 1).Value;
        }

        public static double NextSum(IRingNavigator<MultiValueIndicator> ringNavigator, int valueIndex, double lastSum, int barCount)
        {
            return (lastSum + ringNavigator.FirstToLast.Values[valueIndex]) - ringNavigator.Last(barCount + 1).Values[valueIndex];
        }

        public static double Average(IRingNavigator<SingleValueIndicator> ringNavigator, int barCount)
        {            
            return Sum(ringNavigator, barCount)/barCount;
        }

        public static double NextAverage(IRingNavigator<SingleValueIndicator> ringNavigator, double lastSum, int barCount)
        {
            return (NextSum(ringNavigator, lastSum, barCount))/barCount;
        }
    }
}
