using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ISignal
    {
        string IdentityCode { get; }
        BarItemType BarType { get; }
        List<AnalyticsItem> RegisteredAnalytics { get; }
        bool IsInPosition { get; }
        bool IsOutPosition { get; }
        PositionMode Position { get; }
        event SignalDelegates.EntrySignalHandler EnterPosition;
        event SignalDelegates.ExitSignalHandler ExitPosition;
        void InPositionState(PriceBars priceBar, BarItemType barType);
        void OutPositionState(PriceBars priceBar, BarItemType barType);
        void ForceExit();
    }
}
