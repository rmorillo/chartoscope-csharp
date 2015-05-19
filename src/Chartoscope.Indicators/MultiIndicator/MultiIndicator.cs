using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Droidworks.Common;

namespace Droidworks.Indicators
{
    public class MultiIndicator: MultiValueIndicatorRingBuffer, IAnalyticsIdentity, IPriceActionCalculator, ICalculationDependency
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
        protected bool lastCalculationSuccessful= false;

        public MultiIndicator(string displayName, string shortName, string description, BarItemType barItemType)
            : base(new DateTimeKeyedMultiIndicator(), 300)
        {
            this.displayName = displayName;
            this.shortName = shortName;
            this.description = description;
            this.barItemType = barItemType;

            dependencies = new Dictionary<string, CalculationDependencyItem>();
        }

        public double Sum(int barCount)
        {
            return IndicatorCalculator.Sum(this, 0, barCount);               
        }

        public double Average(int barCount)
        {
            return this.Sum(barCount) / barCount;
        }

        public string UniqueShortName
        {
            get { return BuildUniqueShortName(); }
        }

        protected virtual string BuildUniqueShortName()
        {
            return string.Concat(shortName, "(", barCount.ToString(), ")");
        }

        public string DisplayName
        {
            get { return string.Concat(displayName, "(", barCount.ToString(), ")"); }
        }

        public string Description
        {
            get { return description ; }
        }

        public void Calculator(PriceBars priceAction)
        {
            lastCalculationSuccessful = false;
            Calculate(priceAction);
            lastCalculationHash = priceAction.Last().GetHashCode();

            if (lastCalculationSuccessful && Calculated!=null)
            {
                Calculated();
            }
        }

        protected virtual void Calculate(PriceBars priceAction)
        {
        }

        public void AddDependency(BarItemType barType, IAnalyticsIdentity identity)
        {
            CalculationDependencyItem item= new CalculationDependencyItem(barType, identity);
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
