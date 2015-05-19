using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface IStrategyChart
    {
        void Initialize(int maxBars, int barsViewable, string cacheFolder, BarItemType barType, Guid cacheId, string signalIdentityCode);
        BarItemType SelectedTimeframe { get; set; }
        IOscillatorChart OscillatorChart { get; }
        IPercentageChart PercentageChart { get; }
        IPriceActionChart PriceActionChart { get; }
        IPipChart PipChart { get; }
        void ShowOrders(MarketOrder[] marketOrders);
        void Clear();
    }
}
