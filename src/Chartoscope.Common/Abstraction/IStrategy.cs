using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface IStrategy
    {
        string IdentityCode { get; }
        BarItemType BarType { get; }
        List<AnalyticsItem> RegisteredAnalytics { get; }
        bool IsInPosition { get; }
        PositionMode Position { get; }
        event StrategyDelegates.EntryStrategyHandler EnterPosition;
        event StrategyDelegates.ExitStrategyHandler ExitPosition;
        void InPositionState(PriceBars priceBar, BarItemType barType);
        void OutPositionState(PriceBars priceBar, BarItemType barType);
        void NewQuote(double bidPrice, double askPrice);
    }
}
