using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Chartoscope.Brokers;
using Chartoscope.Caching;
using Chartoscope.Common;
using Chartoscope.Indicators;
using Chartoscope.Persistence;
using Chartoscope.Signals;

namespace Chartoscope.Services
{
    public class SignalCharting
    {
        private object dataStreamSource = null;
        private TickerType tickerType;
        private ISignalChart chartControl = null;
        private BarItemType barType = null;

        public BarItemType BarType { get { return barType; } }

        private BacktestSession backtestSession = null;
        BrokerAccount brokerAccount = null;

        private List<AnalyticsItem> registeredAnalytics = null;
        private Guid cacheId = Guid.Empty;

        public SignalCharting(ISignalChart chartControl, TickerType tickerType, BarItemType barType, object dataStreamSource)
        {
            this.dataStreamSource = dataStreamSource;
            this.tickerType = tickerType;
            this.barType = barType;
            this.chartControl = chartControl;
            this.registeredAnalytics = new List<AnalyticsItem>();

            SharedCacheFactory.Instance.CacheWriter = new FileCacheWriter("cache");
            SharedCacheFactory.Instance.CacheReader = new FileCacheReader("cache");

            if (dataStreamSource is string)
            {
                FileInfo file = new FileInfo(dataStreamSource.ToString());
                long fileHash= file.Attributes.GetHashCode() ^ file.CreationTime.Ticks ^ file.LastWriteTime.Ticks ^ file.Length;
                CacheConfig cacheConfig = new CacheConfig("cache");
                cacheConfig.Initialize();
                cacheConfig.Open(CachingModeOption.Reading);
                CacheRow row = cacheConfig.Read(fileHash);                
                cacheConfig.Close();

                if (row == null)
                {
                    cacheConfig.Open(CachingModeOption.Writing);
                    cacheId= Guid.NewGuid();
                    cacheConfig.Append(fileHash, cacheId, SessionModeOption.Backtesting);
                    cacheConfig.Close();
                }
                else
                {
                    cacheId = new Guid((byte[])row["SessionId"]);
                }
            }
            
        }

        public void Register(object analyticsInstance)
        {
            AnalyticsItem analyticsItem= null;
            if (analyticsInstance is IIndicatorCore)
            {
                analyticsItem = new AnalyticsItem(AnalyticsTypeOption.Indicator, analyticsInstance);
                analyticsItem.Initialize();
                registeredAnalytics.Add(analyticsItem);
            }

            if (analyticsInstance is ISignal)
            {
                analyticsItem = new AnalyticsItem(AnalyticsTypeOption.Signal, analyticsInstance);
                analyticsItem.Initialize();
                registeredAnalytics.Add(analyticsItem);
            }
        }

        private int barsPause = 0;
        private int barCount = 0;

        private Dictionary<string, Dictionary<string, IndicatorPlotting>> GetIndicatorPlotting()
        {
            Dictionary<string, Dictionary<string, IndicatorPlotting>> indicatorPlotting = new Dictionary<string, Dictionary<string, IndicatorPlotting>>();
            foreach (AnalyticsItem item in registeredAnalytics)
            {
                if (item.AnaylticsType == AnalyticsTypeOption.Signal)
                {
                    ISignal signal = item.Instance as ISignal;
                    foreach (AnalyticsItem signalItem in signal.RegisteredAnalytics)
                    {
                        if (signalItem.AnaylticsType == AnalyticsTypeOption.Indicator)
                        {
                            indicatorPlotting.Add(signalItem.IdentityCode, signalItem.PlottingAttributes);
                        }
                    }
                }
            }

            return indicatorPlotting;
        }

