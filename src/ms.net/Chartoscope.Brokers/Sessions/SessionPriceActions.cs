using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Brokers
{
    public class SessionPriceActions
    {
        private Dictionary<string, BarItemType> barItemTypes = null;
        private Dictionary<string, PriceBars> priceActions = null;

        public SessionPriceActions(List<BarItemType> barItemTypes)
        {
            this.barItemTypes = new Dictionary<string, BarItemType>();
            this.priceActions = new Dictionary<string, PriceBars>();

            foreach (BarItemType barItemType in barItemTypes)
            {
                this.barItemTypes.Add(barItemType.Code, barItemType);
                this.priceActions.Add(barItemType.Code, new PriceBars(barItemType));
            }
        }

        public void ReceiveBarItem(BarItemType barType, BarItem barItem)
        {
            this.priceActions[barType.Code].Add(barItem);
        }

        public PriceBars GetPriceAction(BarItemType barItemType)
        {
            return this.priceActions[barItemType.Code];
        }
    }
}
