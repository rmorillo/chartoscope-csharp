using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Caching;
using Chartoscope.Common;

namespace Chartoscope.Strategies
{
    public class CoreStrategy
    {
        public object SignalInstance { get; set; }
        public IStrategy Strategy { get; set; }

        private Guid cacheId = Guid.Empty;
        private bool cachingEnabled = true;

        public StrategyCache Cache = null;

        public CoreStrategy(object instance, Guid cacheId, bool cachingEnabled = true)
        {
            this.SignalInstance = instance;
            this.Strategy = instance as IStrategy;
            this.cacheId = cacheId;
            this.cachingEnabled = cachingEnabled;

            if (cachingEnabled)
            {
                this.Cache = new StrategyCache(instance, cacheId);
                this.Cache.Initialize();
            }
        }
    }
}
