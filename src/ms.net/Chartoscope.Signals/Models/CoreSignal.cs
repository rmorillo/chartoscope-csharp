using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Caching;
using Chartoscope.Common;

namespace Chartoscope.Signals
{
    public class CoreSignal
    {
        public object SignalInstance { get; set; }
        public ISignal Signal { get; set; }

        private Guid cacheId = Guid.Empty;
        private bool cachingEnabled = true;

        public SignalCache Cache = null;

        public CoreSignal(object instance, Guid cacheId, bool cachingEnabled= true)
        {
            this.SignalInstance = instance;
            this.Signal = instance as ISignal;
            this.cacheId = cacheId;
            this.cachingEnabled = cachingEnabled;

            if (cachingEnabled)
            {
                this.Cache = new SignalCache(instance, cacheId);
                this.Cache.Initialize();
            }
        }
    }
}
