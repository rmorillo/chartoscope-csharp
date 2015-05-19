using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class PriceBars: PricebarRingBuffer
    {
        private BarItemType barItemType = null;

        public PriceBars(BarItemType barItemType)
            : base(new DateTimeKeyedPricebars(), 300)
        {
            this.barItemType = barItemType;
        }

        public double Sum(int barCount)
        {
            return BarItemCalculator.Sum(this, barCount);
        }

        public double Average(int barCount)
        {          
            double sum= this.Sum(barCount);

            if (double.IsNaN(sum))
            {
                return double.NaN;
            }
            else
            {
                return  sum / barCount;
            }
        }

        public double MaximumHigh(int barCount, int startIndex=0)
        {
            return BarItemCalculator.MaximumHigh(this, barCount);
        }


        public double MinimumLow(int barCount, int startIndex = 0)
        {
            return BarItemCalculator.MinimumLow(this, barCount);
        }

        public double TypicalPriceAverage(int barCount)
        {
            return BarItemCalculator.TypicalPriceAverage(this, barCount);
        }
    }
}
