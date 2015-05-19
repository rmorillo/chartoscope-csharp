using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Droidworks.Common;
using Metadroids.Analytics.Helpers;
using Metadroids.Common.RingBuffers;

namespace Metadroids.Analytics.Models
{
    public class SingleIndicator: SingleValueIndicatorRingBuffer, IAnalyticsIdentity
    {
        protected int barCount = int.MinValue;
        protected TimeframeMode timeframe = TimeframeMode.Undefined;
        protected string displayName = null;
        protected string shortName = null;
        protected string description = null;

        private Dictionary<int, Metadroids.Common.Models.SumItem> lastSums= new Dictionary<int, Metadroids.Common.Models.SumItem>();

        public SingleIndicator(string displayName, string shortName, string description, TimeframeMode timeframe)
            : base(new DateTimeKeyedSingleIndicator(), 300)
        {
            this.displayName = displayName;
            this.shortName = shortName;
            this.description = description;
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
                if (lastSum != double.MinValue && SecondToLast.Timestamp == lastSumDateTime)
                {
                    sumResult = IndicatorCalculator.NextSum(this, lastSum, barCount);
                }
                else
                {
                    sumResult = IndicatorCalculator.Sum(this, barCount);
                }
            }
            else
            {
                sumResult = IndicatorCalculator.Sum(this, barCount);
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
            get { return string.Concat(shortName, "(", barCount.ToString(), ")"); }
        }

        public string DisplayName
        {
            get { return string.Concat(displayName, "(", barCount.ToString(), ")"); }
        }

        public string Description
        {
            get { return description; }
        }
    }
}
