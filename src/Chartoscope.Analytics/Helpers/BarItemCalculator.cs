using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadroids.Common.Models;
using Metadroids.Common.RingBuffers;

namespace Metadroids.Analytics.Helpers
{
    public static class BarItemCalculator
    {
        public static double Sum(IRingNavigator<BarItem> ringNavigator, int barCount)
        {
            double sumResult = 0;
            for (int index = 0; index < barCount; index++)
            {
                sumResult += ringNavigator.Last(index).Close;
            }
            return sumResult;
        }

        public static double NextSum(IRingNavigator<BarItem> ringNavigator, double lastSum, int barCount)
        {
            return (lastSum + ringNavigator.FirstToLast.Close) - ringNavigator.Last(barCount + 1).Close;
        }

        public static double Average(IRingNavigator<BarItem> ringNavigator, int barCount)
        {            
            return Sum(ringNavigator, barCount)/barCount;
        }

        public static double NextAverage(IRingNavigator<BarItem> ringNavigator, double lastSum, int barCount)
        {
            return (NextSum(ringNavigator, lastSum, barCount))/barCount;
        }

        public static double SumOfSquares(IRingNavigator<BarItem> ringNavigator, int barCount)
        {
            double sumResult = 0;
            for (int index = 0; index < barCount; index++)
            {
                sumResult += ringNavigator.Last(index).Close * ringNavigator.Last(index).Close;
            }
            return sumResult;
        }

        public static double NextSumOfSquares(IRingNavigator<BarItem> ringNavigator, double lastSumOfSquare, int barCount)
        {
            return (lastSumOfSquare + (ringNavigator.FirstToLast.Close * ringNavigator.FirstToLast.Close)) - (ringNavigator.Last(barCount + 1).Close * ringNavigator.Last(barCount + 1).Close);
        }
    }
}
