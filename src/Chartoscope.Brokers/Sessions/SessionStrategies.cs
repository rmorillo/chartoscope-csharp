using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Chartoscope.Caching;
using Chartoscope.Common;
using Chartoscope.Signals;
using Chartoscope.Strategies;

namespace Chartoscope.Brokers
{
    public class SessionStrategies
    {
        private Dictionary<string, SessionStrategy> strategies = null;
        private SessionIndicators sessionIndicators = null;
        private SessionSignals sessionSignals = null;

        public event SessionStrategyDelegates.ExitPositionHandler ExitPosition;
        public event SessionStrategyDelegates.EntryPositionHandler EntryPosition;

        private bool cachingEnabled;

        public bool CachingEnabled
        {
            get { return cachingEnabled; }
        }

        private object[] zeroParamObject = new object[] { 0 };

        private Guid cacheId = Guid.Empty;

        public SessionStrategies(SessionSignals sessionSignals, SessionIndicators sessionIndicators, Guid cacheId, bool cachingEnabled = true)
        {
            strategies = new Dictionary<string, SessionStrategy>();
            this.sessionSignals = sessionSignals;
            this.sessionIndicators = sessionIndicators;
            this.cacheId = cacheId;
            this.cachingEnabled = cachingEnabled;
        }

        public void Add<TStrategy>(TStrategy strategy) where TStrategy : IStrategy
        {
            string barTypeCode = strategy.BarType.Code;

            if (!strategies.ContainsKey(barTypeCode))
            {
                strategies.Add(barTypeCode, new SessionStrategy(strategy.BarType));
            }

            strategy.EnterPosition += OnEntryStrategy;
            strategy.ExitPosition += OnExitStrategy;

            foreach (AnalyticsItem analytics in strategy.RegisteredAnalytics)
            {
                if (analytics.AnaylticsType == AnalyticsTypeOption.Indicator)
                {
                    sessionIndicators.Add<IIndicatorCore>(analytics.Instance as IIndicatorCore);
                    analytics.Initialize();

                    if (this.cachingEnabled)
                    {
                        analytics.EnableCaching(new IndicatorCache(analytics.Instance, analytics.PlottingAttributes.Values.ToArray<IndicatorPlotting>(), cacheId, true));
                    }
                }

                if (analytics.AnaylticsType == AnalyticsTypeOption.Signal)
                {
                    sessionSignals.Add<ISignal>(analytics.Instance as ISignal);
                    analytics.Initialize();

                    if (this.cachingEnabled)
                    {
                        analytics.EnableCaching(new SignalCache(analytics.Instance, cacheId));
                    }
                }
            }

            strategies[barTypeCode].CoreStrategies.Add(strategy.IdentityCode, new CoreStrategy(strategy, this.cacheId, cachingEnabled));

        }

        private void OnEntryStrategy(IStrategy source, long sequence, PositionMode position)
        {
            UpdateStrategyCache(source.IdentityCode, source.BarType, sequence, position);

            if (EntryPosition != null)
            {
                EntryPosition(sequence, position);
            }
        }

        private void UpdateStrategyCache(string identityCode, BarItemType barType, long sequence, PositionMode position)
        {
            strategies[barType.Code].CoreStrategies[identityCode].Cache.AppendStrategy(sequence, lastPriceBar.Time, position, DateTime.Now);
        }

        private void OnExitStrategy(IStrategy source, long sequence)
        {
            UpdateStrategyCache(source.IdentityCode, source.BarType, sequence, PositionMode.Closed);
            if (ExitPosition != null)
            {
                ExitPosition(sequence);
            }
        }

        private BarItem lastPriceBar = null;

