using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface IPriceActionCalculator
    {
        void Calculator(PriceBars priceAction);
        bool HasCalculated(PriceBars priceAction);
    }
}