        private Dictionary<string, Dictionary<string, List<ChartIndicatorItem>>> GetChartIndicators(Dictionary<string, Dictionary<string, IndicatorPlotting>> indicatorPlotting)
        {
            Dictionary<string, Dictionary<string, List<ChartIndicatorItem>>> chartIndicators = new Dictionary<string, Dictionary<string, List<ChartIndicatorItem>>>();
            foreach (KeyValuePair<string, Dictionary<string, IndicatorPlotting>> plotting in indicatorPlotting)
            {
                Dictionary<string, List<ChartIndicatorItem>> item= new Dictionary<string,List<ChartIndicatorItem>>();
                foreach(string key in plotting.Value.Keys)
                {
                    item.Add(key, new List<ChartIndicatorItem>());
                }

                chartIndicators.Add(plotting.Key, item);
            }

            return chartIndicators;
        }

        public void Refresh()
        {
            chartControl.Initialize(200, 100, "cache", barType, cacheId, registeredAnalytics[0].IdentityCode);            

            //chartControl.SelectedTimeframe = this.barType;

            //IPriceActionChart priceChart = chartControl.PriceActionChart;
            //IPercentageChart percentChart = chartControl.PercentageChart;
            //IOscillatorChart oscillatorChart = chartControl.OscillatorChart;

            //List<BarItem> barItems= new List<BarItem>();
            //PriceBars priceBars= backtestSession.GetPriceBars(this.barType);            
            //Dictionary<string, Dictionary<string, IndicatorPlotting>> indicatorPlotting = GetIndicatorPlotting();
            //Dictionary<string, Dictionary<string, List<ChartIndicatorItem>>> chartIndicatorItems = GetChartIndicators(indicatorPlotting);
            
            //BarItem priceBar = priceBars.LastItem;
            //int index = 0;
            //while (priceBar!=null)                
            //{
            //    barItems.Add(priceBar);

            //    foreach (AnalyticsItem item in registeredAnalytics)
            //    {
            //        if (item.AnaylticsType == AnalyticsTypeOption.Signal)
            //        {
            //            ISignal signal = item.Instance as ISignal;
            //            foreach (AnalyticsItem signalItem in signal.RegisteredAnalytics)
            //            {
            //                if (signalItem.AnaylticsType == AnalyticsTypeOption.Indicator)
            //                {
            //                    foreach (KeyValuePair<string, MethodInfo> method in signalItem.PlottingMethods)
            //                    {
            //                        object result = method.Value.Invoke(signalItem.Instance, new object[] { index });
            //                        if (result != null && !double.IsNaN((double)result))
            //                        {
            //                            chartIndicatorItems[signalItem.IdentityCode][method.Key].Add(new ChartIndicatorItem(priceBars.LastItem.Time, (double)result));
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    index++;
            //    priceBar = priceBars.Last(index);                
            //}

            //barItems.Reverse();

            //priceChart.Show();
            //priceChart.SetDataPoints(barItems.ToArray());

            //bool percentChartPlotted = false;
            //bool oscillatorChartPlotted = false;

            //foreach (KeyValuePair<string, Dictionary<string, IndicatorPlotting>> plotting in indicatorPlotting)
            //{
            //    foreach (KeyValuePair<string, IndicatorPlotting> indicator in plotting.Value)
            //    {
            //        List<ChartIndicatorItem> chartItemList = chartIndicatorItems[plotting.Key][indicator.Key];
            //        chartItemList.Reverse();
                    
            //        string chartLabel = null;
            //        if (plotting.Value.Count > 1)
            //            chartLabel = string.Concat(plotting.Key, "-", indicator.Key);
            //        else
            //            chartLabel = plotting.Key;

            //        if (indicator.Value.ChartRange == ChartRangeOption.PriceActionRange)
            //        {                       
            //            priceChart.PlotIndicator(chartLabel, chartItemList.ToArray(), indicator.Value.ChartType);
            //        }

            //        if (indicator.Value.ChartRange == ChartRangeOption.PositiveHundredRange)
            //        {
            //            percentChart.PlotIndicator(chartLabel, chartItemList.ToArray(), indicator.Value.ChartType);
            //            percentChartPlotted = true;
            //        }

            //        if (indicator.Value.ChartRange == ChartRangeOption.PipRange)
            //        {                        
            //            oscillatorChart.PlotIndicator(chartLabel, chartItemList.ToArray(), indicator.Value.ChartType);
            //            oscillatorChartPlotted = true;
            //        }
            //    }
            //}

            //List<ChartSignalItem> signalItems = new List<ChartSignalItem>();
            //foreach (MarketOrder marketOrder in brokerAccount.Orders.GetOrders())
            //{
            //    signalItems.Add(new ChartSignalItem(marketOrder.Position == PositionMode.Long ? SignalStateOption.Buy : SignalStateOption.Sell, marketOrder.OpeningPriceBar.Time));
            //    signalItems.Add(new ChartSignalItem(SignalStateOption.NoSignal, marketOrder.ClosingPriceBar.Time));
            //}

            //priceChart.PlotSignal(signalItems.ToArray());

            //if (percentChartPlotted)
            //{
            //    percentChart.Show();
            //}

            //if (oscillatorChartPlotted)
            //{
            //    oscillatorChart.Show();
            //}

            //chartControl.ShowOrders(brokerAccount.Orders.GetOrders());

            //double profit = brokerAccount.Orders.GetTotalProfit(SpotForex.EURUSD);
            //double loss = brokerAccount.Orders.GetTotalLoss(SpotForex.EURUSD);
            //int winningTrades = brokerAccount.Orders.GetTotalWinningOrders(SpotForex.EURUSD);
            //int losingTrades = brokerAccount.Orders.GetTotalLosingOrders(SpotForex.EURUSD);
            //int breakevenTrades = brokerAccount.Orders.GetTotalBreakevenOrders(SpotForex.EURUSD);

        }

