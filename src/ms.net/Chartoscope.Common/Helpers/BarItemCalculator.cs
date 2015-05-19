using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class BarItemCalculator
    {
        public static double Sum(IRingNavigator<BarItem> ringNavigator, int barCount)
        {
            double sumResult = double.NaN;

            if (ringNavigator.HasValue(barCount-1))
            {            
                sumResult = 0;
                for (int index = 0; index < barCount; index++)
                {
                    sumResult += ringNavigator.Last(index).Close;
                }            
            }

            return sumResult;
        }


        public static double Average(IRingNavigator<BarItem> ringNavigator, int barCount)
        {            
            return Sum(ringNavigator, barCount)/barCount;
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

        public static double MaximumHigh(IRingNavigator<BarItem> ringNavigator, int barCount, int startIndex=0)
        {
            double max = ringNavigator.Last(startIndex).High;
            for (int index = startIndex; index < (barCount + startIndex); index++)
            {
                if (max < ringNavigator.Last(index).High)
                {
                    max = ringNavigator.Last(index).High;
                }
            }
            return max;
        }

        public static double MinimumLow(IRingNavigator<BarItem> ringNavigator, int barCount, int startIndex = 0)
        {
            double min = ringNavigator.Last(startIndex).Low;
            for (int index = startIndex; index < (barCount + startIndex); index++)
            {
                if (min > ringNavigator.Last(index).Low)
                {
                    min = ringNavigator.Last(index).Low;
                }
            }
            return min;
        }

        public static double TypicalPriceSum(IRingNavigator<BarItem> ringNavigator, int barCount)
        {
            double sumResult = double.NaN;

            if (ringNavigator.HasValue(barCount - 1))
            {
                sumResult = 0;
                for (int index = 0; index < barCount; index++)
                {
                    sumResult += ringNavigator.Last(index).TypicalPrice();
                }
            }

            return sumResult;
        }

        public static double TypicalPriceAverage(IRingNavigator<BarItem> ringNavigator, int barCount)
        {
            return TypicalPriceSum(ringNavigator, barCount) / barCount;
        }
    }
}
