using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class IndicatorPlotting: Attribute
    {
        private string label;

        public string Label
        {
            get { return label; }
        }

        private ChartTypeOption chartType;

        public ChartTypeOption ChartType
        {
            get { return chartType; }
        }

        private ChartRangeOption chartRange;

        public ChartRangeOption ChartRange
        {
            get { return chartRange; }
        }

        private bool showByDefault;

        public bool ShowByDefault
        {
            get { return showByDefault; }
        }
        
        public IndicatorPlotting(string label, ChartTypeOption chartType, ChartRangeOption chartRange, bool showByDefault=true)
        {
            this.chartType = chartType;
            this.chartRange = chartRange;
            this.label = label;
            this.showByDefault = showByDefault;
        }
    }
}
