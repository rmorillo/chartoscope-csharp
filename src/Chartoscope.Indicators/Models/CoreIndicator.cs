using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Caching;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class CoreIndicator: ICoreIndicator
    {
        public object IndicatorInstance { get; set; }
        public IPriceActionCalculator PriceAction { get; set; }
        public List<CalculationDependencyItem> Dependencies { get; set; }
        

        public CoreIndicator(object instance)
        {
            this.IndicatorInstance = instance;
            this.PriceAction = instance as IPriceActionCalculator;
            this.Dependencies = ((ICalculationDependency)instance).GetDependencies();            
        }
    }
}
