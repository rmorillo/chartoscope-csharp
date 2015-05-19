using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class BarItemExtension
    {
        public static double TypicalPrice(this BarItem barItem)
        {
            return (barItem.High + barItem.Low + barItem.Close) / 3;
        }

        public static bool BearishCandle(this BarItem barItem)
        {
            return barItem == null ? false : barItem.Close < barItem.Open;
        }

        public static bool BullishCandle(this BarItem barItem)
        {
            return barItem==null? false: barItem.Close > barItem.Open;
        }

        public static double Length(this BarItem barItem)
        {
            return barItem == null ? double.NaN : barItem.High-barItem.Low;
        }
    }
}
