using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chartoscope.Common;

namespace Chartoscope.Common
{
    public interface IPriceActionChart
    {
        bool Visible { get; }
        void Show();
        void Hide();
        void SetDataPoints(BarItem[] barItems);
        void PlotIndicator(string indicatorName, ChartIndicatorItem[] indicatorItems, ChartTypeOption chartType);
        void PlotSignal(ChartSignalItem[] chartSignalItems);
        void PlotStrategy(ChartStrategyItem[] chartStrategyItems);
        void SelectIndicator(string indicatorName);
        void UnselectIndicator(string indicatorName);
        void UnplotIndicator(string indicatorName);
        void ChangeChartType(ChartTypeOption chartType);
    }
}
