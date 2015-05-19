using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Metadroids.Analytics.Helpers;
using Metadroids.Common.Enumerations;
using Metadroids.Common.Models;
using Metadroids.Common.RingBuffers;

namespace Metadroids.Analytics.Models
{
    public class PriceAction: PricebarRingBuffer
    {
        private TimeframeMode timeframe = TimeframeMode.Undefined;

        private Dictionary<int, SumItem> lastSums= new Dictionary<int, SumItem>();

        public PriceAction(TimeframeMode timeframe): base(new DateTimeKeyedPricebars(), 300)
        {
            this.timeframe = timeframe;
        }

        public double Sum(int barCount)
        {
            double sumResult = double.MinValue;
            double lastSum = double.MinValue;
            DateTime lastSumDateTime = DateTime.MinValue;
            if (lastSums.ContainsKey(barCount))
            {
                lastSum = lastSums[barCount].Sum;
                lastSumDateTime = lastSums[barCount].Timestamp;
                if (lastSum != double.MinValue && SecondToLast.Time == lastSumDateTime)
                {
                    sumResult = BarItemCalculator.NextSum(this, lastSum, barCount);
                }
                else
                {
                    sumResult = BarItemCalculator.Sum(this, barCount);
                }
            }
            else
            {
                sumResult = BarItemCalculator.Sum(this, barCount);
                lastSums.Add(barCount, new SumItem(sumResult, FirstToLast.Time));
            }

            lastSums[barCount].Set(sumResult, FirstToLast.Time);

            return sumResult;
        }

        public double Average(int barCount)
        {
            return this.Sum(barCount) / barCount;
        }
    }
}
