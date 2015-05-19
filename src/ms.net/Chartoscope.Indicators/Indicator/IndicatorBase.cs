using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class IndicatorBase<T> : IndicatorRingBuffer<T>, IAnalyticsIdentity, IPriceActionCalculator, ICalculationDependency
        where T: IIndicatorItem
    {
        protected int barCount = int.MinValue;
        protected int maxBarIndex = 0;

        protected string displayName = null;
        protected string shortName = null;
        protected string description = null;
        protected BarItemType barItemType = null;

        public event IndicatorDelegates.CalculationCompletedHandler Calculated;

        private Dictionary<string, CalculationDependencyItem> dependencies = null;
        private int lastCalculationHash = int.MinValue;
        protected bool lastCalculationSuccessful = false;

        public IndicatorBase(string displayName, string shortName, string description, BarItemType barItemType)
            : base(new DateTimeKeyedIndicator<T>(), 1000)
        {
            this.displayName = displayName;
            this.shortName = shortName;
            this.description = description;
            this.barItemType = barItemType;

            dependencies = new Dictionary<string, CalculationDependencyItem>();
        }

        public double Sum(int barCount)
        {
            return IndicatorCalculator<T>.Sum(this, barCount);
        }

        public double Sum(int barCount, int valueIndex)
        {
            return IndicatorCalculator<T>.Sum(this, valueIndex, barCount);
        }

        public double Average(int barCount)
        {
            return this.Sum(barCount) / barCount;
        }

        public double Average(int barCount, int valueIndex)
        {
            return this.Sum(barCount, valueIndex) / barCount;
        }

        protected string identityCode = null;

        public string IdentityCode
        {
            get { return identityCode; }
        }

        protected string identityName = null;
        public string IdentityName
        {
            get { return identityName; }
        }

        public string Description
        {
            get { return description; }
        }

        public void Calculator(PriceBars priceAction)
        {
            lastCalculationSuccessful = false;
            Calculate(priceAction);
            lastCalculationHash = priceAction.Last().GetHashCode();

            if (lastCalculationSuccessful && Calculated != null)
            {
                Calculated();
            }
        }

        protected virtual void Calculate(PriceBars priceAction)
        {
        }

        public void AddDependency(BarItemType barType, IAnalyticsIdentity identity)
        {
            CalculationDependencyItem item = new CalculationDependencyItem(barType, identity);
            this.dependencies.Add(identity.IdentityCode, item);
        }


        public List<CalculationDependencyItem> GetDependencies()
        {
            return new List<CalculationDependencyItem>(this.dependencies.Values);
        }


        public bool HasCalculated(PriceBars priceAction)
        {
            return lastCalculationHash == priceAction.Last().GetHashCode();
        }

        public bool HasValue(int bufferIndex, int valueIndex)
        {
            return HasValue(bufferIndex) && Last(bufferIndex).Values[valueIndex] != double.NaN;
        }
    }
}
