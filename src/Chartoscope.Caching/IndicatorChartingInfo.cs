using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class IndicatorChartingInfo
    {
        private ChartRangeOption chartRange;

        public ChartRangeOption ChartRange
        {
            get { return chartRange; }
        }

        private ChartTypeOption chartType;

        public ChartTypeOption ChartType
        {
            get { return chartType; }
        }

        private string seriesLabel;

        public string SeriesLabel
        {
            get { return seriesLabel; }
            set { seriesLabel = value; }
        }

        private int indicatorIndex;

        public int IndicatorIndex
        {
            get { return indicatorIndex; }
        }

        private string identityCode;

        public string IdentityCode
        {
            get { return identityCode; }
        }

        private bool multiValue;

        public bool MultiValue
        {
            get { return multiValue; }
        }


        public IndicatorChartingInfo(int indicatorIndex, string identityCode, string seriesLabel, ChartRangeOption chartRange, ChartTypeOption chartType, bool multiValue= false)
        {
            this.indicatorIndex = indicatorIndex;
            this.identityCode = identityCode;
            this.seriesLabel = seriesLabel;
            this.chartRange = chartRange;
            this.chartType = chartType;
            this.multiValue = multiValue;
        }
    }
}
