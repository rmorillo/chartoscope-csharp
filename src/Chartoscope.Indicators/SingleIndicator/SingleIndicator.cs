using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Droidworks.Common;

namespace Droidworks.Indicators
{
    public class SingleIndicator : SingleValueIndicatorRingBuffer, IAnalyticsIdentity, IPriceActionCalculator, ICalculationDependency
    {
        protected int barCount = int.MinValue;
        protected int maxBarIndex = 0;

        protected BarItemType barItemType = null;
        protected string displayName = null;
        protected string shortName = null;
        protected string description = null;
        private int lastCalculationHash = int.MinValue;
        protected bool lastCalculationSuccessful = false;

        public event IndicatorDelegates.CalculationCompletedHandler Calculated;

        private Dictionary<int, SumItem> lastSums= new Dictionary<int, SumItem>();

        private Dictionary<string, CalculationDependencyItem> dependencies = null;

        public SingleIndicator(string displayName, string shortName, string description, BarItemType barItemType)
            : base(new DateTimeKeyedSingleIndicator(), 300)
        {
            this.displayName = displayName;
            this.shortName = shortName;
            this.description = description;
            this.barItemType = barItemType;

            dependencies = new Dictionary<string, CalculationDependencyItem>();
        }

        public double Sum(int barCount)
        {
            return IndicatorCalculator.Sum(this, barCount);           
        }

        public double Average(int barCount)
        {
            return this.Sum(barCount) / barCount;
        }

        public static string CreateUniqueName(string name, int bars)
        {
            return string.Concat(name, "(", bars.ToString(), ")");
        }

        public string UniqueShortName
        {
            get { return CreateUniqueName(shortName, barCount); }
        }

        public string DisplayName
        {
            get { return string.Concat(displayName, "(", barCount.ToString(), ")"); }
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

        public IAnalyticsIdentity Parent { get; set; }

        public void AddDependency(BarItemType barType, IAnalyticsIdentity identity)
        {
            CalculationDependencyItem item = new CalculationDependencyItem(barType, identity);
            this.dependencies.Add(identity.UniqueShortName, item);
        }


        public List<CalculationDependencyItem> GetDependencies()
        {
            return new List<CalculationDependencyItem>(this.dependencies.Values);
        }


        public bool HasCalculated(PriceBars priceAction)
        {
            return lastCalculationHash == priceAction.Last().GetHashCode();
        }
    }
}
