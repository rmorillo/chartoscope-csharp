using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public struct CalculationDependencyItem
    {
        private BarItemType barType;

        public BarItemType BarType
        {
            get { return barType; }
            set { barType = value; }
        }

        private IAnalyticsIdentity identity;

        public IAnalyticsIdentity Identity
        {
            get { return identity; }
            set { identity = value; }
        }

        public CalculationDependencyItem(BarItemType barType, IAnalyticsIdentity identity)
        {
            this.barType = barType;
            this.identity = identity;
        }
        
    }
}
