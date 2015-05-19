using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chartoscope.Common;
using Chartoscope.Caching;

namespace Chartoscope.Toolbox
{
    public partial class StrategyChart : UserControl, IStrategyChart
    {
        private StrategyDataFrame strategyDataFrame = null;
        private StrategyFrameReader strategyFrameReader = null;
        private PricebarCache pricebarCache = null;

        public StrategyChart()
        {
            InitializeComponent();

            imlIcons.Images.Add("profit", Properties.Resources.profit);
            imlIcons.Images.Add("loss", Properties.Resources.loss);
            imlIcons.Images.Add("breakeven", Properties.Resources.breakeven);

            lvwOrders.SmallImageList = imlIcons;
            lvwOrders.Columns[0].TextAlign = HorizontalAlignment.Right;

            multiChart1.RequestUpdate += OnRequestUpdate;
            timeNavigator1.ChangeCurrentDate += OnChangeCurrentDate;
        }

        private void OnChangeCurrentDate(DateTime newDateTime)
        {
            RefreshChart(newDateTime);
        }

        private bool OnRequestUpdate(DateTime currentDateTime, DateTime startTime, DateTime endTime, ChartScrollDirectionMode scrollDirection)
        {
            if ((scrollDirection == ChartScrollDirectionMode.LeftToRight && startTime <= this.pricebarCache.StartBarDate) || (scrollDirection == ChartScrollDirectionMode.RightToLeft && endTime >= this.pricebarCache.EndBarDate))
            {
                return false;
            }
            else
            {
                RefreshChart(currentDateTime);
                return true;
            }     
        }

        private void PlotStrategies(StrategyDataItem[] stategyDataItems, string activeStrategy)
        {
            Dictionary<string, double[]> strategyEquityCurve = new Dictionary<string, double[]>();
            foreach (StrategyDataItem strategy in stategyDataItems)
            {

            }
        }

        private void RefreshChart(DateTime currentDateTime)
        {
            multiChart1.Clear();

            strategyFrameReader = strategyDataFrame.GetFrameReader(currentDateTime);
            //Get strategies.
            //Basic SDK can execute only one strategy.  Expert SDK can execute multiple strategies.
            //PlotStrategies(this.strategyDataFrame.Strategies);

            IPriceActionChart priceChart = this.PriceActionChart;

            priceChart.Show();
            priceChart.SetDataPoints(strategyFrameReader.PriceBars);

            List<ChartSignalItem> signalItems = new List<ChartSignalItem>();
            foreach (BarItem barItem in strategyFrameReader.PriceBars)
            {
                MarketOrderState marketOrderState = strategyFrameReader.Read(barItem.Time);
                if (marketOrderState != MarketOrderState.NoOrder)
                {
                    signalItems.Add(new ChartSignalItem(marketOrderState == MarketOrderState.Long ? SignalState.Long : marketOrderState == MarketOrderState.Short ? SignalState.Short : SignalState.NoSignal, barItem.Time));
                }
            }

            priceChart.PlotSignal(signalItems.ToArray());

            //foreach (IndicatorChartingInfo chartingInfo in strategyDataFrame.GetIndicatorChartingInfo())
            //{
            //    ChartIndicatorItem[] chartIndicators = strategyDataFrame.GetIndicators(chartingInfo.IdentityCode, chartingInfo.SeriesLabel);
            //    if (chartingInfo.ChartRange == ChartRangeOption.PriceActionRange)
            //    {
            //        ((IPriceActionChart)multiChart1).PlotIndicator(GetSeriesLabel(chartingInfo), chartIndicators, chartingInfo.ChartType);
            //    }
            //    else if (chartingInfo.ChartRange == ChartRangeOption.PositiveHundredRange)
            //    {
            //        if (!((IPercentageChart)multiChart1).Visible)
            //        {
            //            ((IPercentageChart)multiChart1).Show();
            //        }

            //        ((IPercentageChart)multiChart1).PlotIndicator(GetSeriesLabel(chartingInfo), chartIndicators, chartingInfo.ChartType);
            //    }
            //    else if (chartingInfo.ChartRange == ChartRangeOption.PipRange)
            //    {
            //        if (!((IOscillatorChart)multiChart1).Visible)
            //        {
            //            ((IOscillatorChart)multiChart1).Show();
            //        }

            //        ((IOscillatorChart)multiChart1).PlotIndicator(GetSeriesLabel(chartingInfo), chartIndicators, chartingInfo.ChartType);
            //    }

            //}        
        }

        public IOscillatorChart OscillatorChart
        {
            get { return this.multiChart1; }
        }

        public IPercentageChart PercentageChart
        {
            get { return this.multiChart1; }
        }

        public IPriceActionChart PriceActionChart
        {
            get { return this.multiChart1; }
        }

