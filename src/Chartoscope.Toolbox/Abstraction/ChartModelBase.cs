using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using Chartoscope.Common;

namespace Chartoscope.Toolbox
{
    public abstract class ChartModelBase
    {        
        public Series Series { get; set; }
        public ChartArea ChartArea  { get; set; }
        public SeriesCollection SeriesCollection { get; set; } 

        public ChartModelBase()
        {
            ResetDataPointBounds();
            IndexKeyForTime = new Dictionary<int, DateTime>();
            TimeKeyForIndex = new Dictionary<DateTime, int>();
        }

        public double DataPointMax { get; set; }
        public double DataPointMin { get; set; }

        public void ResetDataPointBounds()
        {
            this.DataPointMax = double.MinValue;
            this.DataPointMin = double.MaxValue;
        }

        public void AddIndicatorSeries(string indicatorName, ChartIndicatorItem[] indicator, ChartTypeOption chartType, string chartArea)
        {          
            if (this.SeriesCollection.FindByName(indicatorName) != null)
            {
                this.SeriesCollection.Remove(this.SeriesCollection.FindByName(indicatorName));
            }

            Series indicatorSeries = new Series(indicatorName);

            switch (chartType)
            {
                case ChartTypeOption.Line:
                    indicatorSeries.ChartType = SeriesChartType.Line;
                    break;
                case ChartTypeOption.Histogram:
                    indicatorSeries.ChartType = SeriesChartType.Column;
                    break;
                case ChartTypeOption.Point:
                    indicatorSeries.ChartType = SeriesChartType.Point;
                    break;
            }
            
            //indicatorSeries.IsVisibleInLegend = false;
            for (int indicatorIndex = indicator.Length - 1; indicatorIndex >= 0; indicatorIndex--)
            {
                ChartIndicatorItem indicatorItem = indicator[indicatorIndex];
                indicatorSeries.Points.AddY(indicatorItem.Value);
            }

            indicatorSeries.ChartArea = chartArea;
            this.SeriesCollection.Add(indicatorSeries);
        }

        public Dictionary<int, DateTime> IndexKeyForTime { get; set; }
        public Dictionary<DateTime, int> TimeKeyForIndex { get; set; }

        public void ChangeChartType(ChartTypeOption chartType)
        {
            switch(chartType)
            {
                case ChartTypeOption.Candlestick:
                    this.Series.ChartType = SeriesChartType.Candlestick;
                    break;
                case ChartTypeOption.Line:
                    this.Series.ChartType = SeriesChartType.Line;
                    break;
                case ChartTypeOption.PriceBar:
                    this.Series.ChartType = SeriesChartType.Stock;
                    break;
            }            
        }
    }
}
