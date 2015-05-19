using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ICoreIndicator
    {
        object IndicatorInstance { get; set; }
        IPriceActionCalculator PriceAction { get; set; }
        List<CalculationDependencyItem> Dependencies { get; set; }
    }
}
