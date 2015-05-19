using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class IndicatorCalculator<T> where T: IIndicatorItem
    {
        public static double Sum(IRingNavigator<T> ringNavigator, int barCount)
        {
            double sumResult = 0;
            for (int index = 0; index < barCount; index++)
            {
                sumResult += ringNavigator.Last(index).Value;
            }
            return sumResult;
        }

        public static double Sum(IRingNavigator<T> ringNavigator, int valueIndex, int barCount)
        {
            double sumResult = 0;
            for (int index = 0; index < barCount; index++)
            {
                sumResult += ringNavigator.Last(index).Values[valueIndex];
            }
            return sumResult;
        }

        public static double Average(IRingNavigator<T> ringNavigator, int barCount)
        {            
            return Sum(ringNavigator, barCount)/barCount;
        }
    }
}
