using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Chartoscope.Caching;
using Chartoscope.Common;
using Chartoscope.Indicators;
using Chartoscope.Signals;

namespace Chartoscope.Brokers
{
    public class SessionSignals
    {
        private Dictionary<string, SessionSignal> signals = null;
        private SessionIndicators sessionIndicators = null;

        public event SessionSignalDelegates.ExitSignalHandler ExitSignal;
        public event SessionSignalDelegates.EntrySignalHandler EntrySignal;

        private bool cachingEnabled;

        public bool CachingEnabled
        {
            get { return cachingEnabled; }
        }

        private object[] zeroParamObject = new object[] { 0 };

        private Guid cacheId = Guid.Empty;

        public SessionSignals(SessionIndicators sessionIndicators, Guid cacheId, bool cachingEnabled= true)
        {
            signals = new Dictionary<string, SessionSignal>();
            this.sessionIndicators = sessionIndicators;
            this.cacheId = cacheId;
            this.cachingEnabled = cachingEnabled;
        }

        public void Add<TSignal>(TSignal signal) where TSignal: ISignal
        {
            string barTypeCode = signal.BarType.Code;

            if (!signals.ContainsKey(barTypeCode))
            {
                signals.Add(barTypeCode, new SessionSignal(signal.BarType));                      
            }

            signal.EnterPosition += OnEntrySignal;
            signal.ExitPosition += OnExitSignal;            

            foreach (AnalyticsItem analytics in signal.RegisteredAnalytics)
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
            }

            signals[barTypeCode].CoreSignals.Add(signal.IdentityCode, new CoreSignal(signal, this.cacheId, cachingEnabled));

        }

        private void OnEntrySignal(ISignal source, long sequence, PositionMode position)
        {
            UpdateSignalCache(source.IdentityCode, source.BarType, sequence, position);

            if (EntrySignal != null)
            {
                EntrySignal(sequence, position);
            }
        }

        private void UpdateSignalCache(string identityCode, BarItemType barType, long sequence, PositionMode position)
        {
            signals[barType.Code].CoreSignals[identityCode].Cache.AppendSignal(sequence, lastPriceBar.Time, position, DateTime.Now);
        }

        private void OnExitSignal(ISignal source, long sequence)
        {
            UpdateSignalCache(source.IdentityCode, source.BarType, sequence, PositionMode.Closed);
            if (ExitSignal != null)
            {
                ExitSignal(sequence);
            }
        }

        private BarItem lastPriceBar = null;

        public void ReceivePriceAction(BarItemType barType, PriceBars priceAction)
        {
            lastPriceBar = priceAction.LastItem;

            foreach(CoreSignal core in signals[barType.Code].CoreSignals.Values)
            {
                if (this.cachingEnabled)
                {
                    UpdateCache(priceAction.LastItem.Time, core.Signal.RegisteredAnalytics);
                }

                if (core.Signal.IsInPosition)
                {
                    core.Signal.InPositionState(priceAction, barType);                    
                }
                
                if (!core.Signal.IsInPosition)
                {
                    core.Signal.OutPositionState(priceAction, barType);
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
                foreach (CoreSignal core in signals[barType.Code].CoreSignals.Values)
                {
                    core.Cache.Open(CachingModeOption.Writing);
                    foreach (AnalyticsItem analyticsItem in core.Signal.RegisteredAnalytics)
                    {
                        switch (analyticsItem.AnaylticsType)
                        {
                            case AnalyticsTypeOption.Indicator:
                                analyticsItem.CachedIndicatorAppender.Open(CachingModeOption.Writing);
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
                foreach (CoreSignal core in signals[barType.Code].CoreSignals.Values)
                {
                    core.Cache.Close();
                    foreach (AnalyticsItem analyticsItem in core.Signal.RegisteredAnalytics)
                    {
                        switch (analyticsItem.AnaylticsType)
                        {
                            case AnalyticsTypeOption.Indicator:
                                analyticsItem.CachedIndicatorAppender.Close();
                                break;
                        }

                    }
                }
            }
        }

        private void UpdateIndicatorCache(DateTime timeKey, AnalyticsItem analyticsItem)
        {
            object[] values= new object[analyticsItem.PlottingMethods.Count];
            int index= 0;
            int nanCount = 0;
            foreach(MethodInfo method in analyticsItem.PlottingMethods.Values)
            {
                double result= (double) method.Invoke(analyticsItem.Instance, zeroParamObject);      
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

            foreach (SessionSignal signal in signals.Values)
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

            foreach (SessionSignal signal in signals.Values)
            {
                barItemTypeList.Add(signal.BarType);
            }

            return barItemTypeList;
        }
    }
}