        public void ReceivePriceAction(BarItemType barType, PriceBars priceAction)
        {
            lastPriceBar = priceAction.LastItem;

            foreach (CoreStrategy core in strategies[barType.Code].CoreStrategies.Values)
            {
                if (this.cachingEnabled)
                {
                    UpdateCache(priceAction.LastItem.Time, core.Strategy.RegisteredAnalytics);
                }

                if (core.Strategy.IsInPosition)
                {
                    core.Strategy.InPositionState(priceAction, barType);
                }

                if (!core.Strategy.IsInPosition)
                {
                    core.Strategy.OutPositionState(priceAction, barType);
                }
            }
        }

        public void RecieveQuote(double bidPrice, double askPrice)
        {
            foreach (SessionStrategy strategy in strategies.Values)
            {
                foreach(CoreStrategy core in strategy.CoreStrategies.Values)
                {
                    core.Strategy.NewQuote(bidPrice, askPrice);
                }
            }
        }

        private void UpdateCache(DateTime timeKey, List<AnalyticsItem> registeredAnalytics)
        {
            if (this.cachingEnabled)
            {
                foreach (AnalyticsItem analyticsItem in registeredAnalytics)
                {
                    switch (analyticsItem.AnaylticsType)
                    {
                        case AnalyticsTypeOption.Indicator:
                            UpdateIndicatorCache(timeKey, analyticsItem);
                            break;
                    }

                }
            }
        }

        public void OpenCache(BarItemType barType)
        {
            if (this.cachingEnabled)
            {
                foreach (CoreStrategy core in strategies[barType.Code].CoreStrategies.Values)
                {
                    core.Cache.Open(CachingModeOption.Writing);
                    foreach (AnalyticsItem analyticsItem in core.Strategy.RegisteredAnalytics)
                    {
                        switch (analyticsItem.AnaylticsType)
                        {
                            case AnalyticsTypeOption.Indicator:
                                analyticsItem.CachedIndicatorAppender.Open(CachingModeOption.Writing);
                                break;
                            case AnalyticsTypeOption.Signal:
                                analyticsItem.CachedSignalAppender.Open(CachingModeOption.Writing);
                                break;
                        }

                    }
                }
            }
        }

        public void CloseCache(BarItemType barType)
        {
            if (this.cachingEnabled)
            {
                foreach (CoreStrategy core in strategies[barType.Code].CoreStrategies.Values)
                {
                    core.Cache.Close();
                    foreach (AnalyticsItem analyticsItem in core.Strategy.RegisteredAnalytics)
                    {
                        switch (analyticsItem.AnaylticsType)
                        {
                            case AnalyticsTypeOption.Indicator:
                                analyticsItem.CachedIndicatorAppender.Close();
                                break;
                            case AnalyticsTypeOption.Signal:
                                sessionSignals.CloseCache(barType);
                                break;
                        }

                    }
                }
            }
        }

        private void UpdateIndicatorCache(DateTime timeKey, AnalyticsItem analyticsItem)
        {
            object[] values = new object[analyticsItem.PlottingMethods.Count];
            int index = 0;
            int nanCount = 0;
            foreach (MethodInfo method in analyticsItem.PlottingMethods.Values)
            {
                double result = (double)method.Invoke(analyticsItem.Instance, zeroParamObject);
                values[index] = result;

                if (double.IsNaN(result))
                {
                    nanCount++;
                }

                index++;
            }

            if (nanCount != index)
            {
                analyticsItem.CachedIndicatorAppender.Append(timeKey, values);
            }
        }


        public List<BarItemType> GetBarItemTypes(BarItemMode barItemMode)
        {
            List<BarItemType> barItemTypeList = new List<BarItemType>();

            foreach (SessionStrategy signal in strategies.Values)
            {
                if (signal.BarType.Mode == barItemMode)
                {
                    barItemTypeList.Add(signal.BarType);
                }
            }

            return barItemTypeList;
        }

        public List<BarItemType> GetBarItemTypes()
        {
            List<BarItemType> barItemTypeList = new List<BarItemType>();

            foreach (SessionStrategy signal in strategies.Values)
            {
                barItemTypeList.Add(signal.BarType);
            }

            return barItemTypeList;
        }
    }
}
