using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chartoscope.Common;

namespace Chartoscope.Common
{
    public interface IOscillatorChart
    {
        bool Visible { get; }
        void Show();
        void Hide();
        void PlotIndicator(string indicatorName, ChartIndicatorItem[] indicatorItems, ChartTypeOption chartType);
        void UnplotIndicator(string indicatorName);
    }
}
