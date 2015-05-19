using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Indicators;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public class SessionIndicators
    {
        private Dictionary<string, SessionIndicator> indicators = null;     
   
        public SessionIndicators()
        {
            indicators = new Dictionary<string, SessionIndicator>();
        }

        public void Add<TIndicator>(TIndicator indicator) where TIndicator: IIndicatorCore
        {
            string barTypeCode = indicator.BarType.Code;

            if (!indicators.ContainsKey(barTypeCode))
            {
                indicators.Add(barTypeCode, new SessionIndicator(indicator.BarType));               
            }

            ((IIndicatorCore)indicator).BuildCore(indicators[barTypeCode]);
        }

        

        public void ReceivePriceAction(BarItemType barType, PriceBars priceAction)
        {
            foreach(CoreIndicator indicator in indicators[barType.Code].CoreIndicators.Values)
            {
                RecurseCalculationDependencies(indicator, priceAction);              
            }
        }

        public void RecurseCalculationDependencies(ICoreIndicator coreIndicator, PriceBars priceAction)
        {
            foreach (CalculationDependencyItem dependency in coreIndicator.Dependencies)
            {
                ICoreIndicator subIndicator= indicators[dependency.BarType.Code].CoreIndicators[dependency.Identity.IdentityCode];
                List<CalculationDependencyItem> subDependencies = subIndicator.Dependencies;
                if (subDependencies.Count > 0)
                {
                    RecurseCalculationDependencies(subIndicator, priceAction);
                }                
            }

            if (!coreIndicator.PriceAction.HasCalculated(priceAction))
            {
                coreIndicator.PriceAction.Calculator(priceAction);
            }
        }

        public List<BarItemType> GetBarItemTypes(BarItemMode barItemMode)
        {
            List<BarItemType> barItemTypeList = new List<BarItemType>();

            foreach (SessionIndicator indicator in indicators.Values)
            {
                if (indicator.BarType.Mode == barItemMode)
                {
                    barItemTypeList.Add(indicator.BarType);
                }
            }

            return barItemTypeList;
        }

        public List<BarItemType> GetBarItemTypes()
        {
            List<BarItemType> barItemTypeList = new List<BarItemType>();

            foreach (SessionIndicator indicator in indicators.Values)
            {
                 barItemTypeList.Add(indicator.BarType);                
            }

            return barItemTypeList;
        }
    }
}