        public IPipChart PipChart
        {
            get { return this.multiChart1; }
        }


        public void Initialize(int maxBars, int barsViewable, string cacheFolder, BarItemType barType, Guid cacheId, string strategyIdentityCode)
        {
            multiChart1.Initialize(maxBars, barsViewable);

            this.SelectedTimeframe = barType;

            pricebarCache= new PricebarCache(barType, cacheId, CacheModeOption.Read);

            this.strategyDataFrame = new StrategyDataFrame(strategyIdentityCode, 300, pricebarCache);

            timeNavigator1.Initialize(pricebarCache.StartBarDate, pricebarCache.EndBarDate);

            strategyFrameReader = strategyDataFrame.GetFrameReader(pricebarCache.StartBarDate);

            IPriceActionChart priceChart = this.PriceActionChart;

            priceChart.Show();
            priceChart.SetDataPoints(strategyFrameReader.PriceBars);

            List<ChartSignalItem> signalItems = new List<ChartSignalItem>();
            foreach (BarItem barItem in strategyFrameReader.PriceBars)
            {
                MarketOrderState marketOrderState = strategyFrameReader.Read(barItem.Time);
                if (marketOrderState != MarketOrderState.NoOrder)
                {
                    signalItems.Add(new ChartSignalItem(marketOrderState == MarketOrderState.Long ? SignalState.Long : marketOrderState == MarketOrderState.Short ? SignalState.Short : SignalState.NoSignal, barItem.Time));
                }
            }

            priceChart.PlotSignal(signalItems.ToArray());

            //foreach (IndicatorChartingInfo chartingInfo in strategyDataFrame.GetIndicatorChartingInfo())
            //{
            //    ChartIndicatorItem[] chartIndicators = strategyDataFrame.GetIndicators(chartingInfo.IdentityCode, chartingInfo.SeriesLabel);
            //    if (chartingInfo.ChartRange == ChartRangeOption.PriceActionRange)
            //    {
            //        priceChart.PlotIndicator(GetSeriesLabel(chartingInfo), chartIndicators, chartingInfo.ChartType);
            //    }
            //    else if (chartingInfo.ChartRange == ChartRangeOption.PositiveHundredRange)
            //    {
            //        if (!PercentageChart.Visible)
            //        {
            //            PercentageChart.Show();
            //        }

            //        PercentageChart.PlotIndicator(GetSeriesLabel(chartingInfo), chartIndicators, chartingInfo.ChartType);
            //    }
            //    else if (chartingInfo.ChartRange == ChartRangeOption.PipRange)
            //    {
            //        if (!OscillatorChart.Visible)
            //        {
            //            OscillatorChart.Show();
            //        }

            //        OscillatorChart.PlotIndicator(GetSeriesLabel(chartingInfo), chartIndicators, chartingInfo.ChartType);
            //    }

            //}            
            
        }


        private string GetSeriesLabel(IndicatorChartingInfo chartingInfo)
        {
            return chartingInfo.MultiValue ? string.Concat(chartingInfo.IdentityCode, "-", chartingInfo.SeriesLabel) : chartingInfo.IdentityCode;
        }

        public BarItemType SelectedTimeframe
        {
            get
            {
                return multiChart1.SelectedTimeFrame;
            }
            set
            {
                multiChart1.SelectedTimeFrame= value;
            }
        }


        public void ShowOrders(MarketOrder[] marketOrders)
        {
            int equityPips = 0;
            chtEquityCurve.Series[0].Points.AddY(0);
            foreach (MarketOrder marketOrder in marketOrders)
            {
                if (marketOrder.ClosedTime != DateTime.MinValue)
                {
                    int profitLossPips = (int)Math.Round(marketOrder.ProfitOrLossInPips * 10000, 0);
                    equityPips += profitLossPips;
                    chtEquityCurve.Series[0].Points.AddY(equityPips);
                    ListViewItem item = lvwOrders.Items.Add(string.Empty, marketOrder.ProfitOrLossInPips > 0 ? "profit" : marketOrder.ProfitOrLossInPips < 0 ? "loss" : "breakeven");
                    item.ForeColor = marketOrder.ProfitOrLossInPips > 0 ? Color.Green : marketOrder.ProfitOrLossInPips < 0 ? Color.IndianRed : Color.DarkOrange;
                    item.SubItems.Add(profitLossPips.ToString());
                    item.SubItems.Add(Enum.GetName(typeof(PositionMode), marketOrder.Position));
                    item.SubItems.Add(marketOrder.OpeningBarTime.ToString("hh:mm tt dd-MMM-yyyy"));
                    item.SubItems.Add(marketOrder.ClosingBarTime.ToString("hh:mm tt dd-MMM-yyyy"));
                }
            }
        }


        public void Clear()
        {
            multiChart1.Clear();
        }
    }
}
