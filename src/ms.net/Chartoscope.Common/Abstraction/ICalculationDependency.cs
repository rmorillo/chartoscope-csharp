using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ICalculationDependency
    {
        void AddDependency(BarItemType barType, IAnalyticsIdentity identity);
        List<CalculationDependencyItem> GetDependencies();
    }
}
