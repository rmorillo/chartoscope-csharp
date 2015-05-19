using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chartoscope.Common
{
    public class AnalyticsItem
    {
        private object instance;

        public object Instance
        {
            get { return instance; }
        }

        public Dictionary<string, MethodInfo> PlottingMethods { get; set; }
        public Dictionary<string, IndicatorPlotting> PlottingAttributes { get; set; }

        private ITimeKeyedCacheAppender cachedIndicatorAppender;

        public ITimeKeyedCacheAppender CachedIndicatorAppender
        {
            get { return cachedIndicatorAppender; }
        }

        private ISequenceKeyedCacheAppender cachedSignalAppender;

        public ISequenceKeyedCacheAppender CachedSignalAppender
        {
            get { return cachedSignalAppender; }
        }

        private AnalyticsTypeOption analyticsType;

        public AnalyticsTypeOption AnaylticsType
        {
            get { return analyticsType; }
        }

        private string identityCode;

        public string IdentityCode
        {
            get { return identityCode; }
        }
        

        public AnalyticsItem(AnalyticsTypeOption analyticsType, object instance)
        {
            this.analyticsType = analyticsType;
            this.instance = instance;            
        }
       
        public void Initialize()
        {
            if (instance is IIndicatorCore)
            {
                this.identityCode = ((IIndicatorCore)instance).IdentityCode;

                Type instanceType = instance.GetType();

                PlottingMethods = new Dictionary<string, MethodInfo>();
                PlottingAttributes = new Dictionary<string, IndicatorPlotting>();

                MethodInfo[] methods = instanceType.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    object[] attributes = method.GetCustomAttributes(typeof(IndicatorPlotting), true);
                    if (attributes.Length > 0)
                    {
                        IndicatorPlotting plotting = attributes[0] as IndicatorPlotting;
                        PlottingMethods.Add(plotting.Label, method);
                        PlottingAttributes.Add(plotting.Label, plotting);
                    }
                }
            }
            else if (instance is ISignal)
            {
                this.identityCode = ((ISignal)instance).IdentityCode;
            }
            else if (instance is IStrategy)
            {
                this.identityCode = ((IStrategy)instance).IdentityCode;
            }
        }

        public void EnableCaching(ITimeKeyedCacheAppender cacheAppender)
        {
            cacheAppender.Initialize();
            this.cachedIndicatorAppender = cacheAppender;
        }

        public void EnableCaching(ISequenceKeyedCacheAppender cacheAppender)
        {
            cacheAppender.Initialize();
            this.cachedSignalAppender = cacheAppender;
        }

        public double GetValue(string plottingLabel, int index)
        {
            object result = PlottingMethods[plottingLabel].Invoke(this.instance, new object[] { index });
            if (result == null)
                return double.NaN;
            else
                return (double)result;
        }
     

    }
}
