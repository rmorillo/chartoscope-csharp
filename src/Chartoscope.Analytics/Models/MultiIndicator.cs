using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Droidworks.Common;
using Metadroids.Analytics.Helpers;
using Metadroids.Common.RingBuffers;

namespace Metadroids.Analytics.Models
{
    public class MultiIndicator: MultiValueIndicatorRingBuffer, IAnalyticsIdentity
    {

        protected int barCount = int.MinValue;
        protected string displayName = null;
        protected string shortName = null;
        protected string description = null;

        private Dictionary<int, Metadroids.Common.Models.SumItem> lastSums = new Dictionary<int, Metadroids.Common.Models.SumItem>();

        public MultiIndicator(string displayName, string shortName, string description)
            : base(new DateTimeKeyedMultiIndicator(), 300)
        {
            this.displayName = displayName;
            this.shortName = shortName;
            this.description = description;
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
                if (lastSum != double.MinValue && SecondToLast.Timestamp == lastSumDateTime)
                {
                    sumResult = IndicatorCalculator.NextSum(this, 0, lastSum, barCount);
                }
                else
                {
                    sumResult = IndicatorCalculator.Sum(this, 0, barCount);
                }
            }
            else
            {
                sumResult = IndicatorCalculator.Sum(this, 0, barCount);
                lastSums.Add(barCount, new Metadroids.Common.Models.SumItem(sumResult, FirstToLast.Timestamp));
            }

            lastSums[barCount].Set(sumResult, FirstToLast.Timestamp);

            return sumResult;
        }

        public double Average(int barCount)
        {
            return this.Sum(barCount) / barCount;
        }

        public string UniqueShortName
        {
            get { return string.Concat(shortName,"(", barCount.ToString(), ")"); }
        }

        public string DisplayName
        {
            get { return string.Concat(displayName, "(", barCount.ToString(), ")"); }
        }

        public string Description
        {
            get { return description ; }
        }
    }
}
