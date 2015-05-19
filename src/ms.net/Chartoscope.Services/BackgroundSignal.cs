using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Chartoscope.Brokers;
using Chartoscope.Common;
using Chartoscope.Indicators;
using Chartoscope.Signals;

namespace Chartoscope.Services
{
    public class BackgroundSignal
    {
        private object dataStreamSource = null;
        private TickerType tickerType;
        private BarItemType barType = null;

        public BarItemType BarType { get { return barType; } }

        private BacktestSession backtestSession = null;

        private List<AnalyticsItem> registeredAnalytics = null;
        private Guid cacheId = Guid.Empty;

        public BackgroundSignal(TickerType tickerType, BarItemType barType, object dataStreamSource)
        {
            this.dataStreamSource = dataStreamSource;
            this.tickerType = tickerType;
            this.barType = barType;
            this.registeredAnalytics = new List<AnalyticsItem>();
        }

        public void Register(object analyticsInstance)
        {
            if (analyticsInstance is IIndicatorCore)
            {
                registeredAnalytics.Add(new AnalyticsItem(AnalyticsTypeOption.Indicator, analyticsInstance));
            }

            if (analyticsInstance is ISignal)
            {
                registeredAnalytics.Add(new AnalyticsItem(AnalyticsTypeOption.Signal, analyticsInstance));
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
       

        public BrokerAccount Start(int barsPause=0)
        {
            this.barsPause = barsPause;
            BrokerAccount brokerAccount = null;

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

                    //if (item.AnaylticsType == AnalyticsTypeOption.Strategy)
                    //{
                    //    backtestSession.Strategies.Add<IStrategy>(item.Instance as IStrategy);
                    //}
                }

                barCount = 0;

                backtestSession.NewBar += OnNewBar;
            
                backtestSession.Start();                
            }

            return brokerAccount;
        }

        private void OnNewBar(BarItemType barItemType, BarItem barItem)
        {
            barCount++;
            if (barCount == barsPause)
            {
                backtestSession.Pause();
            }
        }
    }
}