        public void Start(int barsPause=0)
        {
            this.barsPause = barsPause;

            if (dataStreamSource is string)
            {
                string barDataFile = dataStreamSource as string;

                BacktestBroker backtestBroker = new BacktestBroker();

                brokerAccount = new BrokerAccount();

                backtestBroker.LoadAccount(brokerAccount, cacheId);

                backtestSession = backtestBroker.CreateSession(brokerAccount.AccountId, tickerType, barType, dataStreamSource.ToString(), cacheId);
                     
                foreach(AnalyticsItem item in registeredAnalytics)
                {
                    if (item.AnaylticsType==AnalyticsTypeOption.Indicator)
                    {
                        backtestSession.Indicators.Add<IIndicatorCore>(item.Instance as IIndicatorCore);
                    }

                    if (item.AnaylticsType == AnalyticsTypeOption.Signal)
                    {
                        backtestSession.Signals.Add<ISignal>(item.Instance as ISignal);
                    }
                }

                barCount = 0;

                backtestSession.NewBar += OnNewBar;

                backtestSession.Signals.OpenCache(barType);

                backtestSession.Start();

                backtestSession.Signals.CloseCache(barType);


                //SignalCacheNavigator signalCacheNavigator = new SignalCacheNavigator("cache", barType, cacheId, registeredAnalytics[0].IdentityCode);

                //ignalDataFrame signalDataFrame= signalCacheNavigator.GetSignalDataFrame(signalCacheNavigator.StartBarDate, 300);

                chartControl.Initialize(200, 100, "cache", barType, cacheId, registeredAnalytics[0].IdentityCode);                                

                if (barsPause == 0)
                {
                    //Refresh();
                }

                //double profit= brokerAccount.Orders.GetTotalProfit(SpotForex.EURUSD);
                //double loss = brokerAccount.Orders.GetTotalLoss(SpotForex.EURUSD);
            }
        }

        private void OnNewBar(BarItemType barItemType, BarItem barItem)
        {
            barCount++;
            if (barCount == barsPause)
            {
                backtestSession.Pause();
                Refresh();
            }
        }
    }
}
